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

Public Class MixerClipCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER CLIP", "Masks the video stream on the specified layer. The concept is quite simple; it comes from the ancient DVE machines like ADO. Imagine that the screen has a size of 1x1 (not in pixel, but in an abstract measure). Then the coordinates of a full size picture is 0 0 1 1, which means left edge is at coordinate 0, top edge at coordinate 0, width full size => 1, heigh full size => 1. If you want to crop the picture on the left side (for wipe left to right) You set the left edge to full right => 1 and the width to 0. So this give you the start-coordinates of 1 0 0 1. End coordinates of any wipe are allways the full picture 0 0 1 1.")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal x As Single, ByVal y As Single, ByVal xscale As Single, ByVal ysacle As Single, Optional ByVal duration As Integer = 0, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        MyBase.New("MIXER CLIP", "Masks the video stream on the specified layer. The concept is quite simple; it comes from the ancient DVE machines like ADO. Imagine that the screen has a size of 1x1 (not in pixel, but in an abstract measure). Then the coordinates of a full size picture is 0 0 1 1, which means left edge is at coordinate 0, top edge at coordinate 0, width full size => 1, heigh full size => 1. If you want to crop the picture on the left side (for wipe left to right) You set the left edge to full right => 1 and the width to 0. So this give you the start-coordinates of 1 0 0 1. End coordinates of any wipe are allways the full picture 0 0 1 1.")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setDuration(duration)
        setTween(tween)
        setX(x)
        setY(y)
        setXscale(xscale)
        setYscale(ysacle)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of Single)("x", " The left edge of the new clipSize, 0 = left edge of monitor, 0.5 = middle of monitor, 1.0 = right edge of monitor. Higher and lower values allowed. ", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("y", "The top edge of the new clipSize, 0 = top edge of monitor, 0.5 = middle of monitor, 1.0 = bottom edge of monitor. Higher and lower values allowed.", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("xscale", "The size of the new clipSize, 1 = 1x the screen size, 0.5 = half the screen size. Higher and lower values allowed. ", 1, False))
        addCommandParameter(New CommandParameter(Of Single)("yscale", "The size of the new clipSize, 1 = 1x the screen size, 0.5 = half the screen size. Higher and lower values allowed. ", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("duration", "The the duration of the tween", 0, True))
        addCommandParameter(New CommandParameter(Of CasparCGUtil.Tweens)("tween", "The the tween to use", CasparCGUtil.Tweens.linear, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " CLIP"

        cmd = cmd & " " & DirectCast(getCommandParameter("x"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("y"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("xscale"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("yscale"), CommandParameter(Of Single)).getValue

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

    Public Sub setX(ByVal x As Single)
        If IsNothing(x) Then
            DirectCast(getCommandParameter("x"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("x"), CommandParameter(Of Single)).setValue(x)
        End If
    End Sub

    Public Function getX() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("x")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setY(ByVal y As Single)
        If IsNothing(y) Then
            DirectCast(getCommandParameter("y"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("y"), CommandParameter(Of Single)).setValue(y)
        End If
    End Sub

    Public Function getY() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("y")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setXscale(ByVal xscale As Single)
        If IsNothing(xscale) Then
            DirectCast(getCommandParameter("xscale"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("xscale"), CommandParameter(Of Single)).setValue(xscale)
        End If
    End Sub

    Public Function getXscale() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("xscale")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setYscale(ByVal yscale As Single)
        If IsNothing(yscale) Then
            DirectCast(getCommandParameter("yscale"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("yscale"), CommandParameter(Of Single)).setValue(yscale)
        End If
    End Sub

    Public Function getYscale() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("yscale")
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
