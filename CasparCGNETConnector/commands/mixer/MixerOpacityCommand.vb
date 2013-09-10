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

Public Class MixerOpacityCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER VIDEO OPACITY", "Changes the opacity of the specified layer. The value is a float between 0 and 1")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal opacity As Single, Optional ByVal duration As Integer = 0, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        MyBase.New("MIXER VIDEO OPACITY", "Changes the opacity of the specified layer. The value is a float between 0 and 1")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getParameter("opacity"), CommandParameter(Of Single)).setValue(opacity)
        DirectCast(getParameter("duration"), CommandParameter(Of Integer)).setValue(duration)
        DirectCast(getParameter("tween"), CommandParameter(Of CasparCGUtil.Tweens)).setValue(tween)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of Single)("opacity", "The opacity of the layer", 0.0, False))
        addParameter(New CommandParameter(Of Integer)("duration", "The the duration of the tween", 0, True))
        addParameter(New CommandParameter(Of CasparCGUtil.Tweens)("tween", "The the tween to use", CasparCGUtil.Tweens.linear, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getParameter("channel"), getParameter("layer")) & " VIDEO OPACITY"

        cmd = cmd & " " & DirectCast(getParameter("opacity"), CommandParameter(Of Single)).getValue

        If getParameter("duration").isSet AndAlso getParameter("tween").isSet Then
            cmd = cmd & " " & DirectCast(getParameter("duration"), CommandParameter(Of Integer)).getValue & " " & CasparCGUtil.Tweens.GetName(GetType(CasparCGUtil.Tweens), DirectCast(getParameter("tween"), CommandParameter(Of CasparCGUtil.Tweens)).getValue)
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
