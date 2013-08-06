Public Class ThumbnailListCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("THUMBNAIL LIST", "Requests a list of all thumbnails on the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "THUMBNAIL LIST"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
