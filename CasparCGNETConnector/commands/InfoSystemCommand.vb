Public Class InfoSystemCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO SYSTEM", "Requests system information of the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "INFO SYSTEM"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
