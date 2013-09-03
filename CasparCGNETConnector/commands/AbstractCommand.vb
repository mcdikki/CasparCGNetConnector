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


    Public Sub New(ByVal name As String, ByVal Description As String)
        Me.name = name
        Me.desc = Description
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
        If connection.strictVersionControl OrElse isCompatible(connection) Then
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
    ''' Returns the Description of this command
    ''' </summary>
    ''' <returns>the Description of this command</returns>
    ''' <remarks></remarks>
    Public Function getDescription() As String Implements ICommand.getDescription
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
        Dim i As Integer = 0
        logger.debug("Check reqVersion of command " & getName())
        While i < reqVersion.Length
            If reqVersion(i) < connection.getVersionPart(i) Then
                logger.debug("Need: " & reqVersion(i) & " <= " & connection.getVersionPart(i) & " is '<' --> OK!")
                Exit While
            ElseIf reqVersion(i) > connection.getVersionPart(i) Then
                logger.debug("Need: " & reqVersion(i) & " <= " & connection.getVersionPart(i) & " is '>' --> FAILED!")
                Return False
            End If
            logger.debug("Need: " & reqVersion(i) & " <= " & connection.getVersionPart(i) & " is '=' --> OK!")
            i = i + 1
        End While

        ' Check if version isn't to high
        Dim maxVersion() = getMaxAllowedVersion()
        i = 0
        logger.debug("Check maxAllowedVersion")
        While i < maxVersion.Length OrElse connection.getVersionPart(i) > -1
            If i < maxVersion.Length Then
                If maxVersion(i) < connection.getVersionPart(i) Then
                    logger.debug("Need: " & maxVersion(i) & " >= " & connection.getVersionPart(i) & " is '<' --> FAILED!")
                    Return False
                ElseIf maxVersion(i) > connection.getVersionPart(i) Then
                    logger.debug("Need: " & maxVersion(i) & " >= " & connection.getVersionPart(i) & " is '>' --> OK!")
                    Exit While
                Else
                    logger.debug("Need: " & maxVersion(i) & " >= " & connection.getVersionPart(i) & " is '=' --> OK!")
                End If
            ElseIf connection.getVersionPart(i) > 0 Then
                logger.debug("Need: 0 >= " & connection.getVersionPart(i) & " is '<' --> FAILED!")
                Return False
            End If
            i = i + 1
        End While

        ' Check if all parameter which are set are suiteable for the version
        logger.debug("Checking command parameter")
        For Each p In parameter
            If p.isSet AndAlso Not p.isCompatible(connection) Then Return False
        Next

        logger.debug("Command " & getName() & " is compatible with CasparCG " & connection.getVersion)
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

    Public Function getParamters() As List(Of ICommandParameter) Implements ICommand.getParameters
        Return parameter
    End Function

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
        'str = str.Replace("'", "\'")
        str = str.Replace("""", "\""")
        str = str.Replace("'", """")
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

    Public Function toXml() As MSXML2.DOMDocument Implements ICommand.toXml
        Dim configDoc As New MSXML2.DOMDocument
        Dim pnode As MSXML2.IXMLDOMNode
        Dim node As MSXML2.IXMLDOMNode
        ' Kopfdaten eintragen
        pnode = configDoc.createElement("command")
        node = configDoc.createElement("name")
        node.nodeTypedValue = getName()
        pnode.appendChild(node)
        node = configDoc.createElement("type")
        node.nodeTypedValue = Me.GetType.ToString
        pnode.appendChild(node)
        node = configDoc.createElement("description")
        node.nodeTypedValue = getDescription()
        pnode.appendChild(node)
        'node = configDoc.createElement("reqVersion")
        'node.nodeTypedValue = getRequiredVersion()
        'pnode.appendChild(node)
        'node = configDoc.createElement("maxVersion")
        'node.nodeTypedValue = getMaxAllowedVersion()
        'pnode.appendChild(node)

        '' Add all parameter
        For Each param In getParamters()
            pnode.appendChild(param.toXml().firstChild)
        Next
        configDoc.appendChild(pnode)
        Return configDoc
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
    ''' Returns a Description of this parameter
    ''' </summary>
    ''' <returns>The Description</returns>
    Function getDescription() As String
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

    Function toXml() As MSXML2.DOMDocument
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

    Public Sub New(ByVal name As String, ByVal Description As String, defaultvalue As t, isOptionalParameter As Boolean, Optional ByVal minVersion() As Integer = Nothing, Optional ByVal maxVersion() As Integer = Nothing)
        Me.name = name
        Me.desc = Description
        Me.defaultValue = defaultvalue
        Me._isOptional = isOptionalParameter
    End Sub

    Public Function getName() As String Implements ICommandParameter.getName
        Return name
    End Function

    Public Function getDescription() As String Implements ICommandParameter.getDescription
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
        Return GetType(t)
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
        ' Check if Version is high enough
        Dim reqVersion() = getRequiredVersion()
        Dim i As Integer = 0
        logger.debug("Check reqVersion of parameter " & getName())
        While i < reqVersion.Length
            If reqVersion(i) < connection.getVersionPart(i) Then
                logger.debug("Need: " & reqVersion(i) & " <= " & connection.getVersionPart(i) & " is '<' --> OK!")
                Exit While
            ElseIf reqVersion(i) > connection.getVersionPart(i) Then
                logger.debug("Need: " & reqVersion(i) & " <= " & connection.getVersionPart(i) & " is '>' --> FAILED!")
                Return False
            End If
            logger.debug("Need: " & reqVersion(i) & " <= " & connection.getVersionPart(i) & " is '=' --> OK!")
            i = i + 1
        End While

        ' Check if version isn't to high
        Dim maxVersion() = getMaxAllowedVersion()
        i = 0
        logger.debug("Check maxAllowedVersion")
        While i < maxVersion.Length OrElse connection.getVersionPart(i) > -1
            If i < maxVersion.Length Then
                If maxVersion(i) < connection.getVersionPart(i) Then
                    logger.debug("Need: " & maxVersion(i) & " >= " & connection.getVersionPart(i) & " is '<' --> FAILED!")
                    Return False
                ElseIf maxVersion(i) > connection.getVersionPart(i) Then
                    logger.debug("Need: " & maxVersion(i) & " >= " & connection.getVersionPart(i) & " is '>' --> OK!")
                    Exit While
                Else
                    logger.debug("Need: " & maxVersion(i) & " >= " & connection.getVersionPart(i) & " is '=' --> OK!")
                End If
            ElseIf connection.getVersionPart(i) > 0 Then
                logger.debug("Need: 0 >= " & connection.getVersionPart(i) & " is '<' --> FAILED!")
                Return False
            End If
            i = i + 1
        End While
        Return True
    End Function

    Public Function toXml() As MSXML2.DOMDocument Implements ICommandParameter.toXml
        Dim configDoc As New MSXML2.DOMDocument
        Dim pnode As MSXML2.IXMLDOMNode
        Dim node As MSXML2.IXMLDOMNode
        ' Kopfdaten eintragen
        pnode = configDoc.createElement("parameter")
        node = configDoc.createElement("name")
        node.nodeTypedValue = getName()
        pnode.appendChild(node)
        node = configDoc.createElement("parameterType")
        node.nodeTypedValue = getGenericParameterType().ToString
        pnode.appendChild(node)
        node = configDoc.createElement("valueType")
        node.nodeTypedValue = getGenericType().ToString
        pnode.appendChild(node)
        node = configDoc.createElement("description")
        node.nodeTypedValue = getDescription()
        pnode.appendChild(node)
        node = configDoc.createElement("optional")
        node.nodeTypedValue = isOptional()
        pnode.appendChild(node)
        'node = configDoc.createElement("reqVersion")
        'node.nodeTypedValue = getRequiredVersion()
        'pnode.appendChild(node)
        'node = configDoc.createElement("maxVersion")
        'node.nodeTypedValue = getMaxAllowedVersion()
        'pnode.appendChild(node)
        If isSet() Then
            node = configDoc.createElement("value")
            node.nodeTypedValue = getValue()
            pnode.appendChild(node)
        End If
        configDoc.appendChild(pnode)
        Return configDoc
    End Function

End Class