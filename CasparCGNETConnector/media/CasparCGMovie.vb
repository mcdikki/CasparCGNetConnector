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

    Public Overrides Function clone() As ICasparCGMedia
        Dim media As New CasparCGMovie(FullName)
        media.Base64Thumbnail = Base64Thumbnail
        For Each info In Infos
            media.addInfo(info.Key, info.Value)
        Next
        Return media
    End Function

    Public Overrides ReadOnly Property MediaType As ICasparCGMedia.MediaTypes
        Get
            Return ICasparCGMedia.MediaTypes.MOVIE
        End Get
    End Property

End Class