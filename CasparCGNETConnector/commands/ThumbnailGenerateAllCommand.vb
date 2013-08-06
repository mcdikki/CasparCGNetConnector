Public Class ThumbnailGenerateAllCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("THUMBNAIL GENERATE_ALL", "Requests the server to (re-)generate thumbnails for all media files")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "THUMBNAIL GENERATE_ALL"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
