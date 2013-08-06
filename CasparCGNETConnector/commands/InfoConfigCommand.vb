Public Class InfoConfigCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO CONFIG", "Requests the configuration of the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "INFO CONFIG"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
