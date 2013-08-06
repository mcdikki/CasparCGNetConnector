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

Friend Class ByeCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("BYE", "Disconnects from the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "BYE"
    End Function

    Public Overrides Function execute(ByRef connection As CasparCGConnection) As CasparCGResponse
        ' Bye disconnects from the server and does not reply, so no response
        ' will be returned. This gives the need to send it via sendAsync and fake a response
        connection.sendAsyncCommand(getCommandString)
        Return New CasparCGResponse("202 BYE OK", getCommandString)
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
