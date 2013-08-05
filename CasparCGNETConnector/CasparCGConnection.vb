Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Public Class CasparCGConnection
    Private connectionLock As Semaphore
    Private serveraddress As String = "localhost"
    Private serverport As Integer = 5250 ' std. acmp2 port
    Private client As TcpClient
    Private reconnectTries = 1
    Private connectionAttemp = 0
    Private reconnectTimeout = 1000 ' 1sec
    Private buffersize As Integer = 1024 * 256
    Private tryConnect As Boolean = False
    Private timeout As Integer = 300 ' ms to wait for data before cancel receive
    Private ccgVersion As String = "0.0.0"

    Public Sub New(ByVal serverAddress As String, ByVal serverPort As Integer)
        connectionLock = New Semaphore(1, 1)
        Me.serveraddress = serverAddress
        Me.serverport = serverPort
        client = New TcpClient()
        client.SendBufferSize = buffersize
        client.ReceiveBufferSize = buffersize
        client.NoDelay = True
    End Sub

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    Public Function connect() As Boolean
        If Not client.Connected Then
            Try
                client.Connect(serveraddress, serverport)
                client.NoDelay = True
                If client.Connected Then
                    connectionAttemp = 0
                    logger.log("CasparCGConnection.connect: Connected to " & serveraddress & ":" & serverport.ToString)
                    ccgVersion = readServerVersion()
                End If
            Catch e As Exception
                logger.warn(e.Message)
                If connectionAttemp < reconnectTries Then
                    connectionAttemp = connectionAttemp + 1
                    logger.warn("CasparCGConnection.connect: Try to reconnect " & connectionAttemp & "/" & reconnectTries)
                    Dim i As Integer = 0
                    Dim sw As New Stopwatch
                    sw.Start()
                    While sw.ElapsedMilliseconds < reconnectTimeout
                    End While
                    Return connect()
                Else
                    logger.err("CasparCGConnection.connect: Could not connect to " & serveraddress & ":" & serverport.ToString)
                    Return False
                End If
            End Try
        Else
            logger.log("CasparCGConnection.connect: Allready connected to " & serveraddress & ":" & serverport.ToString)
        End If
        Return client.Connected
    End Function

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    ''' <param name="serverAddress">the server ip or hostname</param>
    ''' <param name="serverPort">the servers port</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function connect(ByVal serverAddress As String, ByVal serverPort As Integer) As Boolean
        Me.serveraddress = serverAddress
        Me.serverport = serverPort
        Return connect()
    End Function

    ''' <summary>
    ''' Return whether or not the CasparCGConnection is connect to the server. If tryConnect is given and true, it will try to establish a connection if not allready connected.
    ''' </summary>
    ''' <param name="tryConnect"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function connected(Optional ByVal tryConnect As Boolean = False) As Boolean
        If client.Connected Then
            Return True
        Else
            If tryConnect Then
                connect()
            End If
            Return client.Connected
        End If
    End Function

    Public Sub close()
        If client.Connected Then
            client.Client.Close()
        End If
    End Sub

    Public Function isOSCSupported() As Boolean
        If getVersionPart(0) = 2 Then
            If getVersionPart(1) = 0 Then
                If getVersionPart(2) <= 3 Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If
        ElseIf getVersionPart(0) < 2 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function isThumbnailSupported() As Boolean
        Return isOSCSupported()
    End Function

    Private Function readServerVersion() As String
        If connected() Then
            Dim response = sendCommand(CasparCGCommandFactory.getVersion)
            If Not IsNothing(response) AndAlso response.isOK Then
                Return response.getData
            End If
        End If
        Return "0.0.0"
    End Function

    Public Function getVersion() As String
        Return ccgVersion
    End Function

    Public Function getVersionPart(part As Integer, Optional Version As String = "") As Integer
        If Version = "" Then Version = getVersion()
        Dim v() = Version.Split(".")
        If v.Length >= part Then
            Dim r As Integer
            If Integer.TryParse(v(part), r) Then
                Return r
            End If
        End If
        Return -1
    End Function


    ''' <summary>
    ''' Sends a command to the casparCG server and returns imediatly after sending no matter if the command was accepted or not.
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <remarks></remarks>
    Public Sub sendAsyncCommand(ByVal cmd As String)
        If connected(tryConnect) Then
            connectionLock.WaitOne()
            logger.debug("CasparCGConnection.sendAsyncCommand: Send command: " & cmd)
            client.GetStream.Write(System.Text.UTF8Encoding.UTF8.GetBytes(cmd & vbCrLf), 0, cmd.Length + 2)
            logger.debug("CasparCGConnection.sendAsyncCommand: Command sent")
            connectionLock.Release()
        Else : logger.err("CasparCGConnection.sendAsyncCommand: Not connected to server. Can't send command.")
        End If
    End Sub

    ''' <summary>
    ''' Sends a command to the casparCG server and returns a CasparCGResonse.
    ''' sendCommand will wait until it receives a returncode. So it may stay longer inside the function.
    ''' If the given commandstring has more than one casparCG command, the response will be only for one of those!
    ''' </summary>
    ''' <param name="cmd"></param>
    Public Function sendCommand(ByVal cmd As String) As CasparCGResponse
        If connected(tryConnect) Then
            connectionLock.WaitOne()
            Dim buffer() As Byte

            ' flush old buffers in case we had some asyncSends
            If client.Available > 0 Then
                ReDim buffer(client.Available)
                client.GetStream.Read(buffer, 0, client.Available)
            End If

            ' send cmd
            logger.debug("CasparCGConnection.sendCommand: Send command: " & cmd)
            client.GetStream.Write(System.Text.UTF8Encoding.UTF8.GetBytes(cmd & vbCrLf), 0, cmd.Length + 2)
            Dim timer As New Stopwatch
            timer.Start()

            ' Waiting for the response:
            Dim input As String = ""
            Dim size As Integer = 0
            Try
                '                                                                                                                                                                                                                                                                                                         '' Version BUGFIX    201 THUMBNAIL RETRIEVE OK
                Do Until (input.Trim().Length > 3) AndAlso (((input.Trim().Substring(0, 3) = "201" OrElse input.Trim().Substring(0, 3) = "200") AndAlso (input.EndsWith(vbLf & vbCrLf) OrElse input.EndsWith(vbCrLf & " " & vbCrLf))) OrElse (input.Trim().Substring(0, 3) <> "201" AndAlso input.Trim().Substring(0, 3) <> "200" AndAlso input.EndsWith(vbCrLf)) OrElse (input.Trim().Length > 16 AndAlso input.Trim().Substring(0, 14) = "201 VERSION OK" AndAlso input.EndsWith(vbCrLf)) OrElse (input.Trim().Length > 27 AndAlso input.Trim().Substring(0, 25) = "201 THUMBNAIL RETRIEVE OK" AndAlso input.EndsWith(vbCrLf)))
                    If client.Available > 0 Then
                        size = client.Available
                        ReDim buffer(size)
                        client.GetStream.Read(buffer, 0, size)
                        input = input & System.Text.UTF8Encoding.UTF8.GetString(buffer, 0, size)
                    End If
                Loop
            Catch e As Exception
                logger.err("CasparCGConnection.sendCommand: Error: " & e.Message)
            End Try
            timer.Stop()
            logger.debug("CasparCGConnection.sendCommand: Waited " & timer.ElapsedMilliseconds & "ms for an answer and received " & input.Length & " Bytes to read.")
            connectionLock.Release()
            logger.debug("CasparCGConnection.sendCommand: Received response for '" & cmd & "': " & input)
            Return New CasparCGResponse(input, cmd)
        Else
            logger.err("CasparCGConnection.sendCommand: Not connected to server. Can't send command.")
            Return Nothing
        End If
    End Function

End Class