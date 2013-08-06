Public MustInherit Class AbstractCommand
    Implements ICommand

    Private name As String
    Private desc As String
    Private response As CasparCGResponse
    Private parameter As List(Of ICommandParameter)
    Private pNames As List(Of String)


    Public Sub New(ByVal name As String, ByVal describtion As String)
        Me.name = name
        Me.desc = describtion
        pNames = New List(Of String)
        parameter = New List(Of ICommandParameter)
    End Sub

    Public Overridable Function execute(ByRef connection As CasparCGConnection) As CasparCGResponse Implements ICommand.execute
        If isCompatible(connection) Then
            response = connection.sendCommand(getCommandString)
            Return getResponse()
        Else
            Throw New NotSupportedException("The command " & getName() & " is not supported by the server version " & connection.getVersion)
        End If
    End Function

    Public Function getResponse() As CasparCGResponse Implements ICommand.getResponse
        Return response
    End Function

    Public Function getName() As String Implements ICommand.getName
        Return name
    End Function

    Public Function getDescribtion() As String Implements ICommand.getDescribtion
        Return desc
    End Function

    Public Function getParameterNames() As List(Of String) Implements ICommand.getParameterNames
        Return pNames
    End Function

    Public Function getParameter(ByVal parameterName As String) As ICommandParameter Implements ICommand.getParameter
        If pNames.Contains(parameterName.ToLower) Then
            Return parameter.Item(pNames.IndexOf(parameterName.ToLower))
        End If
        Return Nothing
    End Function

    Public Function isCompatible(ByRef connection As CasparCGConnection) As Boolean Implements ICommand.isCompatible
        Dim reqVersion() = getRequiredVersion()
        For i As Integer = 0 To reqVersion.Length - 1
            If reqVersion(i) > connection.getVersionPart(i) Then Return False
        Next

        Dim maxVersion() = getMaxAllowedVersion()
        For i As Integer = 0 To maxVersion.Length - 1
            If maxVersion(i) < connection.getVersionPart(i) Then Return False
        Next
        Return True
    End Function

    Protected Sub setParameters(ByRef params As List(Of ICommandParameter))
        If Not IsNothing(params) Then
            parameter = params
            pNames.Clear()
            For Each p In parameter
                pNames.Add(p.getName)
            Next
        End If
    End Sub

    Protected Sub addParameter(ByRef param As ICommandParameter)
        If Not IsNothing(param) Then
            parameter.Add(param)
            pNames.Add(param.getName)
        End If
    End Sub


    Protected Shared Function getDestination(ByRef channel As CommandParameter(Of Integer), Optional ByRef layer As CommandParameter(Of Integer) = Nothing) As String
        Dim cmd As String
        If channel.isSet() Then
            cmd = channel.getValue
        Else
            cmd = "1"
        End If
        If Not IsNothing(layer) AndAlso layer.isSet Then
            cmd = cmd & "-" & layer.getValue
        End If
        Return cmd
    End Function


    ''' <summary>
    ''' Escapes the string str as needed for casparCG Server
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function escape(ByVal str As String) As String
        ' Backslash
        str = str.Replace("\", "\\")

        ' Hochkommata
        str = str.Replace("'", "\'")
        str = str.Replace("""", "\""")
        Return str
    End Function


    ''
    '' Abstract part
    ''
    Public MustOverride Function getCommandString() As String Implements ICommand.getCommandString
    Public MustOverride Function getRequiredVersion() As Integer() Implements ICommand.getRequiredVersion
    Public MustOverride Function getMaxAllowedVersion() As Integer() Implements ICommand.getMaxAllowedVersion

End Class

Public Interface ICommandParameter
    Function getName() As String
    Function getDesc() As String
    Function isOptional() As Boolean
    Function isSet() As Boolean
    Function getGenericType() As Type
End Interface

Public Class CommandParameter(Of t)
    Implements ICommandParameter
    Private name As String
    Private desc As String
    Private defaultValue As t
    Private value As t
    Private _isOptional As Boolean
    Private _isSet As Boolean

    Public Sub New(ByVal name As String, ByVal describtion As String, defaultvalue As t, isOptionalParameter As Boolean)
        Me.name = name
        Me.desc = describtion
        Me.defaultValue = defaultvalue
        Me._isOptional = isOptionalParameter
    End Sub

    Public Function getName() As String Implements ICommandParameter.getName
        Return name
    End Function

    Public Function getDesc() As String Implements ICommandParameter.getDesc
        Return desc
    End Function

    Public Function getDefault() As t
        Return defaultValue
    End Function

    Public Function isOptional() As Boolean Implements ICommandParameter.isOptional
        Return _isOptional
    End Function

    Public Function getValue() As t
        Return value
    End Function

    Public Function getGenericType() As Type Implements ICommandParameter.getGenericType
        Return value.GetType
    End Function

    Public Sub setValue(ByRef value As t)
        _isSet = True
        Me.value = value
    End Sub

    Public Function isSet() As Boolean Implements ICommandParameter.isSet
        Return _isSet
    End Function

End Class