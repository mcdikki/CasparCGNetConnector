Public Class SetCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("SET", "Sets the video mode of the given channel")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal videomode As String)
        MyBase.New("SET", "Sets the video mode of the given channel")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        DirectCast(getParameter("video mode"), CommandParameter(Of String)).setValue(videomode)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of String)("video mode", "The video mode", "PAL", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "SET " & getDestination(getParameter("channel"))
        If getParameter("video mode").isSet Then
            DirectCast(getParameter("parameter"), CommandParameter(Of String())).getValue()
            cmd = cmd & " " & DirectCast(getParameter("video mode"), CommandParameter(Of String)).getValue()
        End If
        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
