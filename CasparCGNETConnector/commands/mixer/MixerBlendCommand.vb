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

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal blendmode As String)
        MyBase.New("MIXER BLEND", "Every layer in the Mixer module can be set to a blend mode over than the default Normal mode, similar to applications like Photoshop. Some common uses are to use screen to make a all the black image data become transparent, or to use add to selectively lighten highlights.")
        InitParameter()
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getCommandParameter("blend mode"), CommandParameter(Of String)).setValue(blendmode)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of String)("blend mode", "The blend mode to use with the mixer like, OVERLAY, ADD, SCREEN etc.", False, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " BLEND"

        cmd = cmd & " " & DirectCast(getCommandParameter("blend mode"), CommandParameter(Of String)).getValue()

        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
