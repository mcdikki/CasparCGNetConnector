Public Class VersionServerCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("VERSION SERVER", "Requests current server version")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "VERSION SERVER"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
