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
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getCommandParameter("keyer"), CommandParameter(Of Boolean)).setValue(keyer)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of Boolean)("keyer", "Sets whether or not the keyer should be active", False, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " KEYER"
        If getCommandParameter("keyer").isSet AndAlso DirectCast(getCommandParameter("keyer"), CommandParameter(Of Boolean)).getValue() Then
            cmd = cmd & " 1"
        Else
            cmd = cmd & " 0"
        End If

        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
