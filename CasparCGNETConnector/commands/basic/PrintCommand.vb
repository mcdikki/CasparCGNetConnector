Public Class PrintCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("PRINT", "Saves a screenshot of a given channel")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer)
        MyBase.New("PRINT", "Saves a screenshot of a given channel")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        'DirectCast(getParameter("file"), CommandParameter(Of String)).setValue(file)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("channel", "The channel", 1, False))
        'addParameter(New CommandParameter(Of String)("file", "The filename", "", True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "PRINT " & getDestination(getParameter("channel"))
        'If getParameter("file").isSet Then
        '    cmd = cmd & " " & DirectCast(getParameter("parameter"), CommandParameter(Of String)).getValue
        'End If
        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
