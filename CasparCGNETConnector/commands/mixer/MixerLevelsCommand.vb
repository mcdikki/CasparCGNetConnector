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
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setMinInput(minInput)
        setMaxInput(maxInput)
        setGamma(gamma)
        setMinOutput(minOutput)
        setMaxOutput(maxOutput)
        setDuration(duration)
        setTween(tween)
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

    Public Sub setChannel(ByVal channel As Integer)
        If channel > 0 Then
            DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        Else
            Throw New ArgumentException("Illegal argument channel=" + channel + ". The parameter channel has to be greater than 0.")
        End If
    End Sub

    Public Function getChannel() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("channel")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setLayer(ByVal layer As Integer)
        If layer < 0 Then
            Throw New ArgumentException("Illegal argument layer=" + layer + ". The parameter layer has to be greater or equal than 0.")
        Else
            DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        End If
    End Sub

    Public Function getLayer() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("layer")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setMinInput(ByVal minInput As Single)
        If IsNothing(minInput) Then
            DirectCast(getCommandParameter("min input"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("min input"), CommandParameter(Of Single)).setValue(minInput)
        End If
    End Sub

    Public Function getMinInput() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("min input")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setMaxInput(ByVal maxInput As Single)
        If IsNothing(maxInput) Then
            DirectCast(getCommandParameter("max input"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("max input"), CommandParameter(Of Single)).setValue(maxInput)
        End If
    End Sub

    Public Function getMaxInput() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("max input")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setMinOutput(ByVal minOutput As Single)
        If IsNothing(minOutput) Then
            DirectCast(getCommandParameter("min output"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("min output"), CommandParameter(Of Single)).setValue(minOutput)
        End If
    End Sub

    Public Function getMinOutput() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("min output")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setMaxOutput(ByVal maxOutput As Single)
        If IsNothing(maxOutput) Then
            DirectCast(getCommandParameter("max output"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("max output"), CommandParameter(Of Single)).setValue(maxOutput)
        End If
    End Sub

    Public Function getMaxOutnput() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("max output")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setGamma(ByVal gamma As Single)
        If IsNothing(gamma) Then
            DirectCast(getCommandParameter("gamma"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("gamma"), CommandParameter(Of Single)).setValue(gamma)
        End If
    End Sub

    Public Function getGamma() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("gamma")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setDuration(ByVal duration As Integer)
        If IsNothing(duration) Then
            DirectCast(getCommandParameter("duration"), CommandParameter(Of Integer)).setValue(0)
        Else
            DirectCast(getCommandParameter("duration"), CommandParameter(Of Integer)).setValue(duration)
        End If
    End Sub

    Public Function getDuratrion() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("duration")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setTween(ByRef tween As CasparCGUtil.Tweens)
        DirectCast(getCommandParameter("tween"), CommandParameter(Of CasparCGUtil.Tweens)).setValue(tween)
    End Sub

    Public Function getTween() As CasparCGUtil.Tweens
        Dim param As CommandParameter(Of CasparCGUtil.Tweens) = getCommandParameter("tween")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
