Imports System.Globalization

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

Public Class MixerCropCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER CROP", "Returns or modifies the edges for the cropping for a layer.")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal left As Single, ByVal top As Single, ByVal right As Single, ByVal bottom As Single)
        MyBase.New("MIXER CROP", "Returns or modifies the edges for the cropping for a layer.")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setLeft(left)
        setTop(top)
        setRight(right)
        setBottom(bottom)
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal left As Single, ByVal top As Single, ByVal right As Single, ByVal bottom As Single, ByVal duration As Integer, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        MyBase.New("MIXER CROP", "Returns or modifies the edges for the cropping for a layer.")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setLeft(left)
        setTop(top)
        setRight(right)
        setBottom(bottom)
        setDuration(duration)
        setTween(tween)
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer)
        MyBase.New("MIXER CROP", "Returns or modifies the edges for the cropping for a layer.")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New LayerParameter)
        addCommandParameter(New CommandParameter(Of Single)("left", "The left edges pos.", 0, True))
        addCommandParameter(New CommandParameter(Of Single)("top", "The top edges pos.", 0, True))
        addCommandParameter(New CommandParameter(Of Single)("right", "The right edges pos.", 1, True))
        addCommandParameter(New CommandParameter(Of Single)("bottom", "The bottom edges pos.", 0, True))
        addCommandParameter(New CommandParameter(Of Integer)("duration", "The the duration of the tween", 0, True))
        addCommandParameter(New CommandParameter(Of CasparCGUtil.Tweens)("tween", "The the tween to use", CasparCGUtil.Tweens.linear, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " CROP"

        If getCommandParameter("left").isSet OrElse getCommandParameter("top").isSet OrElse getCommandParameter("right").isSet OrElse getCommandParameter("bottom").isSet Then
            cmd = cmd & " " & getLeft().ToString(CultureInfo.GetCultureInfo("en-US"))
            cmd = cmd & " " & getTop().ToString(CultureInfo.GetCultureInfo("en-US"))
            cmd = cmd & " " & getRight().ToString(CultureInfo.GetCultureInfo("en-US"))
            cmd = cmd & " " & getBottom().ToString(CultureInfo.GetCultureInfo("en-US"))

            If getCommandParameter("duration").isSet AndAlso getCommandParameter("tween").isSet Then
                cmd = cmd & " " & DirectCast(getCommandParameter("duration"), CommandParameter(Of Integer)).getValue & " " & CasparCGUtil.Tweens.GetName(GetType(CasparCGUtil.Tweens), DirectCast(getCommandParameter("tween"), CommandParameter(Of CasparCGUtil.Tweens)).getValue)
            End If
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

    Public Sub setLeft(ByVal left As Single)
        If IsNothing(left) Then
            DirectCast(getCommandParameter("left"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("left"), CommandParameter(Of Single)).setValue(left)
        End If
    End Sub

    Public Function getLeft() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("left")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setTop(ByVal top As Single)
        If IsNothing(top) Then
            DirectCast(getCommandParameter("top"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("top"), CommandParameter(Of Single)).setValue(top)
        End If
    End Sub

    Public Function getTop() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("top")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setRight(ByVal right As Single)
        If IsNothing(right) Then
            DirectCast(getCommandParameter("right"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("right"), CommandParameter(Of Single)).setValue(right)
        End If
    End Sub

    Public Function getRight() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("right")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function


    Public Sub setBottom(ByVal bottom As Single)
        If IsNothing(bottom) Then
            DirectCast(getCommandParameter("bottom"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("bottom"), CommandParameter(Of Single)).setValue(bottom)
        End If
    End Sub

    Public Function getBottom() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("bottom")
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
        Return {2, 0, 7}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
