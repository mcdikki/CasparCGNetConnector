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

Public Class RouteCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("ROUTE", "Routes a channel or layer to an other channel or layer")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal sourceChannel As Integer, ByVal sourceLayer As Integer)
        MyBase.New("ROUTE", "Routes a channel or layer to an other channel or layer")
        InitParameter()
        Init(channel, layer, sourceChannel, sourceLayer)
    End Sub


    Private Sub Init(ByVal destChannel As Integer, ByVal destLayer As Integer, ByVal sourceChannel As Integer, ByVal sourceLayer As Integer)
        If destChannel > 0 Then setChannel(destChannel)
        If destLayer > -1 Then setLayer(destLayer)
        If sourceChannel > 0 Then setSourceChannel(sourceChannel)
        If sourceLayer > -1 Then setSourceLayer(sourceLayer)
    End Sub

    Private Sub InitParameter()
        '' Add all paramters here:
        addCommandParameter(New ChannelParameter)
        addCommandParameter(New LayerParameter)
        addCommandParameter(New CommandParameter(Of Integer)("source channel", "The source channel (content)", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("source layer", "The source layer (content)", 0, True))
    End Sub

    Public Overrides Function getCommandString() As String
        checkParameter()
        Dim cmd As String = "ROUTE " & getDestination()
        cmd = cmd & " route://" & getDestination(getCommandParameter("source channel"), getCommandParameter("source layer"))

        Return escape(cmd)
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

    Public Sub setSourceChannel(ByVal sourceChannel As Integer)
        If sourceChannel > 0 Then
            DirectCast(getCommandParameter("source channel"), CommandParameter(Of Integer)).setValue(sourceChannel)
        Else
            Throw New ArgumentException("Illegal argument channel=" + sourceChannel + ". The parameter channel has to be greater than 0.")
        End If
    End Sub

    Public Function getSourceChannel() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("source channel")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setSourceLayer(ByVal sourceLayer As Integer)
        If sourceLayer < 0 Then
            Throw New ArgumentException("Illegal argument layer=" + sourceLayer + ". The parameter layer has to be greater or equal than 0.")
        Else
            DirectCast(getCommandParameter("source layer"), CommandParameter(Of Integer)).setValue(sourceLayer)
        End If
    End Sub

    Public Function getSourceLayer() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("source layer")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
