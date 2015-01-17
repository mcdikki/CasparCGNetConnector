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

Public Class MixerGridCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER GRID", "Creates a grid of video streams in ascending order of the layer index, i.e. if resolution equals 2 then a 2x2 grid of layers will be created.")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal resolution As Integer)
        MyBase.New("MIXER GRID", "Creates a grid of video streams in ascending order of the layer index, i.e. if resolution equals 2 then a 2x2 grid of layers will be created.")
        InitParameter()
        setChannel(channel)
        setResolution(resolution)
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal resolution As Integer, ByVal duration As Integer, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        MyBase.New("MIXER GRID", "Creates a grid of video streams in ascending order of the layer index, i.e. if resolution equals 2 then a 2x2 grid of layers will be created.")
        InitParameter()
        setChannel(channel)
        setResolution(resolution)
        setDuration(duration)
        setTween(tween)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New CommandParameter(Of Integer)("resolution", "The resolution of the grid. i.e. if resolution equals 2 then a 2x2 grid of layers will be created.", 2, False))
        addCommandParameter(New CommandParameter(Of Integer)("duration", "The the duration of the tween", 0, True))
        addCommandParameter(New CommandParameter(Of CasparCGUtil.Tweens)("tween", "The the tween to use", CasparCGUtil.Tweens.linear, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel")) & " GRID " & getResolution()

        If getCommandParameter("duration").isSet AndAlso getCommandParameter("tween").isSet Then
            cmd = cmd & " " & getDuratrion() & " " & getTween.ToString
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

    Public Sub setResolution(ByVal duration As Integer)
        If IsNothing(duration) Then
            DirectCast(getCommandParameter("resolution"), CommandParameter(Of Integer)).setValue(0)
        Else
            DirectCast(getCommandParameter("resolution"), CommandParameter(Of Integer)).setValue(duration)
        End If
    End Sub

    Public Function getResolution() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("resolution")
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
