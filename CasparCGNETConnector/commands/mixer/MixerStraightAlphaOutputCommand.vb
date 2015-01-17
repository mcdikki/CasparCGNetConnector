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

Public Class MixerStraightAlphaOutputCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER STRAIGHT_ALPHA_OUTPUT", "If enabled, causes RGB values to be divided with the alpha for the given video channel before the image is sent to consumers. ")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1, Optional ByVal active As Boolean = False)
        MyBase.New("MIXER STRAIGHT_ALPHA_OUTPUT", "If enabled, causes RGB values to be divided with the alpha for the given video channel before the image is sent to consumers. ")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setActive(active)
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1)
        MyBase.New("MIXER STRAIGHT_ALPHA_OUTPUT", "If enabled, causes RGB values to be divided with the alpha for the given video channel before the image is sent to consumers. ")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New LayerParameter)
        addCommandParameter(New CommandParameter(Of Boolean)("active", "Sets whether or not straight alpha output should be active", False, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " STRAIGHT_ALPHA_OUTPUT"
        If getCommandParameter("active").isSet Then
            If getActive() Then
                cmd = cmd & " 1"
            Else
                cmd = cmd & " 0"
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

    Public Sub setActive(ByVal active As Boolean)
        DirectCast(getCommandParameter("active"), CommandParameter(Of Boolean)).setValue(active)
    End Sub

    Public Function getActive() As Boolean
        Dim param As CommandParameter(Of Boolean) = getCommandParameter("active")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
