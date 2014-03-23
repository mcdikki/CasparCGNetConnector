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
        setChannel(channel)
        setVolume(volume)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Single)("volume", "The volume to set the channel to between", 1, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel")) & " MASTERVOLUME " & DirectCast(getCommandParameter("volume"), CommandParameter(Of Single)).getValue()

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

    Public Sub setVolume(ByVal volume As Single)
        If IsNothing(volume) Then
            DirectCast(getCommandParameter("volume"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("volume"), CommandParameter(Of Single)).setValue(volume)
        End If
    End Sub

    Public Function getVolume() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("volume")
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
