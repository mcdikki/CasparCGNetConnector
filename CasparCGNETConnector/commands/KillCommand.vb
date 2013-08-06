Public Class KillCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("KILL", "Stops the server with exitcode 0")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "KILL"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 4}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
