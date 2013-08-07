'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'' Author: Christopher Diekkamp
'' Email: christopher@development.diekkamp.de
'' GitHub: https://github.com/mcdikki
'' 
'' This software is licensed under the 
'' GNU General Public License Version 3 (GPLv3).
'' See http://www.gnu.org/licenses/gpl-3.0-standalone.html 
'' for a copy of the license.
''
'' You are free to copy, use and modify this software.
'' Please let me know of any changes and improvements you made to it.
''
'' Thank you!
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

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

    ''' <summary>
    ''' Executes the command using the given connection and returns the <see cref="CasparCGResponse">CasparCGResponse</see>.
    ''' Throws a <see cref="NotSupportedException">NotSupportedException</see> if the command is not compatible to the CasparCG Server version.
    ''' </summary>
    ''' <param name="connection">the CasparCGConnection to execute the command on</param>
    ''' <returns>a CasparCGResponse if, and only if the command is compatible to the connected server version, else throws a NotSupportedException</returns>
    Public Overridable Function execute(ByRef connection As CasparCGConnection) As CasparCGResponse Implements ICommand.execute
        If isCompatible(connection) Then
            response = connection.sendCommand(getCommandString)
            Return getResponse()
        Else
            Throw New NotSupportedException("The command " & getName() & "(min: " & getVersionString(getRequiredVersion()) & " max: " & getVersionString(getMaxAllowedVersion()) & ")is not supported by the server version " & connection.getVersion)
        End If
    End Function

    ''' <summary>
    ''' Returns the <see cref=" CasparCGResponse ">CasparCGResponse</see> of the last command execution or nothing.
    ''' </summary>
    ''' <returns>the <see cref=" CasparCGResponse ">CasparCGResponse</see> or nothing</returns>
    Public Function getResponse() As CasparCGResponse Implements ICommand.getResponse
        Return response
    End Function

    ''' <summary>
    ''' Returns the name of this command
    ''' </summary>
    ''' <returns>The name of this command</returns>
    Public Function getName() As String Implements ICommand.getName
        Return name
    End Function

    ''' <summary>
    ''' Returns the describtion of this command
    ''' </summary>
    ''' <returns>the describtion of this command</returns>
    ''' <remarks></remarks>
    Public Function getDescribtion() As String Implements ICommand.getDescribtion
        Return desc
    End Function

    ''' <summary>
    ''' Returns a list of all <seealso cref=" ICommandParameter ">parameter</seealso> names of this command
    ''' </summary>
    ''' <returns>a list of paramter names</returns>
    Public Function getParameterNames() As List(Of String) Implements ICommand.getParameterNames
        Return pNames
    End Function

    ''' <summary>
    ''' Returns the <seealso cref=" ICommandParameter ">parameter</seealso> ot the given name or nothing if no parameter of this name exists.
    ''' </summary>
    ''' <param name="parameterName">the all lowercase name of the parameter</param>
    ''' <returns>the <seealso cref=" ICommandParameter ">parameter</seealso> if, and only if, it exists, else nothing</returns>
    Public Function getParameter(ByVal parameterName As String) As ICommandParameter Implements ICommand.getParameter
        If pNames.Contains(parameterName.ToLower) Then
            Return parameter.Item(pNames.IndexOf(parameterName.ToLower))
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns whether or not this command is compatible to the CasparCG Server version of the given connection.
    ''' </summary>
    ''' <param name="connection">the <see cref=" CasparCGConnection ">connection</see></param>
    ''' <returns>true, if and only if the commmand is compatible</returns>
    Public Function isCompatible(ByRef connection As CasparCGConnection) As Boolean Implements ICommand.isCompatible
        ' Check if Version is high enough
        Dim reqVersion() = getRequiredVersion()
        For i As Integer = 0 To reqVersion.Length - 1
            If reqVersion(i) >= connection.getVersionPart(i) Then Return False
        Next

        ' Check if version isn't to high
        Dim maxVersion() = getMaxAllowedVersion()
        For i As Integer = 0 To maxVersion.Length - 1
            If maxVersion(i) <= connection.getVersionPart(i) Then Return False
        Next

        ' Check if all parameter which are set are suiteable for the version
        For Each p In parameter
            If p.isSet AndAlso Not p.isCompatible(connection) Then Return False
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
        If Not IsNothing(param) And Not pNames.Contains(param.getName) Then
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
    Public Shared Function escape(ByVal str As String) As String
        ' Backslash
        str = str.Replace("\", "\\")

        ' Hochkommata
        str = str.Replace("'", "\'")
        str = str.Replace("""", "\""")
        Return str
    End Function

    Public Shared Function getVersionString(ByRef version() As Integer) As String
        If version.Length > 0 Then
            Dim v As String = ""
            For Each vp As Integer In version
                v = v & "." & vp
            Next
            Return v.Substring(1)
        Else
            Return ""
        End If
    End Function


    ''
    '' Abstract part
    ''
    Public MustOverride Function getCommandString() As String Implements ICommand.getCommandString
    Public MustOverride Function getRequiredVersion() As Integer() Implements ICommand.getRequiredVersion
    Public MustOverride Function getMaxAllowedVersion() As Integer() Implements ICommand.getMaxAllowedVersion

End Class

Public Interface ICommandParameter
    ''' <summary>
    ''' Returns the name of this parameter
    ''' </summary>
    ''' <returns>The name of this parameter</returns>
    Function getName() As String
    ''' <summary>
    ''' Returns a describtion of this parameter
    ''' </summary>
    ''' <returns>The describtion</returns>
    Function getDesc() As String
    ''' <summary>
    ''' Returns whether or not this parameter is optional
    ''' </summary>
    ''' <returns>Returns whether or not this parameter is optional</returns>
    Function isOptional() As Boolean
    ''' <summary>
    ''' Returns whether or not this parameter has been set to a specific value.
    ''' </summary>
    ''' <returns>Returns whether or not the value has been set</returns>
    Function isSet() As Boolean
    ''' <summary>
    ''' Returns the value type of this paramter. You can use this to generate the right input for the setValue() method
    ''' </summary>
    ''' <returns>System.Type - The System.Type of this parameters value</returns>
    Function getGenericType() As Type
    ''' <summary>
    ''' Returns the type of this parameter instance: <code>CommandParameter(Of t).getType()</code>.
    ''' Use this to Convert the parameter into the right type with:
    ''' <code>CTypeDynamic(paramterObject, parameterObject.getGenericParameterType).setValue(value)</code>
    ''' </summary>
    ''' <returns>the System.Type of this parameter</returns>
    Function getGenericParameterType() As Type

    ''' <summary>
    ''' Returns the required version to run this parameter
    ''' </summary>
    ''' <returns>the version to run this parameter as array of Integer</returns>
    Function getRequiredVersion() As Integer()

    ''' <summary>
    ''' Returns the maximum version to run this parameter
    ''' </summary>
    ''' <returns>the highest version to run this parameter as array of Integer</returns>
    Function getMaxAllowedVersion() As Integer()

    ''' <summary>
    ''' Returns whether or not this parameter is compatible to the CasparCG Server version of the given connection.
    ''' </summary>
    ''' <param name="connection">the <see cref=" CasparCGConnection ">connection</see></param>
    ''' <returns>true, if and only if the parameter is compatible</returns>
    Function isCompatible(ByRef connection As CasparCGConnection) As Boolean
End Interface

Public Class CommandParameter(Of t)
    Implements ICommandParameter
    Private name As String
    Private desc As String
    Private defaultValue As t
    Private value As t
    Private _isOptional As Boolean
    Private _isSet As Boolean
    Private minVersion() As Integer
    Private maxVersion() As Integer

    Public Sub New(ByVal name As String, ByVal describtion As String, defaultvalue As t, isOptionalParameter As Boolean, Optional ByVal minVersion() As Integer = Nothing, Optional ByVal maxVersion() As Integer = Nothing)
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

    ''' <summary>
    ''' Returns the default value of this parameter or nothing
    ''' </summary>
    ''' <returns>the default parameter or nothing</returns>
    Public Function getDefault() As t
        Return defaultValue
    End Function

    Public Function isOptional() As Boolean Implements ICommandParameter.isOptional
        Return _isOptional
    End Function

    ''' <summary>
    ''' Returns the value of this parameter
    ''' </summary>
    ''' <returns>the value of this parameter</returns>
    Public Function getValue() As t
        Return value
    End Function

    ''' <summary>
    ''' Returns the value type of this paramter. You can use this to generate the right input for the setValue() method
    ''' </summary>
    ''' <returns>System.Type - The System.Type of this parameters value</returns>
    Public Function getGenericType() As Type Implements ICommandParameter.getGenericType
        Dim instance As t
        Return instance.GetType
    End Function

    ''' <summary>
    ''' Returns the type of this parameter instance: CommandParameter(Of t).getType().
    ''' Use this to Convert the parameter into the right type with:
    ''' CTypeDynamic(paramterObject, parameterObject.getGenericParameterType).setValue(value)
    ''' </summary>
    ''' <returns>the System.Type of this parameter</returns>
    Public Function getGenericParameterType() As Type Implements ICommandParameter.getGenericParameterType
        Return Me.GetType
    End Function

    ''' <summary>
    ''' Sets the value of this parameter
    ''' </summary>
    ''' <param name="value">the value to set this parameter to</param>
    Public Sub setValue(ByRef value As t)
        _isSet = True
        Me.value = value
    End Sub

    Public Function isSet() As Boolean Implements ICommandParameter.isSet
        Return _isSet
    End Function

    ''' <summary>
    ''' Returns the required version to run this parameter
    ''' </summary>
    ''' <returns>the version to run this parameter as array of Integer</returns>
    Function getRequiredVersion() As Integer() Implements ICommandParameter.getRequiredVersion
        If Not IsNothing(minVersion) Then
            Return minVersion
        Else
            Return {1}
        End If
    End Function

    ''' <summary>
    ''' Returns the maximum version to run this parameter
    ''' </summary>
    ''' <returns>the highest version to run this parameter as array of Integer</returns>
    Function getMaxAllowedVersion() As Integer() Implements ICommandParameter.getMaxAllowedVersion
        If Not IsNothing(minVersion) Then
            Return maxVersion
        Else
            Return {Integer.MaxValue}
        End If
    End Function

    ''' <summary>
    ''' Returns whether or not this parameter is compatible to the CasparCG Server version of the given connection.
    ''' </summary>
    ''' <param name="connection">the <see cref=" CasparCGConnection ">connection</see></param>
    ''' <returns>true, if and only if the parameter is compatible</returns>
    Public Function isCompatible(ByRef connection As CasparCGConnection) As Boolean Implements ICommandParameter.isCompatible
        Dim reqVersion() = getRequiredVersion()
        For i As Integer = 0 To reqVersion.Length - 1
            If reqVersion(i) >= connection.getVersionPart(i) Then Return False
        Next

        Dim maxVersion() = getMaxAllowedVersion()
        For i As Integer = 0 To maxVersion.Length - 1
            If maxVersion(i) <= connection.getVersionPart(i) Then Return False
        Next
        Return True
    End Function

End Class