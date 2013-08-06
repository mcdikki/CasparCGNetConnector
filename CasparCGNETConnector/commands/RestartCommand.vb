Public Class RestartCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("RESTART", "Stops the server with exitcode 5")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "RESTART"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
