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

Public Class MixerChromaCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER CHROMA", "Enables chroma keying on the specified video layer")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1, Optional ByVal color As String = "", Optional ByVal threshold As Single = 0.0, Optional ByVal softness As Single = 0.0)
        MyBase.New("MIXER CHROMA", "Enables chroma keying on the specified video layer")
        InitParameter()
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        If Not IsNothing(color) AndAlso color.Length > 0 Then DirectCast(getCommandParameter("color"), CommandParameter(Of String)).setValue(color)
        DirectCast(getCommandParameter("threshold"), CommandParameter(Of Single)).setValue(threshold)
        DirectCast(getCommandParameter("softness"), CommandParameter(Of Single)).setValue(softness)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of String)("color", "The color to key with. Only blue, green or none allowed", "none", False))
        addCommandParameter(New CommandParameter(Of Single)("threshold", "The threshold", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("softness", "The softness", 0, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " CHROMA"

        If getCommandParameter("color").isSet AndAlso (DirectCast(getCommandParameter("color"), CommandParameter(Of String)).getValue = "blue" OrElse DirectCast(getCommandParameter("color"), CommandParameter(Of String)).getValue = "green") Then
            cmd = cmd & " " & DirectCast(getCommandParameter("color"), CommandParameter(Of String)).getValue & " " _
                & DirectCast(getCommandParameter("threshold"), CommandParameter(Of Single)).getValue _
                & DirectCast(getCommandParameter("softness"), CommandParameter(Of Single)).getValue
        Else
            cmd = cmd & " none"
        End If

        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
