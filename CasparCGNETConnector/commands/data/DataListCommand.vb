Public Class DataListCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("DATA LIST", "Lists all stored data on the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "DATA LIST"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
