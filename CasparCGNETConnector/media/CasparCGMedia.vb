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

'' Base Class for all playable media in CasparCG which are
'' movies, stills, audios, colors and template

<Serializable()> _
Public MustInherit Class CasparCGMedia
    Private name As String
    Private path As String
    Private Infos As Dictionary(Of String, String)
    Private updated As Boolean = False
    Private uuid As New Guid
    Private thumbB64 As String = ""

    Public Enum MediaType
        STILL = 0
        MOVIE = 1
        AUDIO = 2
        TEMPLATE = 3
        COLOR = 4
    End Enum

    Public MustOverride Function getMediaType() As MediaType

    Public Sub New(ByVal name As String)
        Me.name = parseName(name)
        Me.path = parsePath(name)
        Infos = New Dictionary(Of String, String)
        uuid = Guid.NewGuid
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        Me.name = parseName(name)
        Me.path = parsePath(name)
        Infos = New Dictionary(Of String, String)
        parseXML(xml)
        uuid = Guid.NewGuid
    End Sub

    Public Function getUuid() As String
        Return uuid.ToString
    End Function

    Public MustOverride Function clone() As CasparCGMedia

    Public Function parseName(ByVal nameWithPath As String) As String
        If nameWithPath.Contains("\") Then
            Return nameWithPath.Substring(nameWithPath.LastIndexOf("\") + 1)
        ElseIf nameWithPath.Contains("/") Then
            Return nameWithPath.Substring(name.LastIndexOf("/") + 1)
        Else
            Return nameWithPath
        End If
    End Function

    Public Function parsePath(ByVal nameWithPath As String) As String
        If nameWithPath.Contains("\") Then
            Return nameWithPath.Substring(0, nameWithPath.LastIndexOf("\") + 1)
        ElseIf nameWithPath.Contains("/") Then
            Return nameWithPath.Substring(0, nameWithPath.LastIndexOf("/") + 1)
        Else
            Return ""
        End If
    End Function

    Public Overridable Sub parseXML(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            '' Add all mediaInformation found by INFO
            For Each info As MSXML2.IXMLDOMNode In configDoc.firstChild.childNodes
                setInfo(info.nodeName, info.nodeTypedValue)
            Next
        End If
    End Sub

    Public Function getName() As String
        Return name
    End Function

    Public Function getPath() As String
        Return path
    End Function

    Public Function getFullName() As String
        Return path & name
    End Function

    Public Function getBase64Thumb() As String
        Return thumbB64
    End Function

    Public Sub setBase64Thumb(ByRef base64Thumb As String)
        thumbB64 = base64Thumb
    End Sub

    Public Function getInfo(ByVal info As String) As String
        If Infos.ContainsKey(info) Then
            Return Infos.Item(info)
        Else : Return ""
        End If
    End Function

    Public Function getInfos() As Dictionary(Of String, String)
        Return Infos
    End Function

    Public Function containsInfo(ByVal info As String) As Boolean
        Return Infos.ContainsKey(info)
    End Function

    Public Sub setInfo(ByVal info As String, ByVal value As String)
        If Infos.ContainsKey(info) Then
            Infos.Item(info) = value
        Else
            Infos.Add(info, value)
        End If
    End Sub

    Public Sub addInfo(ByVal info As String, ByVal value As String)
        Infos.Add(info, value)
    End Sub

    Public Overrides Function toString() As String
        Dim out As String = getFullName() & " (" & getMediaType.ToString & ")"
        If getInfos.Count > 0 Then
            out = out & vbNewLine & "INFOS:"
            For Each infoKey As String In getInfos().Keys
                out = out & vbNewLine & vbTab & infoKey & " = " & getInfo(infoKey)
            Next
        End If
        Return out
    End Function

    ''
    '' Only for testing!
    '' To solve the problem that frehly startet items will be marked stopped as 
    '' the update accur to slow, this flag will be set if the first update arrived.
    '' Until this flag is true, the item won't be removed.
    ''

    Public Function hasBeenUpdated() As Boolean
        Return updated
    End Function

    Public Sub setUpdated(Optional updated As Boolean = True)
        Me.updated = updated
    End Sub

End Class