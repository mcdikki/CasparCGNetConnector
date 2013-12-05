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

Public Class DataStoreCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("DATA STORE", "Stores the given data string by the given key")
        InitParameter()
    End Sub

    Public Sub New(ByVal key As String, ByVal data As String)
        MyBase.New("DATA STORE", "Stores the given data string by the given key")
        InitParameter()
        DirectCast(getCommandParameter("key"), CommandParameter(Of String)).setValue(key)
        DirectCast(getCommandParameter("data"), CommandParameter(Of String)).setValue(data)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of String)("key", "The key", "", False))
        addCommandParameter(New CommandParameter(Of String)("data", "The data string to store", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Return escape("DATA STORE '" & DirectCast(getCommandParameter("key"), CommandParameter(Of String)).getValue() & "' '" & DirectCast(getCommandParameter("data"), CommandParameter(Of String)).getValue() & "'")
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
