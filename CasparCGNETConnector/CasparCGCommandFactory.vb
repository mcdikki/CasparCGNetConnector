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

Imports System

Public Class CasparCGCommandFactory

    ''' <summary>
    ''' Enummerates the possible CasparCGCommands
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Command
        'Basic cmd
        PlayCommand
        StopCommand
        LoadCommand
        LoadBgCommand
        PauseCommand
        CallCommand
        SwapCommand
        ClearCommand
        AddCommand
        RemoveCommand
        PrintCommand
        SetCommand
        ByeCommand
        KillCommand
        RestartCommand

        'Data cmd
        DataListCommand
        DataStoreCommand
        DataRetrieveCommand
        DataRemoveCommand

        'Query cmd
        ClsCommand
        TlsCommand
        CinfCommand
        InfoCommand
        InfoTemplateCommand
        InfoConfigCommand
        InfoPathsCommand
        InfoServer
        InfoSystemCommand
        VersionServerCommand
        VersionFlashCommand
        VersionTemplatehostCommand

        'Thumbnail cmd
        ThumbnailListCommand
        ThumbnailGenerateCommand
        ThumbnailGenerateAllCommand
        ThumbnailRetrieveCommand

        'CG cmd
        CgAddCommand
        CgRemoveCommand
        CgPlayCommand
        CgStopCommand
        CgNextCommand
        CgClearCommand
        CgUpdateCommand
        CgInvokeCommand

        'Mixer cmd
        MixerClearCommand
        MixerBlendCommand
        MixerKeyerCommand
        MixerFillCommand
        MixerClipCommand
        MixerChromaCommand
        MixerContrastCommand
        MixerVideoOpacityCommand
        MixerBrightnessCommand
        MixerSaturationCommand
        MixerLevelsCommand
        MixerVolumeCommand
        MixerMastervolumeCommand
        MixerStraighAlphaOutputCommand
        ChannelGridCommand

    End Enum

    ''' <summary>
    ''' Returns an instance of the given command or nothing. You have to fill the command parameter your self before calling execute!
    ''' </summary>
    ''' <param name="command">The command</param>
    ''' <returns>an instance of the requested command or nothing if the requested command was not found</returns>
    ''' <remarks></remarks>
    Public Shared Function getCommand(ByVal command As Command) As ICommand
        Dim cmd As ICommand = getInstance(Type.GetType("CasparCGNETConnector." & command.ToString))
        'Throw New NotImplementedException("The getCommand() function is not implemented yet. Please instanciate the desired command class by your self")
        Return cmd
    End Function

    Private Shared Function getInstance(ByVal t As System.Type) As Object
        Return t.GetConstructor(New System.Type() {}).Invoke(New Object() {})
    End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getLoadbg(ByVal channel As Integer, ByVal layer As Integer, ByVal media As String, Optional ByRef autostarting As Boolean = False, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
    '    Dim cmd As String = "LOADBG " & getDestination(channel, layer) & " '" & media & "'"

    '    If looping Then
    '        cmd = cmd & " LOOP"
    '    End If
    '    If Not IsNothing(transition) Then
    '        cmd = cmd & " " & transition.toString
    '    End If
    '    If seek > 0 Then
    '        cmd = cmd & " SEEK " & seek
    '    End If
    '    If length > 0 Then
    '        cmd = cmd & " LENGTH " & length
    '    End If
    '    If autostarting Then
    '        cmd = cmd & " AUTO"
    '    End If
    '    If filter.Length > 0 Then
    '        cmd = cmd & " FILTER " & filter
    '    End If

    '    Return escape(cmd)
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getLoad(ByVal channel As Integer, ByVal layer As Integer, ByVal media As String, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
    '    Dim cmd As String = "LOAD " & getDestination(channel, layer) & " '" & media & "'"

    '    If looping Then
    '        cmd = cmd & " LOOP"
    '    End If
    '    If Not IsNothing(transition) Then
    '        cmd = cmd & " " & transition.toString
    '    End If
    '    If seek > 0 Then
    '        cmd = cmd & " SEEK " & seek
    '    End If
    '    If length > 0 Then
    '        cmd = cmd & " LENGTH " & length
    '    End If
    '    If filter.Length > 0 Then
    '        cmd = cmd & " FILTER " & filter
    '    End If

    '    Return escape(cmd)
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getPlay(ByVal channel As Integer, ByVal layer As Integer, Optional ByVal media As String = "", Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
    '    Dim cmd As String = "PLAY " & getDestination(channel, layer)

    '    If media.Length > 0 Then
    '        cmd = cmd & " '" & media & "'"
    '    End If
    '    If looping Then
    '        cmd = cmd & " LOOP"
    '    End If
    '    If Not IsNothing(transition) Then
    '        cmd = cmd & " " & transition.toString
    '    End If
    '    If seek > 0 Then
    '        cmd = cmd & " SEEK " & seek
    '    End If
    '    If length > 0 Then
    '        cmd = cmd & " LENGTH " & length
    '    End If
    '    If filter.Length > 0 Then
    '        cmd = cmd & " FILTER " & filter
    '    End If

    '    Return escape(cmd)
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getPlay(ByVal channel As Integer, ByVal layer As Integer, ByVal media As CasparCGMedia, Optional ByVal looping As Boolean = False, Optional ByVal fill As Boolean = True, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
    '    If IsNothing(media) Then
    '        Return getPlay(channel, layer, , looping, seek, length, transition, filter)
    '    Else
    '        If fill Then
    '            If seek = 0 AndAlso media.containsInfo("frame-number") AndAlso media.containsInfo("nb-frames") AndAlso media.getInfo("frame-number") < media.getInfo("nb-frames") Then
    '                seek = Long.Parse(media.getInfo("frame-number"))
    '            End If
    '            If length = 0 AndAlso media.containsInfo("nb-frames") Then
    '                length = media.getInfo("nb-frames")
    '            End If
    '        End If
    '        Return getPlay(channel, layer, media.getFullName, looping, seek, length, transition, filter)
    '    End If
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCall(ByVal channel As Integer, ByVal layer As Integer, Optional ByVal looping As Boolean = False, Optional ByVal seek As Long = 0, Optional ByVal length As Long = 0, Optional ByVal transition As CasparCGTransition = Nothing, Optional ByVal filter As String = "") As String
    '    Dim cmd As String = "CALL " & getDestination(channel, layer)

    '    If looping Then
    '        cmd = cmd & " LOOP"
    '    End If
    '    If Not IsNothing(transition) Then
    '        cmd = cmd & " " & transition.toString
    '    End If
    '    If seek > 0 Then
    '        cmd = cmd & " SEEK " & seek
    '    End If
    '    If length > 0 Then
    '        cmd = cmd & " LENGTH " & length
    '    End If
    '    If filter.Length > 0 Then
    '        cmd = cmd & " FILTER " & filter
    '    End If

    '    Return cmd
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getSwap(ByVal channelA As Integer, ByVal channelB As Integer) As String
    '    Return "SWAP " & channelA & " " & channelB
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getSwap(ByVal channelA As Integer, ByVal channelB As Integer, ByVal layerA As Integer, ByVal layerB As Integer) As String
    '    Return "SWAP " & channelA & "-" & layerA & " " & channelB & "-" & layerB
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getStop(ByVal channel As Integer, ByVal layer As Integer) As String
    '    Return "STOP " & getDestination(channel, layer)
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getPause(ByVal channel As Integer, ByVal layer As Integer) As String
    '    Return "PAUSE " & getDestination(channel, layer)
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getClear(Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1) As String
    '    Dim cmd As String = "CLEAR"
    '    If channel > -1 Then
    '        cmd = cmd & " " & getDestination(channel, layer)
    '    End If
    '    Return cmd
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getInfo(Optional ByVal channel As Integer = -1, Optional ByVal layer As Integer = -1, Optional ByVal onlyBackground As Boolean = False, Optional ByVal onlyForeground As Boolean = False) As String
    '    Dim cmd As String = "INFO"
    '    If channel > -1 Then
    '        cmd = cmd & " " & getDestination(channel, layer)
    '        If layer > -1 Then
    '            If onlyBackground Then
    '                cmd = cmd & " B"
    '            ElseIf onlyForeground Then
    '                cmd = cmd & " F"
    '            End If
    '        End If
    '    End If
    '    Return cmd
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getInfo(ByRef template As CasparCGTemplate) As String
    '    Return escape("INFO TEMPLATE '" & template.getFullName & "'")
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getThumbnail(ByVal media As String) As String
    '    Return escape("THUMBNAIL RETRIEVE '" & media & "'")
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getThumbnail(ByVal media As CasparCGMedia) As String
    '    Return escape("THUMBNAIL RETRIEVE '" & media.getFullName & "'")
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getThumbnailGenerate(ByVal media As String) As String
    '    Return escape("THUMBNAIL GENERATE '" & media & "'")
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getThumbnailGenerate(ByVal media As CasparCGMedia) As String
    '    Return escape("THUMBNAIL GENERATE '" & media.getFullName & "'")
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getThumbnailList() As String
    '    Return "THUMBNAIL LIST"
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getVersion(Optional ByVal ofPart As String = "Server") As String
    '    Return "VERSION " & ofPart
    'End Function

    ' '' CG CMD für Flashtemplates
    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGAdd(ByVal channel As Integer, ByVal layer As Integer, ByVal template As CasparCGTemplate, ByVal flashlayer As Integer, Optional ByVal playOnLoad As Boolean = False) As String
    '    Dim cmd As String = "CG " & getDestination(channel, layer) & " ADD " & flashlayer & " " & template.getFullName

    '    If playOnLoad Then
    '        cmd = cmd & " 1 "
    '    Else
    '        cmd = cmd & " 0 "
    '    End If
    '    cmd = cmd & template.getData.toXML
    '    Return escape(cmd)
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGRemove(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
    '    Return "CG " & getDestination(channel, layer) & " REMOVE " & flashlayer
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGPlay(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
    '    Return "CG " & getDestination(channel, layer) & " PLAY " & flashlayer
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGStop(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
    '    Return "CG " & getDestination(channel, layer) & " STOP " & flashlayer
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGNext(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer) As String
    '    Return "CG " & getDestination(channel, layer) & " NEXT " & flashlayer
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGUpdate(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer, ByRef data As CasparCGTemplateData) As String
    '    Return "CG " & getDestination(channel, layer) & " UPDATE " & flashlayer & " " & escape(data.toXML)
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGInvoke(ByVal channel As Integer, ByVal layer As Integer, ByVal flashlayer As Integer, ByVal method As String) As String
    '    Return "CG " & getDestination(channel, layer) & " INVOKE " & flashlayer & " " & method
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCGClear(ByVal channel As Integer, ByVal layer As Integer) As String
    '    Return "CG " & getDestination(channel, layer) & " CLEAR"
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Private Shared Function getDestination(ByVal channel As Integer, ByVal layer As Integer) As String
    '    Dim cmd As String
    '    If channel > -1 Then
    '        cmd = channel
    '    Else
    '        cmd = "0"
    '    End If
    '    If layer > -1 Then
    '        cmd = cmd & "-" & layer
    '    End If
    '    Return cmd
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getCls() As String
    '    Return "CLS"
    'End Function

    '<Obsolete("This method is deprecated and will be remove with the next release. Please see the ICommand classes isntead.")> _
    'Public Shared Function getTls() As String
    '    Return "TLS"
    'End Function


    ' ''' <summary>
    ' ''' Escapes the string str as needed for casparCG Server
    ' ''' </summary>
    ' ''' <param name="str"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function escape(ByVal str As String) As String
    '    ' Backslash
    '    str = str.Replace("\", "\\")

    '    ' Hochkommata
    '    str = str.Replace("'", "\'")
    '    str = str.Replace("""", "\""")
    '    Return str
    'End Function

End Class

