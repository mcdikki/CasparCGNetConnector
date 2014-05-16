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
        LoadbgCommand
        RouteCommand
        PauseCommand
        ResumeCommand
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
        InfoServerCommand
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
        MixerOpacityCommand
        MixerBrightnessCommand
        MixerSaturationCommand
        MixerLevelsCommand
        MixerVolumeCommand
        MixerMastervolumeCommand
        MixerStraightAlphaOutputCommand
        ChannelGridCommand

    End Enum

    ''' <summary>
    ''' Returns an instance of the given command or nothing. You have to fill the command parameter your self before calling execute!
    ''' </summary>
    ''' <param name="command">The command</param>
    ''' <returns>an instance of the requested command or nothing if the requested command was not found</returns>
    ''' <remarks></remarks>
    Public Shared Function getCommand(ByVal command As Command) As AbstractCommand
        Return getCommand(command.ToString)
    End Function

    Public Shared Function getCommand(ByVal command As String) As AbstractCommand
        If Not command.StartsWith("CasparCGNETConnector.") Then
            command = "CasparCGNETConnector." & command
        End If
        Dim cmd As AbstractCommand = getInstance(Type.GetType(command))
        Return cmd
    End Function

    Private Shared Function getInstance(ByVal t As System.Type) As Object
        Return t.GetConstructor(New System.Type() {}).Invoke(New Object() {})
    End Function

    ''' <summary>
    ''' Escapes the string str as needed for casparCG Server
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function escape(ByVal str As String) As String
        ' Backslash
        str = str.Replace("\", "\\")

        ' Hochkommata
        str = str.Replace("'", "\'")
        str = str.Replace("""", "\""")
        Return str
    End Function

End Class

