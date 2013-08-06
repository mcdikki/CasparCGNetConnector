Public Class CinfCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("CINF", "Requests details of a media file on the server")
    End Sub

    Public Sub New(ByVal media As String)
        MyBase.New("CINF", "Requests details of a media file on the server")
        InitParameter()
        DirectCast(getParameter("media"), CommandParameter(Of String)).setValue(media)
    End Sub

    Public Sub New(ByVal media As CasparCGMedia)
        MyBase.New("CINF", "Requests details of a media file on the server")
        InitParameter()
        DirectCast(getParameter("media"), CommandParameter(Of String)).setValue(media.getFullName)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("media", "The media file", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Return "CINF"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
