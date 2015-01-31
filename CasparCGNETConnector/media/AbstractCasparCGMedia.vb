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

Imports System.Runtime.CompilerServices
Imports System.ComponentModel

<Serializable()> _
Public MustInherit Class AbstractCasparCGMedia
    Implements ICasparCGMedia

#Region "Properties"
    Public Property Name As String Implements ICasparCGMedia.Name
        Get
            Return _name
        End Get
        Protected Set(value As String)
            _name = value
            NotifyPropertyChanged()
            NotifyPropertyChanged("FullName")
        End Set
    End Property

    Public Property Path As String Implements ICasparCGMedia.Path
        Get
            Return _path
        End Get
        Protected Set(value As String)
            _path = value
            NotifyPropertyChanged()
            NotifyPropertyChanged("FullName")
        End Set
    End Property

    Public ReadOnly Property FullName As String Implements ICasparCGMedia.FullName
        Get
            Return Path & Name
        End Get
    End Property

    Public Property Uuid As String Implements ICasparCGMedia.Uuid
        Get
            Return _uuid
        End Get
        Private Set(value As String)
            _uuid = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public MustOverride ReadOnly Property MediaType As ICasparCGMedia.MediaTypes Implements ICasparCGMedia.MediaType

    Public Property Infos As Dictionary(Of String, String) Implements ICasparCGMedia.Infos
        Get
            Return _Infos
        End Get
        Set(value As Dictionary(Of String, String))
            If value Is Nothing Then
                value = New Dictionary(Of String, String)
            End If
            _Infos = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property Base64Thumbnail As String Implements ICasparCGMedia.Base64Thumbnail
        Get
            Return _thumbB64
        End Get
        Set(value As String)
            _thumbB64 = CasparCGUtil.repairBase64(value)
            NotifyPropertyChanged()
        End Set
    End Property
#End Region

#Region "Variables"
    Private _name As String
    Private _path As String
    Private _Infos As Dictionary(Of String, String)
    Private _uuid As String
    Private _thumbB64 As String = ""
#End Region

#Region "Events"
    Public Event MediaFilled(ByRef sender As ICasparCGMedia) Implements ICasparCGMedia.MediaFilled
    Public Event ThumbnailFilled(ByRef sender As ICasparCGMedia) Implements ICasparCGMedia.ThumbnailFilled
    Public Event PropertyChanged As PropertyChangedEventHandler Implements ICasparCGMedia.PropertyChanged
    Public Event InfoChanged(ByRef sender As ICasparCGMedia, ByVal info As KeyValuePair(Of String, String)) Implements ICasparCGMedia.InfoChanged
#End Region

#Region "Constructors"
    Public Sub New(ByVal name As String)
        Me.Name = parseName(name)
        Me.Path = parsePath(name)
        Me.Infos = New Dictionary(Of String, String)
        Me.Uuid = FullName & "(" & MediaType.ToString & ")"
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        Me.Name = parseName(name)
        Me.Path = parsePath(name)
        Me.Infos = New Dictionary(Of String, String)
        parseXML(xml)
        Me.Uuid = FullName() & "(" & MediaType.ToString & ")"
    End Sub
#End Region

#Region "Abstract Methods"
    Public MustOverride Function clone() As ICasparCGMedia Implements ICasparCGMedia.clone
#End Region

#Region "Methods"
    Private Function parseName(ByVal nameWithPath As String) As String
        If nameWithPath.Contains("\") Then
            Return nameWithPath.Substring(nameWithPath.LastIndexOf("\") + 1)
        ElseIf nameWithPath.Contains("/") Then
            Return nameWithPath.Substring(Name.LastIndexOf("/") + 1)
        Else
            Return nameWithPath
        End If
    End Function

    Private Function parsePath(ByVal nameWithPath As String) As String
        If nameWithPath.Contains("\") Then
            Return nameWithPath.Substring(0, nameWithPath.LastIndexOf("\") + 1)
        ElseIf nameWithPath.Contains("/") Then
            Return nameWithPath.Substring(0, nameWithPath.LastIndexOf("/") + 1)
        Else
            Return ""
        End If
    End Function

    Public Overridable Sub parseXML(ByVal xml As String) Implements ICasparCGMedia.parseXML
        Dim configDoc As New Xml.XmlDocument
        configDoc.LoadXml(xml)
        If configDoc.HasChildNodes Then
            '' Add all mediaInformation found by INFO
            For Each info As Xml.XmlNode In configDoc.FirstChild.ChildNodes
                setInfo(info.Name, info.InnerText)
            Next
        End If
    End Sub


    ''' <summary>
    ''' Fills the metadata informations given by the casparCG servers info command. The media will be loaded to background on the given channel and the first free layer. After polling the the INFO, the layer will be cleared again. It is recommend to use a non production channel for this
    ''' </summary>
    ''' <param name="connection">The connection on which the info should be requested</param>
    ''' <param name="channel">The channel to load the media on</param>
    ''' <remarks></remarks>
    Public Overridable Sub fillMediaInfo(ByRef connection As ICasparCGConnection, Optional ByVal channel As Integer = 1) Implements ICasparCGMedia.fillMediaInfo
        If connection.isConnected() Then
            Dim layer = connection.getFreeLayer(channel)
            Dim cmd As AbstractCommand = New LoadbgCommand(channel, layer, FullName)
            Dim clear = New ClearCommand(channel, layer)
            If cmd.execute(connection).isOK Then
                Dim infoDoc As New Xml.XmlDocument
                cmd = New InfoCommand(channel, layer, True)
                infoDoc.LoadXml(cmd.execute(connection).getXMLData())
                If Not IsNothing(infoDoc.SelectSingleNode("producer").SelectSingleNode("destination")) Then
                    If infoDoc.SelectSingleNode("producer").SelectSingleNode("destination").SelectSingleNode("producer").SelectSingleNode("type").FirstChild.Value.Equals("separated-producer") Then
                        parseXML(infoDoc.SelectSingleNode("producer").SelectSingleNode("destination").SelectSingleNode("producer").SelectSingleNode("fill").SelectSingleNode("producer").OuterXml)
                    Else
                        parseXML(infoDoc.SelectSingleNode("producer").SelectSingleNode("destination").SelectSingleNode("producer").OuterXml)
                    End If
                    RaiseEvent MediaFilled(Me)
                Else
                    logger.err("CasparCGMedia.fillMediaInfo: Error loading xml data received from server for " & toString() & ". The xml is not as expected.")
                    logger.err("CasparCGMedia.fillMediaInfo: ServerMessages dump: " & cmd.getResponse.getServerMessage)
                End If
            Else
                logger.err("CasparCGMedia.fillMediaInfo: Error getting media information for " & Name & ". Server messages was: " & cmd.getResponse.getServerMessage)
            End If
            clear.execute(connection)
        End If
    End Sub

    ''' <summary>
    ''' Retrieves a thumbnail (if present and support) using the given serverconnection.
    ''' </summary>
    ''' <param name="connection">the connection to use</param>
    Public Overridable Sub fillThumbnail(ByRef connection As ICasparCGConnection) Implements ICasparCGMedia.fillThumbnail
        ' get Thumbnail
        Dim cmd = New ThumbnailRetrieveCommand(Me)
        If MediaType = ICasparCGMedia.MediaTypes.MOVIE Or MediaType = ICasparCGMedia.MediaTypes.STILL AndAlso cmd.isCompatible(connection) Then
            If cmd.isCompatible(connection) AndAlso cmd.execute(connection).isOK Then
                Base64Thumbnail = (cmd.getResponse.getData)
                RaiseEvent ThumbnailFilled(Me)
            End If
        End If
    End Sub

    Public Function getInfo(ByVal info As String) As String Implements ICasparCGMedia.getInfo
        If _Infos.ContainsKey(info) Then
            Return _Infos.Item(info)
        Else : Return ""
        End If
    End Function

    Public Function containsInfo(ByVal info As String) As Boolean Implements ICasparCGMedia.containsInfo
        Return Infos.ContainsKey(info)
    End Function

    Public Sub setInfo(ByVal info As String, ByVal value As String) Implements ICasparCGMedia.setInfo
        If Infos.ContainsKey(info) Then
            Infos.Item(info) = value
            'RaiseEvent InfoChanged(Me, New KeyValuePair(Of String, String)(info, value))
        Else
            Infos.Add(info, value)
        End If
    End Sub

    Public Sub addInfo(ByVal info As String, ByVal value As String) Implements ICasparCGMedia.addInfo
        Infos.Add(info, value)
        RaiseEvent InfoChanged(Me, New KeyValuePair(Of String, String)(info, value))
    End Sub

    Public Overrides Function toString() As String Implements ICasparCGMedia.toString
        Dim out As String = FullName() & " (" & MediaType.ToString & ")"
        If Infos.Count > 0 Then
            out = out & vbNewLine & "INFOS:"
            For Each info In Infos
                out = out & vbNewLine & vbTab & info.Key & " = " & info.Value
            Next
        End If
        Return out
    End Function

    Public Overridable Function toXml() As Xml.XmlDocument Implements ICasparCGMedia.toXml
        Dim configDoc As New Xml.XmlDocument()
        Dim pnode As Xml.XmlNode
        Dim node As Xml.XmlNode
        ' Kopfdaten eintragen
        pnode = configDoc.CreateElement("media")
        node = configDoc.CreateElement("name")
        node.InnerText = FullName
        pnode.AppendChild(node)
        node = configDoc.CreateElement("type")
        node.InnerText = MediaType
        pnode.AppendChild(node)
        node = configDoc.CreateElement("typename")
        node.InnerText = MediaType.ToString
        pnode.AppendChild(node)
        node = configDoc.CreateElement("uuid")
        node.InnerText = Uuid
        pnode.AppendChild(node)

        '' Add all mediaInformation found by INFO
        node = configDoc.CreateElement("infos")
        Dim inode As Xml.XmlNode
        For Each info In Infos
            inode = configDoc.CreateElement(info.Key)
            inode.InnerText = info.Value
            node.AppendChild(inode)
        Next
        pnode.AppendChild(node)

        node = configDoc.CreateElement("thumb")
        node.InnerText = Base64Thumbnail
        pnode.AppendChild(node)

        configDoc.AppendChild(pnode)
        Return configDoc
    End Function

    Public Function toXmlString() Implements ICasparCGMedia.toXmlString
        Return toXml.OuterXml
    End Function

    ' This method is called by the Set accessor of each property. 
    ' The CallerMemberName attribute that is applied to the optional propertyName 
    ' parameter causes the property name of the caller to be substituted as an argument. 
    Private Sub NotifyPropertyChanged(<CallerMemberName> Optional ByVal propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub
#End Region
End Class