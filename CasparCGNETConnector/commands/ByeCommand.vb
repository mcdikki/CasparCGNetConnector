Friend Class ByeCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("BYE", "Disconnects from the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "BYE"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
