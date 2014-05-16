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

Public Interface ICasparCGMedia
    Inherits ComponentModel.INotifyPropertyChanged

#Region "Properties"
    Property Name As String
    Property Path As String
    ReadOnly Property FullName As String
    Property Uuid As String
    Property Infos As Dictionary(Of String, String)
    Property Base64Thumbnail As String
    ReadOnly Property MediaType As MediaTypes

#End Region

#Region "Events"
    ''' <summary>
    ''' Notifies that the medias metadata (infos) has been filled.
    ''' </summary>
    ''' <param name="sender"></param>
    Event MediaFilled(ByRef sender As ICasparCGMedia)
    ''' <summary>
    ''' Notifies that the thumbnail of the media has been filled.
    ''' </summary>
    ''' <param name="sender"></param>
    Event ThumbnailFilled(ByRef sender As ICasparCGMedia)
    ''' <summary>
    ''' Notifies that an info of this media has been changed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="info"></param>
    Event InfoChanged(ByRef sender As ICasparCGMedia, ByVal info As KeyValuePair(Of String, String))
#End Region

#Region "Enums"
    Enum MediaTypes
        STILL = 0
        MOVIE = 1
        AUDIO = 2
        TEMPLATE = 3
        COLOR = 4
    End Enum
#End Region

#Region "Methods"
    ''' <summary>
    ''' Fills the metadata informations given by the casparCG servers info command. The media will be loaded to background on the given channel and the first free layer. After polling the the INFO, the layer will be cleared again. It is recommend to use a non production channel for this
    ''' </summary>
    ''' <param name="connection">The connection on which the info should be requested</param>
    ''' <param name="channel">The channel to load the media on</param>
    ''' <remarks></remarks>
    Sub fillMediaInfo(ByRef connection As CasparCGConnection, Optional ByVal channel As Integer = 1)
    ''' <summary>
    ''' Retrieves a thumbnail (if present and support) using the given serverconnection.
    ''' </summary>
    ''' <param name="connection">the connection to use</param>
    Sub fillThumbnail(ByRef connection As CasparCGConnection)
    Sub parseXML(ByVal xml As String)
    Sub setInfo(ByVal info As String, ByVal value As String)
    Sub addInfo(ByVal info As String, ByVal value As String)
#End Region

#Region "Functions"
    Function clone() As ICasparCGMedia
    Function containsInfo(ByVal info As String) As Boolean
    Function getInfo(ByVal info As String) As String
    Function toString() As String
    Function toXml() As Xml.XmlDocument
    Function toXmlString()
#End Region
End Interface
