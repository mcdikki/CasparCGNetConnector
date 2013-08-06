Public Class InfoServerCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO SERVER", "Requests informations about the connected server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "INFO SERVER"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
