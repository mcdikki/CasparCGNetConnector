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
'' Please let know of any changes and improofments you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Public Class CasparCGTransition

    Public Enum Transitions
        CUT = 0
        MIX = 1
        PUSH = 2
        WIPE = 3
        SLIDE = 4
    End Enum

    Public Enum Directions
        LEFT = 1
        RIGHT = 0
    End Enum

    Public Enum Tweens
        linear = 0
        easenone
        easeinquad
        easeoutquad
        easeinoutquad
        easeoutinquad
        easeincubic
        easeoutcubic
        easeinoutcubic
        easeoutincubic
        easeinquart
        easeoutquart
        easeinoutquart
        easeoutinquart
        easeinquint
        easeoutquint
        easeinoutquint
        easeoutinquint
        easeinsine
        easeoutsine
        easeinoutsine
        easeoutinsine
        easeinexpo
        easeoutexpo
        easeinoutexpo
        easeoutinexpo
        easeincirc
        easeoutcirc
        easeinoutcirc
        easeoutincirc
        easeinelastic
        easeoutelastic
        easeinoutelastic
        easeoutinelastic
        easeinback
        easeoutback
        easeinoutback
        easeoutintback
        easeoutbounce
        easeinbounce
        easeinoutbounce
        easeoutinbounce
    End Enum

    Private trans As Transitions
    Private duration As Integer
    Private direction As Directions
    Private tween As Tweens

    Public Sub New(ByVal transition As Transitions, Optional ByVal duration As Integer = 0, Optional ByVal direction As Directions = Directions.RIGHT, Optional ByVal tween As Tweens = Tweens.linear)
        '' Logik checken!!
        trans = transition
        Me.duration = duration
        Me.direction = direction
        Me.tween = tween
    End Sub

    Public Overloads Function toString() As String
        Return Transitions.GetName(GetType(Transitions), trans) & " " & duration & " " & Directions.GetName(GetType(Directions), direction) & " " & Tweens.GetName(GetType(Tweens), tween)
    End Function

End Class