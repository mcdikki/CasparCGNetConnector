'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'' Author: Christopher Diekkamp
'' Email: christopher@development.diekkamp.de
'' GitHub: https://github.com/mcdikki
'' 
'' This software is licensed under the 
'' GNU General Public License Version 3 (GPLv3).
'' See http://www.gnu.org/licenses/gpl-3.0-standalone.html 
'' for a copy of the license.
''
'' You are free to copy, use and modify this software.
'' Please let me know of any changes and improvements you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System.Net
Imports System.Net.Sockets
Imports System.Threading


Public Class CasparCGConnection
    Implements IDisposable

    Private connectionLock As Semaphore
    Private serveraddress As String = "localhost"
    Private serverport As Integer = 5250 ' std. acmp2 port
    Private client As TcpClient
    Private connectionAttemp = 0
    Private _checkInterval As Integer = 500
    Private buffersize As Integer = 1024 * 256
    Private tryConnect As Boolean = False
    Private ccgVersion As String = "-1.-1.-1"
    Private channels As Integer = 0
    Private WithEvents timer As New Timers.Timer(_checkInterval)

    ''' <summary>
    ''' Reads or sets the number of retires to perform if a connection can't be established
    ''' </summary>
    Public Property reconnectTries As Integer = 1
    ''' <summary>
    ''' Reads or sets the number of milliseconds to wait between two connection attempts
    ''' </summary>
    Public Property reconnectTimeout As Integer = 1000 ' 1sec
    ''' <summary>
    ''' Reads or sets the number of milliseconds to wait for incoming data before stop reading
    ''' </summary>
    Public Property timeout As Integer = 300 ' ms to wait for data before cancel receive

    ''' <summary>
    ''' Gets or sets whether strict version control is activated. If True, only commands that pass the isCompatible() check will be executed.
    ''' </summary>
    Public Property strictVersionControl As Boolean = True

    Public Property disconnectOnTimeout As Boolean = True

    ''' <summary>
    ''' Reads or sets the interval in milliseconds the connection status will be checked.
    ''' </summary>
    ''' <value>The number of milliseconds between connection checks</value>
    ''' <returns>The number of milliseconds between connection checks</returns>
    Public Property checkInterval As Integer
        Get
            Return _checkInterval
        End Get
        Set(value As Integer)
            _checkInterval = value
            timer.Interval = _checkInterval
        End Set
    End Property

    ''' <summary>
    ''' Fires if this connection has been disconnected.
    ''' </summary>
    ''' <param name="sender"></param>
    Public Event disconnected(ByRef sender As Object)
    ''' <summary>
    ''' Fires if this connection has been connected to the remote host.
    ''' </summary>
    ''' <param name="sender"></param>
    Public Event connected(ByRef sender As Object)


    ''' <summary>
    ''' Creates a new CasparCGConnection to the given serverAddress and serverPort
    ''' </summary>
    ''' <param name="serverAddress">the server ip or hostname</param>
    ''' <param name="serverPort">the servers port</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serverAddress As String, ByVal serverPort As Integer)
        connectionLock = New Semaphore(1, 1)
        Me.serveraddress = serverAddress
        Me.serverport = serverPort
        client = New TcpClient()
        client.SendBufferSize = buffersize
        client.ReceiveBufferSize = buffersize
        client.ReceiveTimeout = timeout
        client.SendTimeout = timeout
        client.NoDelay = True
    End Sub

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    ''' <returns>true, if and only if the connection is established, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function connect() As Boolean
        If Not isConnected() Then
            Try
                'client.Close()
                client = New TcpClient()
                client.SendBufferSize = buffersize
                client.ReceiveBufferSize = buffersize
                client.ReceiveTimeout = timeout
                client.SendTimeout = timeout
                client.NoDelay = True
                client.Connect(serveraddress, serverport)
                If client.Connected Then
                    timer.Start()
                    connectionAttemp = 0
                    logger.log("CasparCGConnection.connect: Connected to " & serveraddress & ":" & serverport.ToString)
                    ccgVersion = readServerVersion()
                    channels = readServerChannels()
                    RaiseEvent connected(Me)
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
        Return isConnected()
    End Function

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    ''' <param name="serverAddress">the server ip or hostname</param>
    ''' <param name="serverPort">the servers port</param>
    ''' <returns>true, if and only if the connection is established, false otherwise</returns>
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
    ''' <returns>true, if and only if the connection is established, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function isConnected(Optional ByVal tryConnect As Boolean = False) As Boolean
        If client.Connected Then 'AndAlso client.Client.Poll(20, SelectMode.SelectWrite) AndAlso Not client.Client.Poll(20, SelectMode.SelectError) Then
            Return True
        Else
            If tryConnect Then
                Return connect()
            Else
                Return False
            End If
        End If
    End Function

    ''' <summary>
    ''' Disconnects and closes the connection to the CasparCG Server
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub close()
        If isConnected() Then
            Dim bye As New ByeCommand()
            bye.execute(Me)
        End If
        closed()
    End Sub

    Private Sub closed()
        timer.Stop()
        client.Close()
        RaiseEvent disconnected(Me)
        ccgVersion = "0.0.0"
        channels = 0
    End Sub

    Private Sub checkConnection() Handles timer.Elapsed
        timer.Stop()
        If client.Connected AndAlso client.Client.Poll(checkInterval, SelectMode.SelectWrite) AndAlso Not client.Client.Poll(checkInterval, SelectMode.SelectError) Then
            Dim blockingState As Boolean = client.Client.Blocking
            Try
                Dim tmp(1) As Byte
                client.Client.Blocking = False
                If Not client.Client.Receive(tmp, 0, SocketFlags.Peek) = 0 Then
                    Exit Sub
                End If
            Catch e As SocketException
                If e.NativeErrorCode.Equals(10035) Then
                    Exit Sub
                End If
            Catch e As Exception
            Finally
                client.Client.Blocking = blockingState
                timer.Start()
            End Try
        End If
        close()
    End Sub

    ''' <summary>
    ''' Returns whether or not the connected CasparCG Server supports OSC
    ''' </summary>
    ''' <returns>true, if and only if a connection is established and the sever supports OSC</returns>
    ''' <remarks></remarks>
    Public Function isOSCSupported() As Boolean
        If My.Computer.Info.OSFullName.Contains("Windows XP") Then
            logger.log("CasparCGConnection.isOSCSupported: Dected Windows XP. OSC is not supported on WinXP.")
            Return False
        ElseIf getVersionPart(0) = 2 Then
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

    Private Function readServerVersion() As String
        If isConnected() Then
            Dim cmd As New VersionServerCommand()
            Dim tmpStrict As Boolean = strictVersionControl()
            strictVersionControl = False
            If cmd.execute(Me).isOK Then
                Return cmd.getResponse.getData
            End If
            strictVersionControl = tmpStrict
        End If
        Return "-1.-1.-1"
    End Function

    ''' <summary>
    ''' Returns the version string of the connected CasparCG Server
    ''' </summary>
    ''' <returns>The version of the connected server or 0.0.0 if not connected</returns>
    ''' <remarks></remarks>
    Public Function getVersion() As String
        Return ccgVersion
    End Function

    ''' <summary>
    ''' Returns a specific part of the version number. 
    ''' e.g.: If the version is 2.0.1 Beta3, you would get 
    ''' getVersionPart(0) = 2
    ''' getVersionPart(2) = 1
    ''' getVersionPart(3) = -1
    ''' </summary>
    ''' <param name="part">The part of the version starting by 0</param>
    ''' <param name="Version">Optional version string to get the part form. If not set, the version of the connected server will be parsed</param>
    ''' <returns>The numberical part of the version or -1 if the part is not pressent or not numerical</returns>
    ''' <remarks></remarks>
    Public Function getVersionPart(part As Integer, Optional Version As String = "") As Integer
        If Version = "" Then Version = getVersion()
        Dim v() = Version.Split(".")
        If part > -1 AndAlso v.Length >= part Then
            Dim r As Integer
            If Integer.TryParse(v(part), r) Then
                Return r
            End If
        End If
        Return -1
    End Function

    Private Function readServerChannels() As Integer
        Dim ch As Integer = 0
        If isConnected() Then
            Dim cmd As New InfoCommand()
            If Not IsNothing(cmd.execute(Me)) AndAlso cmd.getResponse.isOK Then
                Dim lineArray() = cmd.getResponse.getData.Split(vbLf)
                If Not IsNothing(lineArray) Then
                    ch = lineArray.Length
                End If
            End If
        End If
        Return ch
    End Function

    ''' <summary>
    ''' Returns the number of channels on the connected CasparCG Server
    ''' </summary>
    ''' <returns>The number of channels</returns>
    ''' <remarks></remarks>
    Public Function getServerChannels() As Integer
        Return channels
    End Function


    ''' <summary>
    ''' Sends a command to the casparCG server and returns imediatly after sending no matter if the command was accepted or not.
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <remarks></remarks>
    Public Sub sendAsyncCommand(ByVal cmd As String)
        If isConnected(tryConnect) Then
            connectionLock.WaitOne(timeout)
            logger.debug("CasparCGConnection.sendAsyncCommand: Send command: " & cmd)
            Dim buffer() As Byte = System.Text.Encoding.UTF8.GetBytes(cmd & vbCrLf)
            Try
                client.GetStream.Write(buffer, 0, buffer.Length)
                logger.debug("CasparCGConnection.sendAsyncCommand: Command sent")
            Catch e As Exception
                logger.err("CasparCGConnection.sendAsyncCommand: Error sending the command: " & cmd)
                logger.err("CasparCGConnection.sendAsyncCommand: Error was: " & e.Message)
            End Try
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
        If isConnected(tryConnect) Then
            connectionLock.WaitOne()
            Dim buffer() As Byte

            ' flush old buffers in case we had some asyncSends
            If client.Available > 0 Then
                ReDim buffer(client.Available)
                client.GetStream.Read(buffer, 0, client.Available)
            End If

            ' send cmd
            logger.debug("CasparCGConnection.sendCommand: Send command: " & cmd)

            buffer = System.Text.Encoding.UTF8.GetBytes(cmd & vbCrLf)
            client.GetStream.Write(buffer, 0, buffer.Length)
            Dim timer, timeouttimer As New Stopwatch
            timer.Start()

            ' Waiting for the response:
            Dim input As String = ""
            Dim size As Integer = 0
            Try
                '                                                                                                                                                                                                                                                                                                         '' Version BUGFIX    201 THUMBNAIL RETRIEVE OK
                Do Until (input.Trim().Length > 3) AndAlso (((input.Trim().Substring(0, 3) = "201" OrElse input.Trim().Substring(0, 3) = "200") AndAlso (input.EndsWith(vbLf & vbCrLf) OrElse input.EndsWith(vbCrLf & " " & vbCrLf))) OrElse (input.Trim().Substring(0, 3) <> "201" AndAlso input.Trim().Substring(0, 3) <> "200" AndAlso input.EndsWith(vbCrLf)) OrElse (input.Trim().Length > 16 AndAlso input.Trim().Substring(0, 14) = "201 VERSION OK" AndAlso input.EndsWith(vbCrLf)) OrElse (input.Trim().Length > 27 AndAlso input.Trim().Substring(0, 25) = "201 THUMBNAIL RETRIEVE OK" AndAlso input.EndsWith(vbCrLf)))
                    If client.Available > 0 Then
                        timeouttimer.Stop()
                        size = client.Available
                        ReDim buffer(size)
                        client.GetStream.Read(buffer, 0, size)
                        input = input & System.Text.Encoding.UTF8.GetString(buffer, 0, size)
                    Else
                        If timeouttimer.ElapsedMilliseconds > timeout Then
                            Throw New TimeoutException("The remote host took to long for an answer. Timeout after " & timeout & "ms.")
                        End If
                        If Not timeouttimer.IsRunning Then timeouttimer.Restart()
                    End If
                Loop

                timer.Stop()
                logger.debug("CasparCGConnection.sendCommand: Waited " & timer.ElapsedMilliseconds & "ms for an answer and received " & input.Length & " Bytes to read.")
                connectionLock.Release()
                logger.debug("CasparCGConnection.sendCommand: Received response for '" & cmd & "': " & input)
                Return New CasparCGResponse(input, cmd)

            Catch e As Exception
                logger.err("CasparCGConnection.sendCommand: Error: " & e.Message)
                logger.debug("CasparCGConnection.sendCommand: So far reveived from server:" & vbNewLine & input)
                If disconnectOnTimeout Then
                    closed()
                    connectionLock.Release()
                    Return New CasparCGResponse("000 NOT_CONNECTED_ERROR", cmd)
                Else
                    connectionLock.Release()
                    Return New CasparCGResponse("000 TIMEOUT", cmd)
                End If
            End Try
        Else
            logger.err("CasparCGConnection.sendCommand: Not connected to server. Can't send command.")
            Return New CasparCGResponse("000 NOT_CONNECTED_ERROR", cmd)
        End If
    End Function

    ''' <summary>
    ''' Returns the CasparCG Server address this connection is using
    ''' </summary>
    ''' <returns>The IP or DNS address of this connection</returns>
    ''' <remarks></remarks>
    Public Function getServerAddress() As String
        Return serveraddress
    End Function

    ''' <summary>
    ''' Returns the port number this connection is using
    ''' </summary>
    ''' <returns>The TCP port nummber of this connection</returns>
    ''' <remarks></remarks>
    Public Function getServerPort() As Integer
        Return serverport
    End Function

    ''' <summary>
    ''' Sets the address on which this connection tries to connect to the casparCG Server.
    ''' </summary>
    ''' <param name="serverAddress">The IP or DNS address of the server</param>
    ''' <returns>True if this connection is not connected and the address could be set, False otherwise. </returns>
    ''' <remarks></remarks>
    Public Function setServerAddress(ByVal serverAddress As String) As Boolean
        If Not isConnected() Then
            Me.serveraddress = serverAddress
            Return True
        Else : Return False
        End If
    End Function

    ''' <summary>
    ''' Sets the port on which this connection tries to connect to the casparCG Server.
    ''' </summary>
    ''' <param name="serverPort">The TCP port number</param>
    ''' <returns>True if this connection is not connected and the port could be set, False otherwise. </returns>
    ''' <remarks></remarks>
    Public Function setServerPort(ByVal serverPort As Integer) As Boolean
        If Not isConnected() Then
            Me.serverport = serverPort
            Return True
        Else : Return False
        End If
    End Function


    ''' <summary>
    ''' Returns whether or not a layer of a channel is free, which means no producer is playing on it.
    ''' </summary>
    ''' <param name="layer"></param>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isLayerFree(ByVal layer As Integer, ByVal channel As Integer, Optional ByVal onlyForeground As Boolean = False, Optional ByVal onlyBackground As Boolean = False) As Boolean
        If isConnected() Then
            Dim info As New InfoCommand(channel, layer, onlyBackground, onlyForeground)
            Dim doc As New MSXML2.DOMDocument()
            If info.execute(Me).isOK AndAlso doc.loadXML(info.getResponse.getXMLData) Then
                For Each type As MSXML2.IXMLDOMNode In doc.getElementsByTagName("type")
                    If Not type.nodeTypedValue.Equals("empty-producer") Then
                        Return False
                    End If
                Next
                Return True
            Else
                If Not IsNothing(doc.parseError) Then
                    logger.warn("ServerController.isLayerFree: Error checking layer." & vbNewLine & doc.parseError.reason & vbNewLine & doc.parseError.line & ":" & doc.parseError.linepos & vbNewLine & doc.parseError.srcText)
                    logger.warn("Server command and response was: " & info.getResponse.getCommand & vbNewLine & info.getResponse.getServerMessage)
                Else
                    logger.warn("ServerController.isLayerFree: Could not check layer. Server response was incorrect.")
                End If
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' Returns the smallest free layer of the given channel
    ''' </summary>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getFreeLayer(ByVal channel As Integer) As Integer
        Dim layer As Integer = Integer.MaxValue
        While Not isLayerFree(layer, channel) AndAlso layer > 0
            layer = layer - 1
        End While
        Return layer
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Verwalteten Zustand löschen (verwaltete Objekte).
                close()
                client = Nothing
                connectionLock.Dispose()
                connectionLock = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(disposing As Boolean) Bereinigungscode ein.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class