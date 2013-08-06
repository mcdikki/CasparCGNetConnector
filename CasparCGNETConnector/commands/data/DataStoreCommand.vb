Public Class DataStoreCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("DATA STORE", "Stores the given data string by the given key")
        InitParameter()
    End Sub

    Public Sub New(ByVal key As String, ByVal data As String)
        MyBase.New("DATA STORE", "Stores the given data string by the given key")
        InitParameter()
        DirectCast(getParameter("key"), CommandParameter(Of String)).setValue(key)
        DirectCast(getParameter("data"), CommandParameter(Of String)).setValue(data)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("key", "The key", "", False))
        addParameter(New CommandParameter(Of String)("data", "The data string to store", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Return "DATA STORE " & DirectCast(getParameter("key"), CommandParameter(Of String)).getValue() & " " & DirectCast(getParameter("data"), CommandParameter(Of String)).getValue()
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
