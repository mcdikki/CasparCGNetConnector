Public Class TlsCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("TLS", "Requests a list of all templates on the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "TLS"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
