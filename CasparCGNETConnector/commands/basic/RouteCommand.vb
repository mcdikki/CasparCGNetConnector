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

    Public Sub New(ByVal destChannel As Integer, ByVal destLayer As Integer, ByVal srcChannel As Integer, ByVal srcLayer As Integer)
        MyBase.New("ROUTE", "Routes a channel or layer to an other channel or layer")
        InitParameter()
        Init(destChannel, destLayer, srcChannel, srcLayer)
    End Sub


    Private Sub Init(ByVal destChannel As Integer, ByVal destLayer As Integer, ByVal srcChannel As Integer, ByVal srcLayer As Integer)
        If destChannel > 0 Then DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(destChannel)
        If destLayer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(destLayer)
        If srcChannel > 0 Then DirectCast(getParameter("src channel"), CommandParameter(Of Integer)).setValue(srcChannel)
        If srcLayer > -1 Then DirectCast(getParameter("src layer"), CommandParameter(Of Integer)).setValue(srcLayer)
    End Sub

    Private Sub InitParameter()
        '' Add all paramters here:
        addParameter(New CommandParameter(Of Integer)("channel", "The destination channel", 1, False))
        addParameter(New CommandParameter(Of Integer)("layer", "The destination layer", 0, True))
        addParameter(New CommandParameter(Of Integer)("src channel", "The source channel (content)", 1, False))
        addParameter(New CommandParameter(Of Integer)("src layer", "The source layer (content)", 0, True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "ROUTE " & getDestination(getParameter("channel"), getParameter("layer"))
        cmd = cmd & " ROUTE:\\" & getDestination(getParameter("src channel"), getParameter("src layer"))

        Return escape(cmd)
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
