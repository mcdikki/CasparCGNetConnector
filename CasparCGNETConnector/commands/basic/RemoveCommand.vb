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

    Public Sub New(ByVal channel As Integer, ByVal parameter() As String)
        MyBase.New("REMOVE", "Removes a comsumer from a given channel")
        InitParameter()
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        DirectCast(getCommandParameter("parameter"), CommandParameter(Of String())).setValue(parameter)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of String)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of String())("parameter", "The paramter list", {}, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "REMOVE " & getDestination(getCommandParameter("channel"))
        If getCommandParameter("parameter").isSet AndAlso getParameter.Length > 0 Then
            For Each p In getParameter()
                cmd = cmd & " " & p
            Next
        Else
            Throw New ArgumentNullException("The REMOVE command needs at least one parameter to be defined. Empty parameter lists are not allowed.")
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

    Public Sub setParamter(ByVal parameter As String())
        If Not IsNothing(parameter) Then
            DirectCast(getCommandParameter("parameter"), CommandParameter(Of String())).setValue(parameter)
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
