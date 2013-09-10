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

Public Class MixerMastervolumeCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER MASTERVOLUME", "Changes the volume of an entire channel. ")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal volume As Single)
        MyBase.New("MIXER MASTERVOLUME", "Changes the volume of an entire channel. ")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        DirectCast(getParameter("volume"), CommandParameter(Of Single)).setValue(volume)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of Single)("volume", "The volume to set the channel to between", 1, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getParameter("channel")) & " MASTERVOLUME " & DirectCast(getParameter("volume"), CommandParameter(Of Single)).getValue()

        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
