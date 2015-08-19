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

Imports System.ComponentModel

Public Class CasparCGTransition
    Implements INotifyPropertyChanged


    Public Property transition As CasparCGUtil.Transitions
        Get
            Return _trans
        End Get
        Set(value As CasparCGUtil.Transitions)
            _trans = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("transition"))
        End Set
    End Property

    Public Property duration As Integer
        Get
            Return _duration
        End Get
        Set(value As Integer)
            _duration = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("duration"))
        End Set
    End Property

    Public Property direction As CasparCGUtil.Directions
        Get
            Return _direction
        End Get
        Set(value As CasparCGUtil.Directions)
            _direction = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("direction"))
        End Set
    End Property

    Public Property tween As CasparCGUtil.Tweens
        Get
            Return _tween
        End Get
        Set(value As CasparCGUtil.Tweens)
            _tween = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("tween"))
        End Set
    End Property


    Private _trans As CasparCGUtil.Transitions
    Private _duration As Integer
    Private _direction As CasparCGUtil.Directions
    Private _tween As CasparCGUtil.Tweens

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New(ByVal transition As CasparCGUtil.Transitions, Optional ByVal duration As Integer = 0, Optional ByVal direction As CasparCGUtil.Directions = CasparCGUtil.Directions.RIGHT, Optional ByVal tween As CasparCGUtil.Tweens = CasparCGUtil.Tweens.linear)
        '' Logik checken!!
        Me.transition = transition
        Me.duration = duration
        Me.direction = direction
        Me.tween = tween
    End Sub

    Public Overloads Function toString() As String
        If transition = CasparCGUtil.Transitions.MIX OrElse transition = CasparCGUtil.Transitions.CUT Then
            Return CasparCGUtil.Transitions.GetName(GetType(CasparCGUtil.Transitions), transition) & " " & duration & " " & CasparCGUtil.Tweens.GetName(GetType(CasparCGUtil.Tweens), tween)
        End If
        Return CasparCGUtil.Transitions.GetName(GetType(CasparCGUtil.Transitions), transition) & " " & duration & " " & CasparCGUtil.Directions.GetName(GetType(CasparCGUtil.Directions), direction) & " " & CasparCGUtil.Tweens.GetName(GetType(CasparCGUtil.Tweens), tween)
    End Function


End Class