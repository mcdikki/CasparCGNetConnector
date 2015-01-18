Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class TestUtils

    '' Generic helper tests
    ''======================

    Protected Friend Shared Sub testCommandParameterUnset(ByRef cmd As AbstractCommand)
        For Each cp In cmd.getCommandParameters
            Assert.That(cp.isSet, [Is].False, "Uninitialized command parameter marked as set.")
        Next
    End Sub
End Class
