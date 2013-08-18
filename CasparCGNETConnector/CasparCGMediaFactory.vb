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

    Public Shared Function createMedia(ByVal xml As String, Optional fillFromName As Boolean = False, Optional ByRef connection As CasparCGConnection = Nothing) As AbstractCasparCGMedia
        Dim configDoc As New MSXML2.DOMDocument
        Dim media As AbstractCasparCGMedia
        If configDoc.loadXML(xml) AndAlso configDoc.hasChildNodes AndAlso configDoc.firstChild.nodeName.Equals("media") Then
            'name und Typ bestimmen:
            Try
                Dim pnode As MSXML2.IXMLDOMNode = configDoc.firstChild
                Dim name = pnode.selectSingleNode("name").nodeTypedValue
                Select Case pnode.selectSingleNode("type").nodeTypedValue
                    Case AbstractCasparCGMedia.MediaType.AUDIO
                        media = New CasparCGAudio(name)
                    Case AbstractCasparCGMedia.MediaType.COLOR
                        media = New CasparCGColor(name)
                    Case AbstractCasparCGMedia.MediaType.MOVIE
                        media = New CasparCGMovie(name)
                    Case AbstractCasparCGMedia.MediaType.STILL
                        media = New CasparCGStill(name)
                    Case AbstractCasparCGMedia.MediaType.TEMPLATE
                        media = New CasparCGTemplate(name)
                    Case Else
                        Return Nothing
                End Select

                If fillFromName AndAlso Not IsNothing(connection) Then
                    media.fillMediaInfo(connection)
                    Dim cmd As New ThumbnailRetrieveCommand(media)
                    If cmd.execute(connection).isOK Then media.setBase64Thumb(cmd.getResponse.getXMLData)
                ElseIf Not IsNothing(pnode.selectSingleNode("infos")) Then
                    media.parseXML(pnode.selectSingleNode("infos").xml)
                    media.setBase64Thumb(pnode.selectSingleNode("thumb").nodeTypedValue)
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
