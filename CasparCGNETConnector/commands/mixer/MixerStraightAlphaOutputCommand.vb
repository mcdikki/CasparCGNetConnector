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
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getParameter("active"), CommandParameter(Of Boolean)).setValue(active)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of Boolean)("active", "Sets whether or not straight alpha output should be active", False, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getParameter("channel"), getParameter("layer")) & " STRAIGHT_ALPHA_OUTPUT"
        If getParameter("active").isSet AndAlso DirectCast(getParameter("active"), CommandParameter(Of Boolean)).getValue() Then
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
