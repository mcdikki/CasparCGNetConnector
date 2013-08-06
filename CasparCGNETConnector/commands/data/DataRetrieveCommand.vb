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

Public Class DataRetrieveCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("DATA RETRIEVE", "Retrieves the data string stored by the given key")
        InitParameter()
    End Sub

    Public Sub New(ByVal key As String)
        MyBase.New("DATA RETRIEVE", "Retrieves the data string stored by the given key")
        InitParameter()
        DirectCast(getParameter("key"), CommandParameter(Of String)).setValue(key)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("key", "The key", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Return "DATA RETRIEVE " & DirectCast(getParameter("key"), CommandParameter(Of String)).getValue()
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
