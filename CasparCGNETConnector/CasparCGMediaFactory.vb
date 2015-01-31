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

'' Factory for all CasparCGMedia 
Public Class CasparCGMediaFactory

    Public Shared Function createMedia(ByVal xml As String, Optional fillFromName As Boolean = False, Optional ByRef connection As ICasparCGConnection = Nothing) As AbstractCasparCGMedia
        Dim configDoc As New Xml.XmlDocument
        Dim media As AbstractCasparCGMedia
        configDoc.LoadXml(xml)
        If configDoc.HasChildNodes AndAlso configDoc.FirstChild.Name.Equals("media") Then
            'name und Typ bestimmen:
            Try
                Dim pnode As Xml.XmlNode = configDoc.FirstChild
                Dim name = pnode.SelectSingleNode("name").FirstChild.Value
                Select Case pnode.SelectSingleNode("type").FirstChild.Value
                    Case ICasparCGMedia.MediaTypes.AUDIO
                        media = New CasparCGAudio(name)
                    Case ICasparCGMedia.MediaTypes.COLOR
                        media = New CasparCGColor(name)
                    Case ICasparCGMedia.MediaTypes.MOVIE
                        media = New CasparCGMovie(name)
                    Case ICasparCGMedia.MediaTypes.STILL
                        media = New CasparCGStill(name)
                    Case ICasparCGMedia.MediaTypes.TEMPLATE
                        media = New CasparCGTemplate(name)
                    Case Else
                        Return Nothing
                End Select

                If fillFromName AndAlso Not IsNothing(connection) Then
                    media.fillMediaInfo(connection)
                    media.fillThumbnail(connection)
                ElseIf Not IsNothing(pnode.SelectSingleNode("infos")) Then
                    media.parseXML(pnode.SelectSingleNode("infos").OuterXml)
                    media.Base64Thumbnail = pnode.SelectSingleNode("thumb").InnerText
                ElseIf Not IsNothing(pnode.SelectSingleNode("template")) Then
                    media.parseXML(pnode.SelectSingleNode("template").OuterXml)
                End If

                Return media
            Catch e As Exception
                logger.err("CasparCGMediaFactory.createMedia: Error while parsing a CasparCGMedia from xml definition. Error was: " + e.Message)
            End Try
        End If
        logger.warn("CasparCGMediaFactory.createMedia: Can't read xml. No media created. Reason: No or a wrong xml definition was given.")
        Return Nothing
    End Function
End Class
