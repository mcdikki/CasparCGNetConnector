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

Public Class MixerKeyerCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER KEYER", "Replaces layer n+1's alpha channel with the alpha channel of layer n, and hides the RGB channels of layer n. If keyer equals 1 then the specified layer will not be rendered, instead it will be used as the key for the layer above. ")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1, Optional ByVal keyer As Boolean = False)
        MyBase.New("MIXER KEYER", "Replaces layer n+1's alpha channel with the alpha channel of layer n, and hides the RGB channels of layer n. If keyer equals 1 then the specified layer will not be rendered, instead it will be used as the key for the layer above. ")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setKeyer(keyer)
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1)
        MyBase.New("MIXER KEYER", "Replaces layer n+1's alpha channel with the alpha channel of layer n, and hides the RGB channels of layer n. If keyer equals 1 then the specified layer will not be rendered, instead it will be used as the key for the layer above. ")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New LayerParameter)
        addCommandParameter(New CommandParameter(Of Boolean)("keyer", "Sets whether or not the keyer should be active", False, True))
    End Sub

    Public Overrides Function getCommandString() As String
        checkParameter()
        Dim cmd As String = "MIXER " & getDestination() & " KEYER"
        If getCommandParameter("keyer").isSet Then
            If DirectCast(getCommandParameter("keyer"), CommandParameter(Of Boolean)).getValue() Then
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

    Public Sub setKeyer(ByVal keyer As Boolean)
        DirectCast(getCommandParameter("keyer"), CommandParameter(Of Boolean)).setValue(keyer)
    End Sub

    Public Function getKeyer() As Boolean
        Dim param As CommandParameter(Of Boolean) = getCommandParameter("keyer")
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
