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

Public Class PrintCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("PRINT", "Saves a screenshot of a given channel")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal file As String = "")
        MyBase.New("PRINT", "Saves a screenshot of a given channel")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If Not IsNothing(file) AndAlso file.Length > 0 Then DirectCast(getParameter("file"), CommandParameter(Of String)).setValue(file)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of String)("file", "The destination filename", "", True, {2, 0, 4}))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "PRINT " & getDestination(getParameter("channel"))
        If getParameter("file").isSet Then
            cmd = cmd & " " & DirectCast(getParameter("parameter"), CommandParameter(Of String)).getValue
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
