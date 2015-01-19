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

Public Class MixerBlendCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER BLEND", "Every layer in the Mixer module can be set to a blend mode over than the default Normal mode, similar to applications like Photoshop. Some common uses are to use screen to make a all the black image data become transparent, or to use add to selectively lighten highlights.")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, Optional ByVal blendmode As String = "")
        MyBase.New("MIXER BLEND", "Every layer in the Mixer module can be set to a blend mode over than the default Normal mode, similar to applications like Photoshop. Some common uses are to use screen to make a all the black image data become transparent, or to use add to selectively lighten highlights.")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        If blendmode.Length > 0 Then setBlendmode(blendmode)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New LayerParameter)
        addCommandParameter(New CommandParameter(Of String)("blendmode", "The blend mode to use with the mixer like, OVERLAY, ADD, SCREEN etc.", "normal", True))
    End Sub

    Public Overrides Function getCommandString() As String
        checkParameter()
        Dim cmd As String = "MIXER " & getDestination() & " BLEND"

        If getCommandParameter("blendmode").isSet Then cmd = cmd & " " & DirectCast(getCommandParameter("blendmode"), CommandParameter(Of String)).getValue()

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

    Public Sub setBlendmode(ByVal blendmode As String)
        If IsNothing(blendmode) Then
            DirectCast(getCommandParameter("blendmode"), CommandParameter(Of String)).setValue("")
        Else
            DirectCast(getCommandParameter("blendmode"), CommandParameter(Of String)).setValue(blendmode)
        End If
    End Sub

    Public Function getBlendmode() As String
        Dim param As CommandParameter(Of String) = getCommandParameter("blendmode")
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
