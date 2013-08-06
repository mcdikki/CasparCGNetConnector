Public Class InfoTemplateCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO TEMPLATE", "Requests informations about a template")
        InitParameter()
    End Sub

    Public Sub New(ByVal template As String)
        MyBase.New("INFO TEMPLATE", "Requests informations about a template")
        InitParameter()

        DirectCast(getParameter("template"), CommandParameter(Of String)).setValue(template)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("template", "The template", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "INFO TEMPLATE"
        If getParameter("template").isSet Then
            cmd = cmd & " " & DirectCast(getParameter("template"), CommandParameter(Of String)).getValue
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
