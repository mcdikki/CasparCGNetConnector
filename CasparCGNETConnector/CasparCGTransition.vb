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

Public Class CasparCGTransition

    Private trans As CasparCGUtil.Transitions
    Private duration As Integer
    Private direction As CasparCGUtil.Directions
    Private tween As CasparCGUtil.Tweens

    Public Sub New(ByVal transition As CasparCGUtil.Transitions, Optional ByVal duration As Integer = 0, Optional ByVal direction As CasparCGUtil.Directions = CasparCGUtil.Directions.RIGHT, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        '' Logik checken!!
        trans = transition
        Me.duration = duration
        Me.direction = direction
        Me.tween = tween
    End Sub

    Public Overloads Function toString() As String
        If trans = CasparCGUtil.Transitions.MIX OrElse trans = CasparCGUtil.Transitions.CUT Then
            Return CasparCGUtil.Transitions.GetName(GetType(CasparCGUtil.Transitions), trans) & " " & duration & " " & CasparCGUtil.Tweens.GetName(GetType(CasparCGUtil.Tweens), tween)
        End If
        Return CasparCGUtil.Transitions.GetName(GetType(CasparCGUtil.Transitions), trans) & " " & duration & " " & CasparCGUtil.Directions.GetName(GetType(CasparCGUtil.Directions), direction) & " " & CasparCGUtil.Tweens.GetName(GetType(CasparCGUtil.Tweens), tween)
    End Function

End Class