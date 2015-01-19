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
        setKey(key)
        setData(data)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of String)("key", "The key", "", False))
        addCommandParameter(New CommandParameter(Of String)("data", "The data string to store", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        checkParameter()
        Return escape("DATA STORE '" & getKey() & "' '" & getData() & "'")
    End Function

    Public Sub setKey(ByVal key As String)
        If IsNothing(key) Then
            DirectCast(getCommandParameter("key"), CommandParameter(Of String)).setValue("")
        Else
            DirectCast(getCommandParameter("key"), CommandParameter(Of String)).setValue(key)
        End If
    End Sub

    Public Function getKey() As String
        Dim param As CommandParameter(Of String) = getCommandParameter("key")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setData(ByVal data As String)
        If IsNothing(data) Then
            DirectCast(getCommandParameter("data"), CommandParameter(Of String)).setValue("")
        Else
            DirectCast(getCommandParameter("data"), CommandParameter(Of String)).setValue(data)
        End If
    End Sub

    Public Function getData() As String
        Dim param As CommandParameter(Of String) = getCommandParameter("data")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
