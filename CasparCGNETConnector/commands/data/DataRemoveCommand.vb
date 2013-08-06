Public Class DataRemoveCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("DATA REMOVE", "Removes the data string stored by the given key")
        InitParameter()
    End Sub

    Public Sub New(ByVal key As String)
        MyBase.New("DATA REMOVE", "Removes the data string stored by the given key")
        InitParameter()
        DirectCast(getParameter("key"), CommandParameter(Of String)).setValue(key)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("key", "The key", "", False))
    End Sub

    Public Overrides Function getCommandString() As String
        Return "DATA REMOVE " & DirectCast(getParameter("key"), CommandParameter(Of String)).getValue()
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
