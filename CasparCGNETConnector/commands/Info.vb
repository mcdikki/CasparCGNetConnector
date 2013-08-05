Public Class Info
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO", "Requests informations about the server, channels, layers or templates")
        InitParameter()
    End Sub

    'Public Shared Function getInfo(Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal onlyBackground As Boolean = False, Optional ByVal onlyForeground As Boolean = False) As String
    '    Dim cmd As String = "INFO"
    '    If channel > -1 Then
    '        cmd = cmd & " " & getDestination(channel, layer)
    '        If layer > -1 Then
    '            If onlyBackground Then
    '                cmd = cmd & " B"
    '            ElseIf onlyForeground Then
    '                cmd = cmd & " F"
    '            End If
    '        End If
    '    End If
    '    Return cmd
    'End Function

    'Public Shared Function getInfo(ByRef template As CasparCGTemplate) As String
    '    Return escape("INFO TEMPLATE '" & template.getFullName & "'")
    'End Function

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, True))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of Boolean)("only background", "only show background", False, True))
        addParameter(New CommandParameter(Of Boolean)("only foreground", "only show foreground", False, True))
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
