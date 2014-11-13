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


Public Interface ICasparCGConnection
    Inherits IDisposable


    ''' <summary>
    ''' Reads or sets the number of retires to perform if a connection can't be established
    ''' </summary>
    Property reconnectTries As Integer
    ''' <summary>
    ''' Reads or sets the number of milliseconds to wait between two connection attempts
    ''' </summary>
    Property reconnectTimeout As Integer
    ''' <summary>
    ''' Reads or sets the number of milliseconds to wait for incoming data before stop reading
    ''' </summary>
    Property timeout As Integer

    ''' <summary>
    ''' Gets or sets whether strict version control is activated. If True, only commands that pass the isCompatible() check will be executed.
    ''' </summary>
    Property strictVersionControl As Boolean

    Property disconnectOnTimeout As Boolean

    ''' <summary>
    ''' Reads or sets the interval in milliseconds the connection status will be checked.
    ''' </summary>
    ''' <value>The number of milliseconds between connection checks</value>
    ''' <returns>The number of milliseconds between connection checks</returns>
    Property checkInterval As Integer

    ''' <summary>
    ''' Fires if this connection has been disconnected.
    ''' </summary>
    ''' <param name="sender"></param>
    Event disconnected(ByRef sender As Object)
    ''' <summary>
    ''' Fires if this connection has been connected to the remote host.
    ''' </summary>
    ''' <param name="sender"></param>
    Event connected(ByRef sender As Object)

    ''' <summary>
    ''' Connects to the given server and port and returns true if a connection could be established and false otherwise.
    ''' </summary>
    ''' <returns>true, if and only if the connection is established, false otherwise</returns>
    ''' <remarks></remarks>
    Function connect() As Boolean


    ''' <summary>
    ''' Return whether or not the CasparCGConnection is connect to the server. If tryConnect is given and true, it will try to establish a connection if not Already connected.
    ''' </summary>
    ''' <param name="tryConnect"></param>
    ''' <returns>true, if and only if the connection is established, false otherwise</returns>
    ''' <remarks></remarks>
    Function isConnected(Optional ByVal tryConnect As Boolean = False) As Boolean


    ''' <summary>
    ''' Disconnects and closes the connection to the CasparCG Server
    ''' </summary>
    ''' <remarks></remarks>
    Sub close()


    ''' <summary>
    ''' Returns whether or not the connected CasparCG Server supports OSC
    ''' </summary>
    ''' <returns>true, if and only if a connection is established and the sever supports OSC</returns>
    ''' <remarks></remarks>
    Function isOSCSupported() As Boolean


    ''' <summary>
    ''' Returns the version string of the connected CasparCG Server
    ''' </summary>
    ''' <returns>The version of the connected server or 0.0.0 if not connected</returns>
    ''' <remarks></remarks>
    Function getVersion() As String


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
    Function getVersionPart(part As Integer, Optional Version As String = "") As Integer


    ''' <summary>
    ''' Returns the number of channels on the connected CasparCG Server
    ''' </summary>
    ''' <returns>The number of channels</returns>
    ''' <remarks></remarks>
    Function getServerChannels() As Integer


    ''' <summary>
    ''' Sends a command to the casparCG server and returns imediatly after sending no matter if the command was accepted or not.
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <remarks></remarks>
    Sub sendAsyncCommand(ByVal cmd As String)


    ''' <summary>
    ''' Sends a command to the casparCG server and returns a CasparCGResonse.
    ''' sendCommand will wait until it receives a returncode. So it may stay longer inside the function.
    ''' If the given commandstring has more than one casparCG command, the response will be only for one of those!
    ''' </summary>
    ''' <param name="cmd"></param>
    Function sendCommand(ByVal cmd As String) As CasparCGResponse


    ''' <summary>
    ''' Returns the CasparCG Server address this connection is using
    ''' </summary>
    ''' <returns>The IP or DNS address of this connection</returns>
    ''' <remarks></remarks>
    Function getServerAddress() As String


    ''' <summary>
    ''' Returns the port number this connection is using
    ''' </summary>
    ''' <returns>The TCP port nummber of this connection</returns>
    ''' <remarks></remarks>
    Function getServerPort() As Integer


    ''' <summary>
    ''' Sets the address on which this connection tries to connect to the casparCG Server.
    ''' </summary>
    ''' <param name="serverAddress">The IP or DNS address of the server</param>
    ''' <returns>True if this connection is not connected and the address could be set, False otherwise. </returns>
    ''' <remarks></remarks>
    Function setServerAddress(ByVal serverAddress As String) As Boolean


    ''' <summary>
    ''' Sets the port on which this connection tries to connect to the casparCG Server.
    ''' </summary>
    ''' <param name="serverPort">The TCP port number</param>
    ''' <returns>True if this connection is not connected and the port could be set, False otherwise. </returns>
    ''' <remarks></remarks>
    Function setServerPort(ByVal serverPort As Integer) As Boolean


    ''' <summary>
    ''' Returns whether or not a layer of a channel is free, which means no producer is playing on it.
    ''' </summary>
    ''' <param name="layer"></param>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function isLayerFree(ByVal layer As Integer, ByVal channel As Integer, Optional ByVal onlyForeground As Boolean = False, Optional ByVal onlyBackground As Boolean = False) As Boolean


    ''' <summary>
    ''' Returns the smallest free layer of the given channel
    ''' </summary>
    ''' <param name="channel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getFreeLayer(ByVal channel As Integer) As Integer

End Interface