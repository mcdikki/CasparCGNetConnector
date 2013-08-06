Public Class RemoveCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("REMOVE", "Removes a consumer from a given channel")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal parameter() As String)
        MyBase.New("REMOVE", "Removes a comsumer from a given channel")
        InitParameter()
        DirectCast(getParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        DirectCast(getParameter("paramter"), CommandParameter(Of String())).setValue(parameter)
    End Sub

    Private Sub InitParameter()
        addParameter(New CommandParameter(Of String)("channel", "The channel", 1, False))
        addParameter(New CommandParameter(Of String())("parameter", "The paramter list", {}, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "REMOVE " & getDestination(getParameter("channel"))
        If getParameter("parameter").isSet AndAlso DirectCast(getParameter("parameter"), CommandParameter(Of String())).getValue.Length > 0 Then
            For Each p In DirectCast(getParameter("parameter"), CommandParameter(Of String())).getValue
                cmd = cmd & " " & p
            Next
        Else
            Throw New ArgumentNullException("The REMOVE command needs at least one parameter to be defined. Empty parameter lists are not allowed.")
        End If
        Return cmd
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {1}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
