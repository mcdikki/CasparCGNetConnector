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


''' <summary>
''' This FailoverCasparCGConnection bundles two separte CasparCGConnections and always send commands to both.
''' There is a master and a slave connection. While commands are send to both, only the result of the master is routed to the caller.
''' If the master fails, the slave will over take over control.
''' In any case of failure, master or slave, an event will be rissen.
''' </summary>
''' <remarks></remarks>
Public Class FailoverCasparCGConnection
    Implements ICasparCGConnection

    Private connectionLock As Semaphore
    Private _checkInterval As Integer = 500
    Private tryConnect As Boolean = False
    Private ccgVersion As String = "-1.-1.-1"
    Private channels As Integer = 0

    Private master As ICasparCGConnection = Nothing
    Private slave As ICasparCGConnection = Nothing

    Private masterTask As Task
    Private slaveTask As Task

    Public Property failoverMode As failoverModes

    ''' <summary>
    ''' Reads or sets the number of retires to perform if a connection can't be established
    ''' </summary>
    Public Property reconnectTries As Integer = 1 Implements ICasparCGConnection.reconnectTries
    ''' <summary>
    ''' Reads or sets the number of milliseconds to wait between two connection attempts
    ''' </summary>
    Public Property reconnectTimeout As Integer = 1000 Implements ICasparCGConnection.reconnectTimeout ' 1sec
    ''' <summary>
    ''' Reads or sets the number of milliseconds to wait for incoming data before stop reading
    ''' </summary>
    Public Property timeout As Integer = 3000 Implements ICasparCGConnection.timeout ' ms to wait for data before cancel receive

    ''' <summary>
    ''' Gets or sets whether strict version control is activated. If True, only commands that pass the isCompatible() check will be executed.
    ''' </summary>
    Public Property strictVersionControl As Boolean = True Implements ICasparCGConnection.strictVersionControl

    Public Property disconnectOnTimeout As Boolean = True Implements ICasparCGConnection.disconnectOnTimeout

    ''' <summary>
    ''' Reads or sets the interval in milliseconds the connection status will be checked.
    ''' </summary>
    ''' <value>The number of milliseconds between connection checks</value>
    ''' <returns>The number of milliseconds between connection checks</returns>
    Public Property checkInterval As Integer Implements ICasparCGConnection.checkInterval
        Get
            Return _checkInterval
        End Get
        Set(value As Integer)
            _checkInterval = value
            If Not master Is Nothing Then
                master.checkInterval = checkInterval
            ElseIf Not slave Is Nothing Then
                slave.checkInterval = checkInterval
            End If
        End Set
    End Property


#Region "enums"
    Public Enum connectionRoles
        master
        slave
        both
    End Enum

    ''' <summary>
    ''' The mode controls the way the connection is handled.
    '''     master_slave: one connection is the main one. The command succeeds if it succeeds on the master. Errors on the slave will be reported but no execption will be thrown. The connection only waits for the response of the master. If the master fails, the slave takes the master role and a error event will be raised, but no exception will be thrown.
    '''     masrer_master: both connections are equal. Commands are successfull if they are on both. Any error will be forwarded to the user just as it would be a normal connection.
    '''     slidingMaster: if one connection succeeds, the command succeeds. The first response will be reported. So the master is changing to the fastest all the time. Errors are reported and exceptions are thrown if both connections does fail.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum failoverModes
        master_slave
        master_master
        slidingMaster
    End Enum
#End Region


#Region "Events"
    ''' <summary>
    ''' Fires if this connection has been disconnected.
    ''' </summary>
    ''' <param name="sender"></param>
    Public Event disconnected(ByRef sender As Object) Implements ICasparCGConnection.disconnected

    ''' <summary>
    ''' Fires if this connection has been connected to the remote host.
    ''' </summary>
    ''' <param name="sender"></param>
    Public Event connected(ByRef sender As Object) Implements ICasparCGConnection.connected

    ''' <summary>
    ''' Fires if an error accures in one of the connections
    ''' </summary>
    ''' <param name="sender">The FailoverCasparCGConnection which is the source of this event</param>
    ''' <param name="args">Additionl information about what happend</param>
    Public Event connectionFailed(ByRef sender As Object, ByVal args As FailoverConnectionFailedEventArgs)
#End Region


    ''' <summary>
    ''' Creates a new CasparCGConnection to the given serverAddress and serverPort
    ''' </summary>
    ''' <param name="masterServerAddress">the server ip or hostname of the master server</param>
    ''' <param name="masterServerPort">the master servers port</param>
    ''' <param name="slaveServerAddress">the server ip or hostname of the slave server</param>
    ''' <param name="slaveServerPort">the slave servers port</param>
    Public Sub New(ByVal masterServerAddress As String, ByVal masterServerPort As Integer, ByVal slaveServerAddress As String, ByVal slaveServerPort As Integer)
        connectionLock = New Semaphore(1, 1)
        master = New CasparCGConnection(masterServerAddress, masterServerPort)
        slave = New CasparCGConnection(slaveServerAddress, slaveServerPort)
    End Sub

    ''' <summary>
    ''' Creates a new CasparCGConnection to the given serverAddress and the default port
    ''' </summary>
    ''' <param name="masterServerAddress">the server ip or hostname of the master server</param>
    ''' <param name="slaveServerAddress">the server ip or hostname of the slave server</param>
    Public Sub New(ByVal masterServerAddress As String, ByVal slaveServerAddress As String)
        Me.New(masterServerAddress, 5250, slaveServerAddress, 5250)
    End Sub

    ''' <summary>
    ''' Creates a new CasparCGConnection to localhost and the default port
    ''' </summary>
    Public Sub New(ByRef masterConnection As ICasparCGConnection, ByRef slaveConnection As ICasparCGConnection)
        connectionLock = New Semaphore(1, 1)
        master = masterConnection
        slave = slaveConnection
    End Sub

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    ''' <returns>true, if and only if the connection is established, false otherwise</returns>
    Public Function connect() As Boolean Implements ICasparCGConnection.connect

        If Not isConnected() Then
            If Not master Is Nothing AndAlso Not master.isConnected Then
                master.connect()
            End If
            If Not slave Is Nothing AndAlso Not slave.isConnected Then
                slave.connect()
            End If
        ElseIf Not master Is Nothing AndAlso Not slave Is Nothing Then
            logger.log("FailoverCasparCGConnection.connect: Already connected to " & master.getServerAddress & ":" & master.getServerPort.ToString & " / " & slave.getServerAddress & ":" & slave.getServerPort.ToString)
            'connectionLock.Release()
        Else
            If master Is Nothing Then
                logger.warn("FailoverCasparCGConnection.connect: Warning: master connection not set!")
            Else
                logger.warn("FailoverCasparCGConnection.connect: Warning: slave connection not set!")
            End If
        End If
        Return isConnected()
    End Function

    ''' <summary>
    ''' Return whether or not the FailoverCasparCGConnection is connect to the server which means one of master or client is connected. If tryConnect is given and true, it will try to establish a connection if not Already connected.
    ''' </summary>
    ''' <param name="tryConnect"></param>
    ''' <returns>true, if and only if the connection is established, false otherwise</returns>
    Public Function isConnected(Optional ByVal tryConnect As Boolean = False) As Boolean Implements ICasparCGConnection.isConnected
        Return (Not master Is Nothing AndAlso master.isConnected(tryConnect)) Or (Not slave Is Nothing AndAlso slave.isConnected(tryConnect))
    End Function

    ''' <summary>
    ''' Disconnects and closes the connection to the CasparCG Server
    ''' </summary>
    Public Sub close() Implements ICasparCGConnection.close
        If Not master Is Nothing Then master.close()
        If Not slave Is Nothing Then slave.close()
        closed()
    End Sub

    Private Sub closed()
        ccgVersion = "0.0.0"
        channels = 0
        logger.log("FailoverCasparCGConnection.closed: Disconnected from server.")
        RaiseEvent disconnected(Me)
    End Sub


    ''' <summary>
    ''' Returns whether or not the connected CasparCG Server supports OSC
    ''' </summary>
    ''' <returns>true, if and only if a connection is established and the sever supports OSC</returns>
    Public Function isOSCSupported() As Boolean Implements ICasparCGConnection.isOSCSupported
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


    ''' <summary>
    ''' Returns the version string of the connected CasparCG Server
    ''' </summary>
    ''' <returns>The version of the connected server or 0.0.0 if not connected</returns>
    Public Function getVersion() As String Implements ICasparCGConnection.getVersion
        If Not master Is Nothing Then
            Return master.getVersion
        ElseIf Not slave Is Nothing Then
            Return slave.getVersion
        Else
            Return ccgVersion
        End If
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
    Public Function getVersionPart(part As Integer, Optional Version As String = "") As Integer Implements ICasparCGConnection.getVersionPart
        If Version = "" Then Version = getVersion()
        Dim v() = Version.Split(".".ToCharArray()(0))
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
    Public Function getServerChannels() As Integer Implements ICasparCGConnection.getServerChannels
        Return channels
    End Function


    ''' <summary>
    ''' Sends a command to the casparCG server and returns imediatly after sending no matter if the command was accepted or not.
    ''' </summary>
    ''' <param name="cmd"></param>
    Public Sub sendAsyncCommand(ByVal cmd As String) Implements ICasparCGConnection.sendAsyncCommand
        If isConnected(tryConnect) Then
            connectionLock.WaitOne(timeout)
            '' TODO
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
    Public Function sendCommand(ByVal cmd As String) As CasparCGResponse Implements ICasparCGConnection.sendCommand
        If isConnected(tryConnect) Then
            ''TODO
            Dim masterResponse As CasparCGResponse
            Dim slaveResponse As CasparCGResponse

            Try
                masterTask = Task.Factory.StartNew(New Action(Sub() masterResponse = master.sendCommand(cmd)))
                slaveTask = Task.Factory.StartNew(New Action(Sub() slaveResponse = slave.sendCommand(cmd)))

                Select Case failoverMode
                    Case failoverModes.master_slave
                        masterTask.Wait()

                    Case failoverModes.master_master

                    Case failoverModes.slidingMaster

                End Select

            Catch ex As Exception

            End Try
        Else
            logger.err("FailoverCasparCGConnection.sendCommand: Not connected to server. Can't send command.")
            RaiseEvent connectionFailed(Me, New FailoverConnectionFailedEventArgs(Me, connectionRoles.both, FailoverConnectionFailedEventArgs.reasons.NOT_CONNECTED))
            Return New CasparCGResponse("000 NOT_CONNECTED_ERROR", cmd)
        End If
    End Function

    ''' <summary>
    ''' Returns the CasparCG Server address this connection is using
    ''' </summary>
    ''' <returns>The IP or DNS address of this connection</returns>
    ''' <remarks></remarks>
    Public Function getServerAddress() As String Implements ICasparCGConnection.getServerAddress
        If Not master Is Nothing Then
            Return master.getServerAddress
        ElseIf Not slave Is Nothing Then
            Return slave.getServerAddress
        Else
            Return "UNKNOWN_ADDRESS"
        End If
    End Function

    ''' <summary>
    ''' Returns the port number this connection is using
    ''' </summary>
    ''' <returns>The TCP port nummber of this connection</returns>
    ''' <remarks></remarks>
    Public Function getServerPort() As Integer Implements ICasparCGConnection.getServerPort
        If Not master Is Nothing Then
            Return master.getServerPort
        ElseIf Not slave Is Nothing Then
            Return slave.getServerPort
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' Sets the address on which this connection tries to connect to the casparCG Server.
    ''' </summary>
    ''' <param name="serverAddress">The IP or DNS address of the server</param>
    ''' <returns>True if this connection is not connected and the address could be set, False otherwise. </returns>
    ''' <remarks></remarks>
    Public Function setServerAddress(ByVal serverAddress As String) As Boolean Implements ICasparCGConnection.setServerAddress
        If Not master Is Nothing AndAlso Not master.isConnected Then
            Return master.setServerAddress(serverAddress)
        ElseIf Not slave Is Nothing AndAlso slave.isConnected Then
            Return slave.setServerAddress(serverAddress)
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Sets the port on which this connection tries to connect to the casparCG Server.
    ''' </summary>
    ''' <param name="serverPort">The TCP port number</param>
    ''' <returns>True if this connection is not connected and the port could be set, False otherwise. </returns>
    ''' <remarks></remarks>
    Public Function setServerPort(ByVal serverPort As Integer) As Boolean Implements ICasparCGConnection.setServerPort
        If Not master Is Nothing AndAlso Not master.isConnected Then
            Return master.setServerPort(serverPort)
        ElseIf Not slave Is Nothing AndAlso slave.isConnected Then
            Return slave.setServerPort(serverPort)
        Else
            Return False
        End If
    End Function


    ''' <summary>
    ''' Returns whether or not a layer of a channel is free, which means no producer is playing on it.
    ''' </summary>
    ''' <param name="layer"></param>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isLayerFree(ByVal layer As Integer, ByVal channel As Integer, Optional ByVal onlyForeground As Boolean = False, Optional ByVal onlyBackground As Boolean = False) As Boolean Implements ICasparCGConnection.isLayerFree
        If isConnected() Then
            Dim info As New InfoCommand(channel, layer, onlyBackground, onlyForeground)
            Dim doc As New Xml.XmlDocument()
            If info.execute(Me).isOK Then
                doc.LoadXml(info.getResponse.getXMLData)
                For Each type As Xml.XmlNode In doc.GetElementsByTagName("type")
                    If Not type.InnerText.Equals("empty-producer") Then
                        Return False
                    End If
                Next
                Return True
            Else
                logger.warn("ServerController.isLayerFree: Could not check layer. Server response was incorrect.")
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
    Public Function getFreeLayer(ByVal channel As Integer) As Integer Implements ICasparCGConnection.getFreeLayer
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
                If Not master Is Nothing Then master.Dispose()
                If Not slave Is Nothing Then slave.Dispose()

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

Public Class FailoverConnectionFailedEventArgs
    Public Property connection As ICasparCGConnection
    Public Property connectionRole As FailoverCasparCGConnection.connectionRoles
    Public Property reason As reasons = reasons.UNKNOWN

    Public Enum reasons
        UNKNOWN = -1

        NOT_CONNECTED = 0

        DISCONNECTED = 1
        MASTER_DISCONNECTED = 11
        SLAVE_DISCONNECTED = 12

        COMMAND_NOT_SUPPORTED = 2
        MASTER_COMMAND_NOT_SUPPORTED = 21
        SLAVE_COMMAND_NOT_SUPPORTED = 22

        TIMEOUT = 3
        MASTER_TIMEOUT = 31
        SLAVE_TIMEOUT = 32

        RESPONSE_CODE_NOT_EQUAL = 4
        RESPONSE_DATA_NOT_EQUAL = 5

        UNKNOWN_EXCEPTION = 9
        MASTER_UNKNOWN_EXCEPTION = 91
        SLAVE_UNKNOWN_EXCEPTION = 92
    End Enum

    Public Sub New(ByRef connection As ICasparCGConnection, ByVal connectionRole As FailoverCasparCGConnection.connectionRoles, ByVal reason As reasons)
        Me.connection = connection
        Me.connectionRole = connectionRole
        Me.reason = reason
    End Sub
End Class