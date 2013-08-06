Public Class InfoPathsCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("INFO PATHS", "Requests the path configuration of the server")
    End Sub

    Public Overrides Function getCommandString() As String
        Return "INFO PATHS"
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
