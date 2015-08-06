﻿'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
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
    Public Overridable Function execute(ByRef connection As ICasparCGConnection) As CasparCGResponse
        If Not IsNothing(connection) AndAlso connection.isConnected Then
            If Not connection.strictVersionControl OrElse isCompatible(connection) Then
                response = connection.sendCommand(getCommandString)
                Return getResponse()
            Else
                logger.err("The command " & getName() & "(min: " & getVersionString(getRequiredVersion()) & " max: " & getVersionString(getMaxAllowedVersion()) & ")is not supported by the server version " & connection.getVersion)
                Throw New NotSupportedException("The command " & getName() & "(min: " & getVersionString(getRequiredVersion()) & " max: " & getVersionString(getMaxAllowedVersion()) & ")is not supported by the server version " & connection.getVersion)
            End If
        Else
            Return New CasparCGResponse("000 NOT_CONNECTED_ERROR", getCommandString)
        End If
    End Function

    ''' <summary>
    ''' Returns the <see cref=" CasparCGResponse ">CasparCGResponse</see> of the last command execution or nothing.
    ''' </summary>
    ''' <returns>the <see cref=" CasparCGResponse ">CasparCGResponse</see> or nothing</returns>
    Public Function getResponse() As CasparCGResponse
        Return response
    End Function

    ''' <summary>
    ''' Returns the name of this command
    ''' </summary>
    ''' <returns>The name of this command</returns>
    Public Function getName() As String
        Return name
    End Function

    ''' <summary>
    ''' Returns the Description of this command
    ''' </summary>
    ''' <returns>the Description of this command</returns>
    ''' <remarks></remarks>
    Public Function getDescription() As String
        Return desc
    End Function

    ''' <summary>
    ''' Returns a list of all <seealso cref=" ICommandParameter ">parameter</seealso> names of this command
    ''' </summary>
    ''' <returns>a list of paramter names</returns>
    Public Function getCommandParameterNames() As List(Of String)
        Return pNames
    End Function

    ''' <summary>
    ''' Returns the <seealso cref=" ICommandParameter ">parameter</seealso> ot the given name or nothing if no parameter of this name exists.
    ''' </summary>
    ''' <param name="parameterName">the all lowercase name of the parameter</param>
    ''' <returns>the <seealso cref=" ICommandParameter ">parameter</seealso> if, and only if, it exists, else nothing</returns>
    Public Function getCommandParameter(ByVal parameterName As String) As ICommandParameter
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
    Public Function isCompatible(ByRef connection As ICasparCGConnection) As Boolean
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

    Protected Sub setCommandParameters(ByRef params As List(Of ICommandParameter))
        If Not IsNothing(params) Then
            parameter = params
            pNames.Clear()
            For Each p In parameter
                pNames.Add(p.getName)
            Next
        End If
    End Sub

    Public Function getCommandParameters() As List(Of ICommandParameter)
        Return parameter
    End Function

    Protected Sub addCommandParameter(ByRef param As ICommandParameter)
        If Not IsNothing(param) And Not pNames.Contains(param.getName) Then
            parameter.Add(param)
            pNames.Add(param.getName.ToLower)
        End If
    End Sub

    Protected Sub checkParameter()
        For Each p In getCommandParameters()
            If Not p.isOptional AndAlso Not p.isSet Then Throw New ArgumentNullException(p.getName, p.getName & " is mandatory but not set.")
        Next
    End Sub

    Public Function getDestination() As String
        Dim dst As String = ""
        If getCommandParameterNames.Contains("channel") Then
            If getCommandParameter("channel").isSet Then
                dst = DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).getValue
            Else
                dst = "1"
            End If
            If getCommandParameterNames.Contains("layer") AndAlso getCommandParameter("layer").isSet Then
                dst = dst & "-" & DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).getValue
            End If
        End If
        Return dst
    End Function

    Public Shared Function getDestination(ByRef channel As CommandParameter(Of Integer), Optional ByRef layer As CommandParameter(Of Integer) = Nothing) As String
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

    Public Function toXml() As Xml.XmlDocument
        Dim configDoc As New Xml.XmlDocument
        Dim pnode As Xml.XmlNode
        Dim node As Xml.XmlNode
        ' Kopfdaten eintragen
        pnode = configDoc.createElement("command")
        node = configDoc.createElement("name")
        node.InnerText = getName()
        pnode.appendChild(node)
        node = configDoc.createElement("type")
        node.InnerText = Me.GetType.ToString
        pnode.appendChild(node)
        node = configDoc.createElement("description")
        node.InnerText = getDescription()
        pnode.appendChild(node)
        'node = configDoc.createElement("reqVersion")
        'node.InnerText = getRequiredVersion()
        'pnode.appendChild(node)
        'node = configDoc.createElement("maxVersion")
        'node.InnerText = getMaxAllowedVersion()
        'pnode.appendChild(node)

        '' Add all parameter
        For Each param In getCommandParameters()
            pnode.AppendChild(configDoc.ImportNode(param.toXml().FirstChild, True))
        Next
        configDoc.appendChild(pnode)
        Return configDoc
    End Function


    ''
    '' Abstract part
    ''
    ''' <summary>
    ''' Returns the command as string
    ''' </summary>
    ''' <returns>the command as string</returns>
    Public MustOverride Function getCommandString() As String
    ''' <summary>
    ''' Returns the required version to run this command
    ''' </summary>
    ''' <returns>the version to run this command as array of Integer</returns>
    Public MustOverride Function getRequiredVersion() As Integer()
    ''' <summary>
    ''' Returns the maximum version to run this command
    ''' </summary>
    ''' <returns>the highest version to run this command as array of Integer</returns>
    Public MustOverride Function getMaxAllowedVersion() As Integer()

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
    Function isCompatible(ByRef connection As ICasparCGConnection) As Boolean

    Function toXml() As Xml.XmlDocument

    Sub unset()
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
    Public Overridable Sub setValue(ByRef value As t)
        _isSet = True
        Me.value = value
    End Sub

    Public Function isSet() As Boolean Implements ICommandParameter.isSet
        Return _isSet
    End Function

    Public Sub unset() Implements ICommandParameter.unset
        _isSet = False
    End Sub

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
    Public Function isCompatible(ByRef connection As ICasparCGConnection) As Boolean Implements ICommandParameter.isCompatible
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

    Public Function toXml() As Xml.XmlDocument Implements ICommandParameter.toXml
        Dim configDoc As New Xml.XmlDocument
        Dim pnode As Xml.XmlNode
        Dim node As Xml.XmlNode
        ' Kopfdaten eintragen
        pnode = configDoc.CreateElement("parameter")
        node = configDoc.CreateElement("name")
        node.InnerText = getName()
        pnode.AppendChild(node)
        node = configDoc.CreateElement("parameterType")
        node.InnerText = getGenericParameterType().ToString
        pnode.AppendChild(node)
        node = configDoc.CreateElement("valueType")
        node.InnerText = getGenericType().ToString
        pnode.AppendChild(node)
        node = configDoc.CreateElement("description")
        node.InnerText = getDescription()
        pnode.AppendChild(node)
        node = configDoc.CreateElement("optional")
        node.InnerText = isOptional()
        pnode.AppendChild(node)
        'node = configDoc.createElement("reqVersion")
        'node.InnerText = getRequiredVersion()
        'pnode.appendChild(node)
        'node = configDoc.createElement("maxVersion")
        'node.InnerText = getMaxAllowedVersion()
        'pnode.appendChild(node)
        If isSet() Then
            node = configDoc.CreateElement("value")
            node.InnerText = getValue().ToString
            pnode.AppendChild(node)
        End If
        configDoc.AppendChild(pnode)
        Return configDoc
    End Function

End Class

Public Class ChannelParameter
    Inherits CommandParameter(Of Integer)
    Implements ICommandParameter

    Sub New()
        MyBase.New("channel", "The channel", 1, False, {1})
    End Sub

    Sub New(ByVal isOptional As Boolean)
        MyBase.New("channel", "The channel", 1, isOptional, {1})
    End Sub

    Public Overrides Sub setValue(ByRef value As Integer)
        If value > 0 Then
            MyBase.setValue(value)
        Else
            Throw New ArgumentException("Illegal argument channel=" + value + ". The parameter channel has to be greater than 0.")
        End If
    End Sub
End Class

Public Class LayerParameter
    Inherits CommandParameter(Of Integer)
    Implements ICommandParameter

    Sub New()
        MyBase.New("layer", "The layer", 0, True, {1})
    End Sub

    Sub New(ByVal isOptional As Boolean)
        MyBase.New("layer", "The layer", 0, isOptional, {1})
    End Sub

    Public Overrides Sub setValue(ByRef value As Integer)
        If value > -1 Then
            MyBase.setValue(value)
        Else
            Throw New ArgumentException("Illegal argument layer=" + value + ". The parameter layer has to be possitiv.")
        End If
    End Sub
End Class