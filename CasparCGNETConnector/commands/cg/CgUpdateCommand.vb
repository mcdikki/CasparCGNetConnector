﻿'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
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

Public Class CgUpdateCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("CG Update", "Sends new data to the template on specified layer")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer, ByVal data As String)
        MyBase.New("CG Update", "Sends new data to the template on specified layer")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getParameter("flashlayer"), CommandParameter(Of Integer)).setValue(flashlayer)
        If Not IsNothing(data) AndAlso data.Length > 0 Then DirectCast(getParameter("data"), CommandParameter(Of String)).setValue(data)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of Integer)("flashlayer", "The flashlayer", 0, False))
        addParameter(New CommandParameter(Of String)("data", "The xml data string", "", True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "CG " & getDestination(getParameter("channel"), getParameter("layer")) & " UPDATE"

        cmd = cmd & " " & DirectCast(getParameter("flashlayer"), CommandParameter(Of Integer)).getValue
        cmd = cmd & " " & DirectCast(getParameter("data"), CommandParameter(Of String)).getValue

        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
