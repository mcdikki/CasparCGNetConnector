Public Class Info
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO", "Requests informations about the server, channels, layers or templates")
        InitParameter()
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, True))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of Boolean)("only background", "Only show info of background", False, True))
        addParameter(New CommandParameter(Of Boolean)("only foreground", "Only show info of foreground", False, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "INFO"
        If getParameter("Channel").isSet Then
            cmd = cmd & " " & getDestination(getParameter("Channel"), getParameter("layer"))
            If getParameter("layer").isSet Then
                If getParameter("onlyBackground").isSet AndAlso DirectCast(getParameter("onlyBackground"), CommandParameter(Of Boolean)).getValue Then
                    cmd = cmd & " B"
                ElseIf getParameter("onlyForeground").isSet AndAlso DirectCast(getParameter("onlyForeround"), CommandParameter(Of Boolean)).getValue Then
                    cmd = cmd & " F"
                End If
            End If
        End If
        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
