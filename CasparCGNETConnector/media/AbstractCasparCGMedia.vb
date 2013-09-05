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
Public MustInherit Class AbstractCasparCGMedia
    Private name As String
    Private path As String
    Private Infos As Dictionary(Of String, String)
    Private updated As Boolean = False
    Private uuid As String
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
        uuid = getFullName() & "(" & getMediaType.ToString & ")"
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        Me.name = parseName(name)
        Me.path = parsePath(name)
        Infos = New Dictionary(Of String, String)
        parseXML(xml)
        uuid = getFullName() & "(" & getMediaType.ToString & ")"
    End Sub

    Public Function getUuid() As String
        Return uuid
    End Function

    Public MustOverride Function clone() As AbstractCasparCGMedia

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


    ''' <summary>
    ''' Fills the metadata informations given by the casparCG servers info command. The media will be loaded to background on the given channel and the first free layer. After polling the the INFO, the layer will be cleared again. It is recommend to use a non production channel for this
    ''' </summary>
    ''' <param name="connection">The connection on which the info should be requested</param>
    ''' <param name="channel">The channel to load the media on</param>
    ''' <remarks></remarks>
    Public Overridable Sub fillMediaInfo(ByRef connection As CasparCGConnection, Optional ByVal channel As Integer = 1)
        If connection.isConnected() Then
            Dim layer = connection.getFreeLayer(channel)
            Dim cmd As ICommand = New LoadbgCommand(channel, layer, getFullName)
            Dim clear = New ClearCommand(channel, layer)
            If cmd.execute(connection).isOK Then
                Dim infoDoc As New MSXML2.DOMDocument
                cmd = New InfoCommand(channel, layer, True)
                If infoDoc.loadXML(cmd.execute(connection).getXMLData()) AndAlso Not IsNothing(infoDoc.selectSingleNode("producer").selectSingleNode("destination")) Then
                    If infoDoc.selectSingleNode("producer").selectSingleNode("destination").selectSingleNode("producer").selectSingleNode("type").nodeTypedValue.Equals("separated-producer") Then
                        parseXML(infoDoc.selectSingleNode("producer").selectSingleNode("destination").selectSingleNode("producer").selectSingleNode("fill").selectSingleNode("producer").xml)
                    Else
                        parseXML(infoDoc.selectSingleNode("producer").selectSingleNode("destination").selectSingleNode("producer").xml)
                    End If
                Else
                    logger.err("CasparCGMedia.fillMediaInfo: Error loading xml data received from server for " & toString() & ". Error: " & infoDoc.parseError.reason)
                    logger.err("CasparCGMedia.fillMediaInfo: ServerMessages dump: " & cmd.getResponse.getServerMessage)
                End If
            Else
                logger.err("CasparCGMedia.fillMediaInfo: Error getting media information for " & getName() & ". Server messages was: " & cmd.getResponse.getServerMessage)
            End If
            clear.execute(connection)
        End If
    End Sub

    ''' <summary>
    ''' Retrieves a thumbnail (if present and support) using the given serverconnection.
    ''' </summary>
    ''' <param name="connection">the connection to use</param>
    Public Overridable Sub fillThumbnail(ByRef connection As CasparCGConnection)
        ' get Thumbnail
        Dim cmd = New ThumbnailRetrieveCommand(Me)
        If getMediaType() = MediaType.MOVIE Or getMediaType() = MediaType.STILL AndAlso cmd.isCompatible(connection) Then
            If cmd.isCompatible(connection) AndAlso cmd.execute(connection).isOK Then
                setBase64Thumb(cmd.getResponse.getData)
            End If
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
        thumbB64 = CasparCGUtil.repairBase64(base64Thumb)
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

    Public Sub setInfos(ByVal infos As Dictionary(Of String, String))
        If Not IsNothing(infos) Then
            Me.Infos = infos
        Else
            Me.Infos.Clear()
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

    Public Overridable Function toXml() As MSXML2.DOMDocument
        Dim configDoc As New MSXML2.DOMDocument
        Dim pnode As MSXML2.IXMLDOMNode
        Dim node As MSXML2.IXMLDOMNode
        ' Kopfdaten eintragen
        pnode = configDoc.createElement("media")
        node = configDoc.createElement("name")
        node.nodeTypedValue = getFullName()
        pnode.appendChild(node)
        node = configDoc.createElement("type")
        node.nodeTypedValue = getMediaType()
        pnode.appendChild(node)
        node = configDoc.createElement("typename")
        node.nodeTypedValue = getMediaType().ToString
        pnode.appendChild(node)
        node = configDoc.createElement("uuid")
        node.nodeTypedValue = getUuid()
        pnode.appendChild(node)

        '' Add all mediaInformation found by INFO
        node = configDoc.createElement("infos")
        Dim inode As MSXML2.IXMLDOMNode
        For Each info As String In getInfos().Keys
            'setInfo(info.nodeName, info.nodeTypedValue)
            inode = configDoc.createElement(info)
            inode.nodeTypedValue = getInfo(info)
            node.appendChild(inode)
        Next
        pnode.appendChild(node)

        node = configDoc.createElement("thumb")
        node.nodeTypedValue = getBase64Thumb()
        pnode.appendChild(node)

        configDoc.appendChild(pnode)
        Return configDoc
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