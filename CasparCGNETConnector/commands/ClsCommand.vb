Public Class ClsCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("CLS", "Requests a list of all media files on the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "CLS"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
