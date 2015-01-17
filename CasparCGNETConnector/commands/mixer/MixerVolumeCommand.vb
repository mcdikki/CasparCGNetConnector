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

Public Class MixerVolumeCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER VOLUME", "Changes the volume of the specified layer")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal volume As Single)
        MyBase.New("MIXER VOLUME", "Changes the volume of the specified layer")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setVolume(volume)
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal volume As Single, ByVal duration As Integer, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        MyBase.New("MIXER VOLUME", "Changes the volume of the specified layer")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setVolume(volume)
        setDuration(duration)
        setTween(tween)
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer)
        MyBase.New("MIXER VOLUME", "Changes the volume of the specified layer")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New LayerParameter)
        addCommandParameter(New CommandParameter(Of Single)("volume", "The volume value of the layer", 0.0, False))
        addCommandParameter(New CommandParameter(Of Integer)("duration", "The the duration of the tween", 0, True))
        addCommandParameter(New CommandParameter(Of CasparCGUtil.Tweens)("tween", "The the tween to use", CasparCGUtil.Tweens.linear, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " VOLUME"

        If getCommandParameter("volume").isSet Then
            cmd = cmd & " " & getVolume.ToString(CultureInfo.GetCultureInfo("en-US"))

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

    Public Sub setVolume(ByVal volume As Single)
        If IsNothing(volume) Then
            DirectCast(getCommandParameter("volume"), CommandParameter(Of Single)).setValue(0.0)
        Else
            DirectCast(getCommandParameter("volume"), CommandParameter(Of Single)).setValue(volume)
        End If
    End Sub

    Public Function getVolume() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("volume")
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
