Public Class VersionFlashCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("VERSION FLASH", "Requests current flash version on the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "VERSION FLASH"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
