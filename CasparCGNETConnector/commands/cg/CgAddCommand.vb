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

Public Class CgAddCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("CG ADD", "Adds a flashtemplate to a given channel / layer on a given flashlayer")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal template As CasparCGTemplate, ByVal flashlayer As Integer, Optional ByVal playOnLoad As Boolean = False, Optional ByVal data As String = "")
        MyBase.New("CG ADD", "Adds a flashtemplate to a given channel / layer on a given flashlayer")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getParameter("template"), CommandParameter(Of String)).setValue(template.getFullName)
        DirectCast(getParameter("flashlayer"), CommandParameter(Of Integer)).setValue(flashlayer)
        DirectCast(getParameter("play on load"), CommandParameter(Of Boolean)).setValue(playOnLoad)
        If Not IsNothing(data) AndAlso data.Length > 0 Then DirectCast(getParameter("data"), CommandParameter(Of String)).setValue(data)
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal template As String, ByVal flashlayer As Integer, Optional ByVal playOnLoad As Boolean = False, Optional ByVal data As String = "")
        MyBase.New("CG ADD", "Adds a flashtemplate to a given channel / layer on a given flashlayer")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getParameter("template"), CommandParameter(Of String)).setValue(template)
        DirectCast(getParameter("flashlayer"), CommandParameter(Of Integer)).setValue(flashlayer)
        DirectCast(getParameter("play on load"), CommandParameter(Of Boolean)).setValue(playOnLoad)
        If Not IsNothing(data) AndAlso data.Length > 0 Then DirectCast(getParameter("data"), CommandParameter(Of String)).setValue(data)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of String)("template", "The template", "", False))
        addParameter(New CommandParameter(Of Integer)("flashlayer", "The flashlayer", 0, False))
        addParameter(New CommandParameter(Of Boolean)("play on load", "Starts playing the template when loaded", False, False))
        addParameter(New CommandParameter(Of String)("data", "The xml data string", "", True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "CG " & getDestination(getParameter("channel"), getParameter("layer")) & " ADD"

        cmd = cmd & " '" & DirectCast(getParameter("template"), CommandParameter(Of String)).getValue & "'"
        cmd = cmd & " " & DirectCast(getParameter("flashlayer"), CommandParameter(Of Integer)).getValue

        If getParameter("play on load").isSet AndAlso DirectCast(getParameter("play on load"), CommandParameter(Of Boolean)).getValue Then
            cmd = cmd & " 1"
        Else
            cmd = cmd & " 0"
        End If
        If getParameter("data").isSet Then
            cmd = cmd & " '" & DirectCast(getParameter("data"), CommandParameter(Of String)).getValue & "'"
        End If

        Return escape(cmd)
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
