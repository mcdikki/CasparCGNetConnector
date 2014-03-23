'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'' Author: Christopher Diekkamp
'' Email: christopher@development.diekkamp.de
'' GitHub: https://github.com/mcdikki
'' 
'' This software is licensed under the 
'' GNU General Public License Version 3 (GPLv3).
'' See http://www.gnu.org/licenses/gpl-3.0-standalone.html 
'' for a copy of the license.
''
'' You are free to copy, use and modify this software.
'' Please let me know of any changes and improvements you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public Class InfoTemplateCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO TEMPLATE", "Requests informations about a template")
        InitParameter()
    End Sub

    Public Sub New(ByVal template As String)
        MyBase.New("INFO TEMPLATE", "Requests informations about a template")
        InitParameter()
        setTemplate(template)
    End Sub

    Public Sub New(ByVal template As CasparCGTemplate)
        MyBase.New("INFO TEMPLATE", "Requests informations about a template")
        InitParameter()
        setTemplate(template)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of String)("template", "The template", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "INFO TEMPLATE"
        If getCommandParameter("template").isSet Then
            cmd = cmd & " '" & DirectCast(getCommandParameter("template"), CommandParameter(Of String)).getValue & "'"
        End If
        Return escape(cmd)
    End Function

    Public Sub setTemplate(ByVal template As String)
        If IsNothing(template) Then
            DirectCast(getCommandParameter("template"), CommandParameter(Of String)).setValue("")
        Else
            DirectCast(getCommandParameter("template"), CommandParameter(Of String)).setValue(template)
        End If
    End Sub

    Public Sub setTemplate(ByVal template As CasparCGTemplate)
        If IsNothing(template) Then
            DirectCast(getCommandParameter("template"), CommandParameter(Of String)).setValue("")
        Else
            DirectCast(getCommandParameter("template"), CommandParameter(Of String)).setValue(template.getFullName)
        End If
    End Sub

    Public Function getTemplate() As String
        Dim param As CommandParameter(Of String) = getCommandParameter("template")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
