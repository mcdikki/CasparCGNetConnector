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
Module CasparCGMediaFactory

    Public Function createMedia(ByVal xml As String) As CasparCGMedia
        Dim configDoc As New MSXML2.DOMDocument
        Dim media As CasparCGMedia
        If configDoc.loadXML(xml) AndAlso configDoc.hasChildNodes AndAlso configDoc.firstChild.nodeName.Equals("media") Then
            'name und Typ bestimmen:
            Dim pnode As MSXML2.IXMLDOMNode = configDoc.firstChild
            Dim name = pnode.selectSingleNode("name").nodeTypedValue
            Select Case pnode.selectSingleNode("type").nodeTypedValue
                Case CasparCGMedia.MediaType.AUDIO
                    media = New CasparCGAudio(name, pnode.selectSingleNode("infos").xml)
                Case CasparCGMedia.MediaType.COLOR
                    media = New CasparCGColor(name, pnode.selectSingleNode("infos").xml)
                Case CasparCGMedia.MediaType.MOVIE
                    media = New CasparCGMovie(name, pnode.selectSingleNode("infos").xml)
                    media.setBase64Thumb(pnode.selectSingleNode("thumb").nodeTypedValue)
                Case CasparCGMedia.MediaType.STILL
                    media = New CasparCGStill(name, pnode.selectSingleNode("infos").xml)
                    media.setBase64Thumb(pnode.selectSingleNode("thumb").nodeTypedValue)
                Case CasparCGMedia.MediaType.TEMPLATE
                    media = New CasparCGTemplate(name, pnode.selectSingleNode("infos").xml)
                Case Else
                    Return Nothing
            End Select

            Return media
        Else
            logger.warn("CasparCGMediaFactory.createMedia: Can't read xml. No media created. Reason: No or a wrong xml definition was given.")
            Return Nothing
        End If

    End Function
End Module
