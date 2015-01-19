Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class TestUtils

    '' Generic helper tests
    ''======================

    Public Shared Sub testCommandDestination(ByRef cmd As AbstractCommand)
        ' Channel and Layer parameter exists
        Assert.That(cmd.getCommandParameter("channel"), [Is].Not.Null, "Channel parameter missing.")
        Assert.That(cmd.getCommandParameter("layer"), [Is].Not.Null, "Layer parameter missing.")

        ' construct default layer works
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")
        Assert.That(cmd.getDestination, [Is].StringStarting("5").IgnoreCase, "Default layer not set correctly.")

        ' Set layer works
        DirectCast(cmd.getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(2)
        Assert.That(DirectCast(cmd.getCommandParameter("layer"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(2), "Set layer failed.")
        Assert.That(cmd.getDestination, [Is].StringStarting("5-2").IgnoreCase, "Destination not set correctly.")
    End Sub

    Protected Friend Shared Sub testCommandParameterUnset(ByRef cmd As AbstractCommand)
        For Each cp In cmd.getCommandParameters
            Assert.That(cp.isSet, [Is].False, "Uninitialized command parameter marked as set.")
        Next
    End Sub
End Class
