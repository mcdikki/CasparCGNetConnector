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

Public Interface ICommand

    ''' <summary>
    ''' Executes the command using the given connection and returns the <see cref="CasparCGResponse">CasparCGResponse</see>.
    ''' Throws a <see cref="NotSupportedException">NotSupportedException</see> if the command is not compatible to the CasparCG Server version.
    ''' </summary>
    ''' <param name="connection">the CasparCGConnection to execute the command on</param>
    ''' <returns>a CasparCGResponse if, and only if the command is compatible to the connected server version, else throws a NotSupportedException</returns>
    Function execute(ByRef connection As CasparCGConnection) As CasparCGResponse

    ''' <summary>
    ''' Returns the <see cref=" CasparCGResponse ">CasparCGResponse</see> of the last command execution or nothing.
    ''' </summary>
    ''' <returns>the <see cref=" CasparCGResponse ">CasparCGResponse</see> or nothing</returns>
    Function getResponse() As CasparCGResponse

    ''' <summary>
    ''' Returns the name of this command
    ''' </summary>
    ''' <returns>The name of this command</returns>
    Function getName() As String

    ''' <summary>
    ''' Returns the Description of this command
    ''' </summary>
    ''' <returns>the Description of this command</returns>
    ''' <remarks></remarks>
    Function getDescription() As String

    ''' <summary>
    ''' Returns a list of all <seealso cref=" ICommandParameter ">parameter</seealso> names of this command
    ''' </summary>
    ''' <returns>a list of paramter names</returns>
    Function getParameterNames() As List(Of String)

    ''' <summary>
    ''' Returns a list of all <seealso cref=" ICommandParameter ">parameter</seealso> of this command
    ''' </summary>
    ''' <returns>a list of paramters</returns>
    Function getParameters() As List(Of ICommandParameter)

    ''' <summary>
    ''' Returns the <seealso cref=" ICommandParameter ">parameter</seealso> ot the given name or nothing if no parameter of this name exists.
    ''' </summary>
    ''' <param name="parameterName">the all lowercase name of the parameter</param>
    ''' <returns>the <seealso cref=" ICommandParameter ">parameter</seealso> if, and only if, it exists, else nothing</returns>
    Function getParameter(ByVal parameterName As String) As ICommandParameter

    ''' <summary>
    ''' Returns whether or not this command is compatible to the CasparCG Server version of the given connection.
    ''' </summary>
    ''' <param name="connection">the <see cref=" CasparCGConnection ">connection</see></param>
    ''' <returns>true, if and only if the commmand is compatible</returns>
    Function isCompatible(ByRef connection As CasparCGConnection) As Boolean

    ''' <summary>
    ''' Returns the command as string
    ''' </summary>
    ''' <returns>the command as string</returns>
    Function getCommandString() As String

    ''' <summary>
    ''' Returns the required version to run this command
    ''' </summary>
    ''' <returns>the version to run this command as array of Integer</returns>
    Function getRequiredVersion() As Integer()

    ''' <summary>
    ''' Returns the maximum version to run this command
    ''' </summary>
    ''' <returns>the highest version to run this command as array of Integer</returns>
    Function getMaxAllowedVersion() As Integer()


    Function toXml() As MSXML2.DOMDocument

End Interface

