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
'' Please let know of any changes and improofments you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

<Serializable()> _
Public Class CasparCGColor
    Inherits CasparCGMedia

    Public Sub New(ByVal name As String)
        MyBase.New(name)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name, xml)
    End Sub

    Public Overrides Function clone() As CasparCGMedia
        Dim media As New CasparCGColor(getFullName)
        For Each info As String In getInfos.Keys
            media.addInfo(info, getInfo(info))
        Next
        Return media
    End Function

    Public Overrides Function getMediaType() As CasparCGMedia.MediaType
        Return MediaType.COLOR
    End Function
End Class