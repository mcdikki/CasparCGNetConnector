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

Public Class MixerLevelsCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER LEVELS", "Documentation missing. Sorry :-(")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal minInput As Single, ByVal maxInput As Single, ByVal gamma As Single, ByVal minOutput As Single, ByVal maxOutput As Single, Optional ByVal duration As Integer = 0, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        MyBase.New("MIXER LEVELS", "Documentation missing. Sorry :-(")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getParameter("min input"), CommandParameter(Of Single)).setValue(minInput)
        DirectCast(getParameter("max input"), CommandParameter(Of Single)).setValue(maxInput)
        DirectCast(getParameter("gamma"), CommandParameter(Of Single)).setValue(gamma)
        DirectCast(getParameter("minOutput"), CommandParameter(Of Single)).setValue(minOutput)
        DirectCast(getParameter("maxOutput"), CommandParameter(Of Single)).setValue(maxOutput)
        DirectCast(getParameter("duration"), CommandParameter(Of Integer)).setValue(duration)
        DirectCast(getParameter("tween"), CommandParameter(Of CasparCGUtil.Tweens)).setValue(tween)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of Single)("min input", "Documentation missing. Sorry :-(", 0, False))
        addParameter(New CommandParameter(Of Single)("max input", "Documentation missing. Sorry :-(", 1, False))
        addParameter(New CommandParameter(Of Single)("gamma", "Documentation missing. Sorry :-(", 0, False))
        addParameter(New CommandParameter(Of Single)("min output", "Documentation missing. Sorry :-(", 0, False))
        addParameter(New CommandParameter(Of Single)("man output", "Documentation missing. Sorry :-(", 1, False))
        addParameter(New CommandParameter(Of Integer)("duration", "The the duration of the tween", 0, True))
        addParameter(New CommandParameter(Of CasparCGUtil.Tweens)("tween", "The the tween to use", CasparCGUtil.Tweens.linear, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getParameter("channel"), getParameter("layer")) & " LEVELS"

        cmd = cmd & " " & DirectCast(getParameter("min input"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getParameter("max input"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getParameter("gamma"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getParameter("min output"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getParameter("max output"), CommandParameter(Of Single)).getValue

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
