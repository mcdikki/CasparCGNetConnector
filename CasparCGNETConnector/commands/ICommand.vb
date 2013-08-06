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

