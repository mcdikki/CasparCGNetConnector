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

<Serializable()> _
Public Class CasparCGMovie
    Inherits AbstractCasparCGMedia

    Public Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name, xml)
    End Sub

    Public Overrides Function clone() As AbstractCasparCGMedia
        Dim media As New CasparCGMovie(getFullName)
        For Each info As String In getInfos.Keys
            media.addInfo(info, getInfo(info))
        Next
        media.setBase64Thumb(getBase64Thumb())
        Return media
    End Function

    Public Overrides Function getMediaType() As AbstractCasparCGMedia.MediaType
        Return MediaType.MOVIE
    End Function

End Class