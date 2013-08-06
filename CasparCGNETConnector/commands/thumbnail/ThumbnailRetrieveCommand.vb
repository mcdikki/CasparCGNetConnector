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

Public Class ThumbnailRetrieveCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("THUMBNAIL RETRIEVE", "Requests the base64 encoded thumbnail for a specific media file")
    End Sub

    Public Sub New(ByVal media As String)
        MyBase.New("THUMBNAIL RETRIEVE", "Requests the base64 encoded thumbnail for a specific media file")
        InitParameter()
        DirectCast(getParameter("media"), CommandParameter(Of String)).setValue(media)
    End Sub

    Public Sub New(ByVal media As CasparCGMedia)
        MyBase.New("THUMBNAIL RETRIEVE", "Requests the base64 encoded thumbnail for a specific media file")
        InitParameter()
        DirectCast(getParameter("media"), CommandParameter(Of String)).setValue(media.getFullName)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("media", "The media file", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd = "THUMBNAIL RETRIEVE " & DirectCast(getParameter("media"), CommandParameter(Of String)).getValue
        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
