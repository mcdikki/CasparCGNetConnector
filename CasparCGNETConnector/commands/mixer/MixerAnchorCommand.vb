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

Public Class MixerAnchorCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER ANCHOR", "Changes the anchor point around which fill_translation, fill_scale and ROTATION will be done from. x The left anchor point, 0 = left edge of monitor, 0.5 = middle of monitor, 1.0 = right edge of monitor. Higher and lower values allowed. y The top anchor point, 0 = top edge of monitor, 0.5 = middle of monitor, 1.0 = bottom edge of monitor. Higher and lower values allowed.")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal x As Single, ByVal y As Single)
        MyBase.New("MIXER ANCHOR", "Changes the anchor point around which fill_translation, fill_scale and ROTATION will be done from. x The left anchor point, 0 = left edge of monitor, 0.5 = middle of monitor, 1.0 = right edge of monitor. Higher and lower values allowed. y The top anchor point, 0 = top edge of monitor, 0.5 = middle of monitor, 1.0 = bottom edge of monitor. Higher and lower values allowed.")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setX(x)
        setY(y)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of Single)("x", "x The left anchor point, 0 = left edge of monitor, 0.5 = middle of monitor, 1.0 = right edge of monitor. Higher and lower values allowed.", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("y", "y The top anchor point, 0 = top edge of monitor, 0.5 = middle of monitor, 1.0 = bottom edge of monitor. Higher and lower values allowed.", 0, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " ANCHOR"

        cmd = cmd & " " & DirectCast(getCommandParameter("x"), CommandParameter(Of Single)).getValue
        cmd = cmd & " " & DirectCast(getCommandParameter("y"), CommandParameter(Of Single)).getValue

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

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 7}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
