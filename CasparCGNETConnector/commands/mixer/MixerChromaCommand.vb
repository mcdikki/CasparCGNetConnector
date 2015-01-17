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

Public Class MixerChromaCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER CHROMA", "Enables chroma keying on the specified video layer")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1, Optional ByVal color As String = "", Optional ByVal threshold As Single = 0.0, Optional ByVal softness As Single = 0.0)
        MyBase.New("MIXER CHROMA", "Enables chroma keying on the specified video layer")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setColor(color)
        setThreshold(threshold)
        setSoftness(softness)
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1)
        MyBase.New("MIXER CHROMA", "Enables chroma keying on the specified video layer")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
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

        If getCommandParameter("color").isSet Then
            If (getColor().ToLower.Equals("blue") OrElse getColor().ToLower.Equals("green")) Then
                cmd = cmd & " " & getColor() & " " & getThreshold().ToString(CultureInfo.GetCultureInfo("en-US")) & " " & getSoftness().ToString(CultureInfo.GetCultureInfo("en-US"))
            ElseIf getColor.ToLower.Equals("none") Then
                cmd = cmd & " none"
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

    Public Sub setColor(ByVal color As String)
        If IsNothing(color) Then
            DirectCast(getCommandParameter("color"), CommandParameter(Of String)).setValue("none")
        ElseIf color.ToLower.Equals("none") OrElse color.ToLower.Equals("green") OrElse color.ToLower.Equals("blue") Then
            DirectCast(getCommandParameter("color"), CommandParameter(Of String)).setValue(color)
        Else
            Throw New ArgumentException("Illegal argument color=" & color & ". Color must be one of: none, green, blue")
        End If
    End Sub

    Public Function getColor() As String
        Dim param As CommandParameter(Of String) = getCommandParameter("color")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setSoftness(ByVal softness As Single)
        If IsNothing(softness) Then
            DirectCast(getCommandParameter("softness"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("softness"), CommandParameter(Of Single)).setValue(softness)
        End If
    End Sub

    Public Function getSoftness() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("softness")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setThreshold(ByVal threshold As Single)
        If IsNothing(threshold) Then
            DirectCast(getCommandParameter("threshold"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("threshold"), CommandParameter(Of Single)).setValue(threshold)
        End If
    End Sub

    Public Function getThreshold() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("threshold")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
