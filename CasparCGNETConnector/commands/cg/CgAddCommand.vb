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
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getCommandParameter("template"), CommandParameter(Of String)).setValue(template.getFullName)
        DirectCast(getCommandParameter("flashlayer"), CommandParameter(Of Integer)).setValue(flashlayer)
        DirectCast(getCommandParameter("play on load"), CommandParameter(Of Boolean)).setValue(playOnLoad)
        If Not IsNothing(data) AndAlso data.Length > 0 Then DirectCast(getCommandParameter("data"), CommandParameter(Of String)).setValue(data)
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal template As String, ByVal flashlayer As Integer, Optional ByVal playOnLoad As Boolean = False, Optional ByVal data As String = "")
        MyBase.New("CG ADD", "Adds a flashtemplate to a given channel / layer on a given flashlayer")
        InitParameter()
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getCommandParameter("template"), CommandParameter(Of String)).setValue(template)
        DirectCast(getCommandParameter("flashlayer"), CommandParameter(Of Integer)).setValue(flashlayer)
        DirectCast(getCommandParameter("play on load"), CommandParameter(Of Boolean)).setValue(playOnLoad)
        If Not IsNothing(data) AndAlso data.Length > 0 Then DirectCast(getCommandParameter("data"), CommandParameter(Of String)).setValue(data)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of String)("template", "The template", "", False))
        addCommandParameter(New CommandParameter(Of Integer)("flashlayer", "The flashlayer", 0, False))
        addCommandParameter(New CommandParameter(Of Boolean)("play on load", "Starts playing the template when loaded", False, False))
        addCommandParameter(New CommandParameter(Of String)("data", "The xml data string", "", True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "CG " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " ADD"

        cmd = cmd & " '" & DirectCast(getCommandParameter("template"), CommandParameter(Of String)).getValue & "'"
        cmd = cmd & " " & DirectCast(getCommandParameter("flashlayer"), CommandParameter(Of Integer)).getValue

        If getCommandParameter("play on load").isSet AndAlso DirectCast(getCommandParameter("play on load"), CommandParameter(Of Boolean)).getValue Then
            cmd = cmd & " 1"
        Else
            cmd = cmd & " 0"
        End If
        If getCommandParameter("data").isSet Then
            cmd = cmd & " '" & DirectCast(getCommandParameter("data"), CommandParameter(Of String)).getValue & "'"
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
