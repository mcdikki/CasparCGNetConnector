Public Interface ICommand

    Function execute(ByRef connection As CasparCGConnection) As CasparCGResponse
    Function getResponse() As CasparCGResponse
    Function getName() As String
    Function getDescribtion() As String
    Function getParameterNames() As List(Of String)
    Function getParameter(ByVal parameterName As String) As ICommandParameter
    Function isCompatible(ByRef connection As CasparCGConnection) As Boolean
    Function getCommandString() As String
    Function getRequiredVersion() As Integer()
    Function getMaxAllowedVersion() As Integer()

End Interface

