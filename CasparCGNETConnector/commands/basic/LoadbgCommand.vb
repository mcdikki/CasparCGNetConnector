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

Public Class LoadbgCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("LOADBG", "Loads a media to the background")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1, Optional ByVal media As CasparCGMedia = Nothing, Optional ByVal autostarting As Boolean = False, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "")
        MyBase.New("LOADBG", "Loads a media to the background")
        InitParameter()
        Init(channel, layer, media.getFullName, autostarting, looping, seek, length, transition, filter)
    End Sub

    Public Sub New(ByVal channel As Integer, Optional ByVal layer As Integer = -1, Optional ByVal media As String = "", Optional ByVal autostarting As Boolean = False, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "")
        MyBase.New("LOADBG", "Loads a media to the background")
        InitParameter()
        Init(channel, layer, media, autostarting, looping, seek, length, transition, filter)
    End Sub

    Private Sub Init(ByVal channel As Integer, ByVal layer As Integer, Optional ByVal media As String = "", Optional ByVal autostarting As Boolean = False, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "")
        If channel > 0 Then DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        If layer > -1 Then DirectCast(getParameter("layer"), CommandParameter(Of Integer)).setValue(layer)

        If media.Length > 0 Then
            DirectCast(getParameter("media"), CommandParameter(Of String)).setValue(media)
        End If
        If autostarting Then
            DirectCast(getParameter("autostarting"), CommandParameter(Of Boolean)).setValue(True)
        End If
        If looping Then
            DirectCast(getParameter("looping"), CommandParameter(Of Boolean)).setValue(looping)
        End If
        If Not IsNothing(transition) Then
            DirectCast(getParameter("transition"), CommandParameter(Of CasparCGTransition)).setValue(transition)
        End If
        If seek > 0 Then
            DirectCast(getParameter("seek"), CommandParameter(Of Integer)).setValue(seek)
        End If
        If length > 0 Then
            DirectCast(getParameter("length"), CommandParameter(Of Integer)).setValue(length)
        End If
        If filter.Length > 0 Then
            DirectCast(getParameter("filter"), CommandParameter(Of String)).setValue(filter)
        End If
    End Sub

    Private Sub InitParameter()
        '' Add all paramters here:
        addParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addParameter(New CommandParameter(Of String)("media", "The media to play", "", True))
        addParameter(New CommandParameter(Of Boolean)("autostarting", "Starts playing the media automatically when coming to foreground", False, True))
        addParameter(New CommandParameter(Of Boolean)("looping", "Loops the media", False, True))
        addParameter(New CommandParameter(Of Integer)("seek", "The Number of frames to seek before playing", 0, True))
        addParameter(New CommandParameter(Of Integer)("length", "The number of frames to play", 0, True))
        addParameter(New CommandParameter(Of CasparCGTransition)("transition", "The transition to perform at start", Nothing, True))
        addParameter(New CommandParameter(Of String)("filter", "The ffmpeg filter to apply", "", True))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "LOADBG " & getDestination(getParameter("channel"), getParameter("layer"))

        If getParameter("media").isSet Then
            cmd = cmd & " '" & DirectCast(getParameter("media"), CommandParameter(Of String)).getValue() & "'"
        End If
        If getParameter("autostarting").isSet AndAlso DirectCast(getParameter("autostarting"), CommandParameter(Of Boolean)).getValue() Then
            cmd = cmd & " AUTO"
        End If
        If getParameter("looping").isSet AndAlso DirectCast(getParameter("looping"), CommandParameter(Of Boolean)).getValue() Then
            cmd = cmd & " LOOP"
        End If
        If getParameter("transition").isSet Then
            cmd = cmd & " " & DirectCast(getParameter("transition"), CommandParameter(Of CasparCGTransition)).getValue().toString
        End If
        If getParameter("seek").isSet Then
            cmd = cmd & " SEEK " & DirectCast(getParameter("seek"), CommandParameter(Of Integer)).getValue()
        End If
        If getParameter("length").isSet Then
            cmd = cmd & " LENGTH " & DirectCast(getParameter("length"), CommandParameter(Of Integer)).getValue()
        End If
        If getParameter("filter").isSet Then
            cmd = cmd & " FILTER '" & DirectCast(getParameter("filter"), CommandParameter(Of String)).getValue() & "'"
        End If

        Return escape(cmd)
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function

End Class
