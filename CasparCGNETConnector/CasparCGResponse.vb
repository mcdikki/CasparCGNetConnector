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

Public Class CasparCGResponse

    Private cmd As String
    Private serverMessage As String
    Private returncode As CasparReturnCode
    Private command As String
    Private data As String
    Private xml As String

    Public Enum CasparReturnCode
        UNKNOWN_RETURNCODE = 0  ' This code is not known
        INFO = 100              ' 100 [action] - Information about an event.
        INFO_DATA = 101         ' 101[action] - Information about an event. A line of data is being returned. 
        OK_MULTI_DATA = 200     ' 200 [command] OK	- The command has been executed and several lines of data are being returned (terminated by an empty line).
        OK_DATA = 201           ' 201 [command] OK	- The command has been executed and a line of data is being returned
        OK = 202                ' 202 [command] OK	- The command has been executed 
        ERR_CMD_UNKNOWN = 400   ' 400 ERROR	- Command not understood
        ERR_CHANNEL = 401       ' 401 [command] ERROR	- Illegal video_channel
        ERR_PARAMETER_MISSING = 402 ' 402 [command] ERROR	- Parameter missing
        ERR_PARAMETER = 403     ' 403 [command] ERROR	- Illegal parameter
        ERR_MEDIA_UNKNOWN = 404 ' 404 [command] ERROR	- Media file not found
        ERR_SERVER = 500        ' 500 FAILED	- Internal server error
        ERR_SERVER_DATA = 501   ' 501 [command] FAILED	- Internal server error
        ERR_FILE_UNREADABLE = 502 ' 502 [command] FAILED	- Media file unreadable 
    End Enum

    Public Sub New(ByVal serverMessage As String, ByVal cmd As String)
        Me.cmd = cmd
        Me.serverMessage = serverMessage
        Me.returncode = parseReturnCode(serverMessage)
        Me.command = parseReturnCommand(serverMessage)
        Me.data = parseReturnData(serverMessage)
        Me.xml = parseXml(data)
    End Sub

    Public Shared Function parseReturnCode(ByVal serverMessage As String) As CasparReturnCode
        If Not IsNothing(serverMessage) Then
            serverMessage = Trim(serverMessage)
            If serverMessage.Length > 2 AndAlso IsNumeric(serverMessage.Substring(0, 3)) Then
                Dim returncode As Integer = Integer.Parse(serverMessage.Substring(0, 3))
                If [Enum].IsDefined(GetType(CasparReturnCode), returncode) Then
                    Return returncode
                End If
            End If
        End If
        Return 0
    End Function

    Public Shared Function parseReturnCommand(ByVal serverMessage As String) As String
        If Not IsNothing(serverMessage) AndAlso serverMessage.Length > 3 Then
            serverMessage = serverMessage.Trim().Substring(4) ' Code wegschneiden
            Return serverMessage.Substring(0, serverMessage.IndexOf(" "))
        End If
        Return ""
    End Function

    Public Shared Function parseReturnData(ByVal serverMessage As String) As String
        If Not IsNothing(serverMessage) AndAlso serverMessage.Length > 0 Then
            serverMessage.Substring(serverMessage.IndexOf(vbCr) + 1)
            ' Leerzeilen am ende entfernen
            Dim lines() = serverMessage.Split(vbCrLf)
            serverMessage = ""
            If lines.Length > 1 Then
                For i = 1 To lines.Length - 1
                    lines(i) = lines(i).Replace("vbcr", "").Replace(vbLf, "").Trim(vbVerticalTab).Trim(vbTab).Trim(vbNullChar).Trim(vbNewLine)
                    If lines(i).Length > 0 Then
                        If i = 1 Then
                            serverMessage = lines(i)
                        Else
                            serverMessage = serverMessage & vbNewLine & lines(i)
                        End If
                    End If
                Next
            End If
            Return serverMessage
        End If
        Return ""
    End Function

    Public Shared Function parseXml(ByVal data As String) As String
        If Not IsNothing(data) Then
            Dim xml As String = ""
            If data.Contains("<?") And data.Contains("?>") Then
                xml = data
            End If
            While xml.Contains("<?") And xml.Contains("?>")
                Dim start As Integer = xml.IndexOf("<")
                While Not xml.Substring(start, 2) = "<?"
                    start = xml.Substring(start + 2).IndexOf("<")
                End While
                Dim ende As Integer = xml.IndexOf(">")
                While Not xml.Substring(ende - 1, 2) = "?>"
                    ende = xml.Substring(ende + 2).IndexOf(">")
                End While
                xml = xml.Remove(start, ende + 1 - start)
            End While
            Return xml
        End If
        Return ""
    End Function

    Public Function getCode() As CasparReturnCode
        Return returncode
    End Function

    Public Function getCommand() As String
        Return cmd
    End Function

    Public Function getReturnedCommand() As String
        Return command
    End Function

    Public Function getData() As String
        Return data
    End Function

    Public Function getXMLData() As String
        Return xml
    End Function

    Public Function getServerMessage() As String
        Return serverMessage
    End Function


    Public Function isOK() As Boolean
        If returncode >= 200 AndAlso returncode < 300 Then
            Return True
        Else : Return False
        End If
    End Function

    Public Function isERR() As Boolean
        If returncode >= 400 Then
            Return True
        Else : Return False
        End If
    End Function

    Public Function isINFO() As Boolean
        If returncode >= 100 AndAlso returncode < 200 Then
            Return True
        Else : Return False
        End If
    End Function

    Public Function isUNKNOWN() As Boolean
        If returncode = CasparReturnCode.UNKNOWN_RETURNCODE Then
            Return True
        Else : Return False
        End If
    End Function

End Class