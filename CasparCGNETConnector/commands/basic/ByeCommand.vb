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
