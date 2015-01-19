Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class QueryCommandTests

    Dim media As New CasparCGMovie("amb")
    Dim template As New CasparCGTemplate("phone")

    <Test>
    Public Sub testCinf()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CinfCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullException for missing media.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullException for missing media.")
        End Try

        ' Test a set command
        cmd.setMedia(media)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CINF ""amb""").IgnoreCase, "Wrong set command string.")
        cmd.setMedia("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CINF ""test""").IgnoreCase, "Wrong set command string.")

        ' Test parameterized constructors
        cmd = New CinfCommand(media)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CINF ""amb""").IgnoreCase, "Wrong set command string.")
        cmd = New CinfCommand("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CINF ""test""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCls()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ClsCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("CLS").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    Public Sub testInfo()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO").IgnoreCase, "Wrong set command string.")
        cmd.setChannel(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2").IgnoreCase, "Wrong set command string.")
        cmd.setOnlyBackground(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 B").IgnoreCase, "Wrong set command string.")
        cmd.setOnlyBackground(True)
        cmd.setOnlyForeground(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 B").IgnoreCase, "Wrong set command string.")
        cmd.setOnlyBackground(False)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 F").IgnoreCase, "Wrong set command string.")
        cmd.setDelay(True)
        cmd.setOnlyBackground(False)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 DELAY").IgnoreCase, "Wrong set command string.")
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2-1 DELAY").IgnoreCase, "Wrong set command string.")
        cmd.setOnlyBackground(True)
        cmd.setDelay(False)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2-1 B").IgnoreCase, "Wrong set command string.")


        ' Test parameterized constructors
        cmd = New InfoCommand(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, , True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 B").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, , True, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 B").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, , , True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 F").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, , , , True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2 DELAY").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, 1, , , True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2-1 DELAY").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, 1, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2-1 B").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, 1, True, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2-1 B").IgnoreCase, "Wrong set command string.")
        cmd = New InfoCommand(2, 1, , True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO 2-1 F").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testInfoConfig()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoConfigCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO CONFIG").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testInfoPaths()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoPathsCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO PATHS").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testInfoQueues()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoQueuesCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO QUEUES").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 7}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testInfoServer()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoServerCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO SERVER").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testInfoSystem()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoSystemCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO SYSTEM").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testInfoTemplate()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoTemplateCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullException for missing template.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullException for missing template.")
        End Try

        ' Test a set command
        cmd.setTemplate(template)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO TEMPLATE ""phone""").IgnoreCase, "Wrong set command string.")
        cmd.setTemplate("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO TEMPLATE ""test""").IgnoreCase, "Wrong set command string.")

        ' Test parameterized constructors
        cmd = New InfoTemplateCommand(template)
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO TEMPLATE ""phone""").IgnoreCase, "Wrong set command string.")
        cmd = New InfoTemplateCommand("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO TEMPLATE ""test""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testInfoThreads()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New InfoThreadsCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("INFO THREADS").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 7}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testTls()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New TlsCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("TLS").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testVersionFlash()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New VersionFlashCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("VERSION FLASH").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testVersionServer()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New VersionServerCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("VERSION SERVER").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({-1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testVersionTemplatehost()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New VersionTemplatehostCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("VERSION TEMPLATEHOST").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

End Class
