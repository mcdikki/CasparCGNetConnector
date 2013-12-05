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
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getCommandParameter("min input"), CommandParameter(Of Single)).setValue(minInput)
        DirectCast(getCommandParameter("max input"), CommandParameter(Of Single)).setValue(maxInput)
        DirectCast(getCommandParameter("gamma"), CommandParameter(Of Single)).setValue(gamma)
        DirectCast(getCommandParameter("minOutput"), CommandParameter(Of Single)).setValue(minOutput)
        DirectCast(getCommandParameter("maxOutput"), CommandParameter(Of Single)).setValue(maxOutput)
        DirectCast(getCommandParameter("duration"), CommandParameter(Of Integer)).setValue(duration)
        DirectCast(getCommandParameter("tween"), CommandParameter(Of CasparCGUtil.Tweens)).setValue(tween)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of Single)("min input", "Documentation missing. Sorry :-(", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("max input", "Documentation missing. Sorry :-(", 1, False))
        addCommandParameter(New CommandParameter(Of Single)("gamma", "Documentation missing. Sorry :-(", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("min output", "Documentation missing. Sorry :-(", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("man output", "Documentation missing. Sorry :-(", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("duration", "The the duration of the tween", 0, True))
        addCommandParameter(New CommandParameter(Of CasparCGUtil.Tweens)("tween", "The the tween to use", CasparCGUtil.Tweens.linear, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " LEVELS"

        cmd = cmd & " " & DirectCast(getCommandParameter("min input"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("max input"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("gamma"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("min output"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("max output"), CommandParameter(Of Single)).getValue

        If getCommandParameter("duration").isSet AndAlso getCommandParameter("tween").isSet Then
            cmd = cmd & " " & DirectCast(getCommandParameter("duration"), CommandParameter(Of Integer)).getValue & " " & CasparCGUtil.Tweens.GetName(GetType(CasparCGUtil.Tweens), DirectCast(getCommandParameter("tween"), CommandParameter(Of CasparCGUtil.Tweens)).getValue)
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
