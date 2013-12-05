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

Public Class SwapCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("SWAP", "Swapes the given channels or layers")
        InitParameter()
    End Sub

    Public Sub New(ByVal channelA As Integer, ByVal channelB As Integer, Optional ByVal layerA As Integer = -1, Optional ByVal layerB As Integer = -1)
        MyBase.New("SWAP", "Swapss the given channels or layers")
        InitParameter()
        Init(channelA, channelB, layerA, layerB)
    End Sub


    Private Sub Init(ByVal channelA As Integer, ByVal channelB As Integer, ByVal layerA As Integer, ByVal layerB As Integer)
        If channelA > 0 Then DirectCast(getCommandParameter("channelA"), CommandParameter(Of Integer)).setValue(channelA)
        If layerA > -1 Then DirectCast(getCommandParameter("layerA"), CommandParameter(Of Integer)).setValue(layerA)
        If channelA > 0 Then DirectCast(getCommandParameter("channelB"), CommandParameter(Of Integer)).setValue(channelB)
        If layerA > -1 Then DirectCast(getCommandParameter("layerB"), CommandParameter(Of Integer)).setValue(layerB)
    End Sub

    Private Sub InitParameter()
        '' Add all paramters here:
        addCommandParameter(New CommandParameter(Of Integer)("channelA", "The first channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("channelB", "The second channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layerA", "The first layer", 0, True))
        addCommandParameter(New CommandParameter(Of Integer)("layerB", "The second layer", 0, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "SWAP "
        If getCommandParameter("layerA").isSet AndAlso getCommandParameter("layerB").isSet Then
            cmd = cmd & getDestination(getCommandParameter("channelA"), getCommandParameter("layerA")) & " " & getDestination(getCommandParameter("channelB"), getCommandParameter("layerB"))
        Else
            cmd = cmd & DirectCast(getCommandParameter("channelA"), CommandParameter(Of Integer)).getValue & " " & DirectCast(getCommandParameter("channelB"), CommandParameter(Of Integer)).getValue
        End If

        Return escape(cmd)
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
