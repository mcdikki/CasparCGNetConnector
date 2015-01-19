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

Public Class SetCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("SET", "Sets the video mode of the given channel")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal videomode As String)
        MyBase.New("SET", "Sets the video mode of the given channel")
        InitParameter()
        setChannel(channel)
        setVidemode(videomode)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New CommandParameter(Of String)("video mode", "The video mode", "PAL", False))
    End Sub

    Public Overrides Function getCommandString() As String
        checkParameter()
        Return "SET " & getDestination() & " MODE " & getVideomode()
    End Function

    Public Sub setChannel(ByVal channel As Integer)
        If channel > 0 Then
            DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        Else
            Throw New ArgumentException("Illegal argument channel=" + channel + ". The parameter channel has to be greater than 0.")
        End If
    End Sub

    Public Sub setVidemode(ByVal videomode As String)
        If Not IsNothing(videomode) Then
            DirectCast(getCommandParameter("video mode"), CommandParameter(Of String)).setValue(videomode)
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

    Public Function getVideomode() As String
        Dim param As CommandParameter(Of String) = getCommandParameter("video mode")
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
