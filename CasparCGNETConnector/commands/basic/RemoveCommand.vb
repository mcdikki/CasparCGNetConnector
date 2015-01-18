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

Public Class RemoveCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("REMOVE", "Removes a consumer from a given channel")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal consumer As String, Optional ByVal parameter() As String = Nothing)
        MyBase.New("REMOVE", "Removes a comsumer from a given channel")
        InitParameter()
        setChannel(channel)
        setConsumer(consumer)
        If Not IsNothing(parameter) Then setParamter(parameter)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New CommandParameter(Of String)("consumer", "The consumer to add to the channel i.e. SCREEN or FILE.", "", False))
        addCommandParameter(New CommandParameter(Of String())("parameter", "The paramter list", {}, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "REMOVE " & getDestination(getCommandParameter("channel")) & " " & getConsumer()
        If getCommandParameter("parameter").isSet AndAlso Not IsNothing(getParameter()) AndAlso getParameter().Length > 0 Then
            For Each p In getParameter()
                cmd = cmd & " " & p
            Next
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

    Public Sub setConsumer(ByVal consumer As String)
        If Not IsNothing(consumer) Then
            DirectCast(getCommandParameter("consumer"), CommandParameter(Of String)).setValue(consumer)
        End If
    End Sub

    Public Function getConsumer() As String
        Dim param As CommandParameter(Of String) = getCommandParameter("consumer")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setParamter(ByVal parameter As String())
        If Not IsNothing(parameter) Then
            DirectCast(getCommandParameter("parameter"), CommandParameter(Of String())).setValue(parameter)
        End If
    End Sub

    Public Function getParameter() As String()
        Dim param As CommandParameter(Of String()) = getCommandParameter("parameter")
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
