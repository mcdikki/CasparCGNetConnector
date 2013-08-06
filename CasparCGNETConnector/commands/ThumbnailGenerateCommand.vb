Public Class ThumbnailGenerateCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("THUMBNAIL GENERATE", "Requests the server to (re-)generate the thumbnails for a specific media file")
    End Sub

    Public Sub New(ByVal media As String)
        MyBase.New("THUMBNAIL GENERATE", "Requests the server to (re-)generate the thumbnails for a specific media file")
        InitParameter()
        DirectCast(getParameter("media"), CommandParameter(Of String)).setValue(media)
    End Sub

    Public Sub New(ByVal media As CasparCGMedia)
        MyBase.New("THUMBNAIL GENERATE", "Requests the server to (re-)generate the thumbnails for a specific media file")
        InitParameter()
        DirectCast(getParameter("media"), CommandParameter(Of String)).setValue(media.getFullName)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("media", "The media file", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd = "THUMBNAIL GENERATE " & DirectCast(getParameter("media"), CommandParameter(Of String)).getValue
        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
