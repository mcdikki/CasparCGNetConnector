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

Public Class CgNextCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("CG Next", "Triggers a 'continue' in the template on the specified layer")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer)
        MyBase.New("CG Next", "Triggers a 'continue' in the template on the specified layer")
        DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        DirectCast(getCommandParameter("flashlayer"), CommandParameter(Of Integer)).setValue(flashlayer)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of Integer)("flashlayer", "The flashlayer", 0, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "CG " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " NEXT"

        cmd = cmd & " " & DirectCast(getCommandParameter("flashlayer"), CommandParameter(Of Integer)).getValue

        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
