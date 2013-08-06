Public Class VersionTemplatehostCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("VERSION TEMPLATEHOST", "Requests current templatehost version of the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "VERSION TEMPLATEHOST"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
