Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class CgCommandTests
    Private t As CasparCGTemplate
    Private td As CasparCGTemplate
    Private Const tXml = "<?xml version=""1.0"" encoding=""utf-8""?><template version=""1.8.0"" authorName=""Andreas Jeansson"" authorEmail=""andreas.jeansson@svt.se"" templateInfo="""" originalWidth=""1024"" originalHeight=""576"" originalFrameRate=""25""><components><component name=""CasparTextField""><property name=""text"" type=""string"" info=""String data""/></component></components><keyframes/><instances><instance name=""f1"" type=""CasparTextField""/><instance name=""f0"" type=""CasparTextField""/></instances></template>"
    Private Const tdStr = """<templateData><componentData id=\""f1\""><data id=\""text\"" value=\""Name\"" /></componentData><componentData id=\""f0\""><data id=\""text\"" value=\""Surname\"" /></componentData></templateData>"""

    <SetUp>
    Public Sub setUpTestdata()
        t = New CasparCGTemplate("Simpletemplate2", tXml)
        td = New CasparCGTemplate("Simpletemplate2", tXml)
        ' Initialize Template and its data
        td.getData.getInstance("f0").setValue("text", "Surname")
        td.getData.getInstance("f1").setValue("text", "Name")
    End Sub

    <Test>
    Public Sub testCgAdd()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgAddCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing template and flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing template and flashlayer.")
        End Try
        cmd.setTemplate(t)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        End Try
        cmd.setFlashlayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 0").IgnoreCase, "Wrong set command string.")
        cmd.setPlayOnLoad(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 1").IgnoreCase, "Wrong set command string.")
        cmd.setData("empty")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 1 ""empty""").IgnoreCase, "Wrong set command string.")

        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgAddCommand(1, 1, t, 1, False)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 0").IgnoreCase, "Wrong set command string.")
        cmd = New CgAddCommand(1, 1, t, 1, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 1").IgnoreCase, "Wrong set command string.")
        cmd = New CgAddCommand(1, 1, td, 1, True, "empty")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 1 ""empty""").IgnoreCase, "Wrong set command string.")
        cmd = New CgAddCommand(1, 1, td, 1, False)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 0 " & tdStr).IgnoreCase, "Wrong set command string.")
        cmd = New CgAddCommand(1, 1, td, 1, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 ADD 1 ""SIMPLETEMPLATE2"" 1 " & tdStr).IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCgClear()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgClearCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 CLEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgClearCommand(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1 CLEAR").IgnoreCase, "Wrong set command string.")
        cmd = New CgClearCommand(1, 2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-2 CLEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCgInvoke()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgInvokeCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer and method.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer and method.")
        End Try
        cmd.setMethod("update()")
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        End Try
        cmd.setFlashlayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 INVOKE 1 ""update()""").IgnoreCase, "Wrong set command string.")


        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgInvokeCommand(1, 2, 10, "update()")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-2 INVOKE 10 ""update()""").IgnoreCase, "Wrong set command string.")


        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCgNext()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgNextCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        End Try
        cmd.setFlashlayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 NEXT 1").IgnoreCase, "Wrong set command string.")


        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgNextCommand(1, 2, 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-2 NEXT 10").IgnoreCase, "Wrong set command string.")


        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCgPlay()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgPlayCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        End Try
        cmd.setFlashlayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 PLAY 1").IgnoreCase, "Wrong set command string.")


        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgPlayCommand(1, 2, 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-2 PLAY 10").IgnoreCase, "Wrong set command string.")


        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCgRemove()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgRemoveCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        End Try
        cmd.setFlashlayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 REMOVE 1").IgnoreCase, "Wrong set command string.")


        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgRemoveCommand(1, 2, 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-2 REMOVE 10").IgnoreCase, "Wrong set command string.")


        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCgStop()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgStopCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        End Try
        cmd.setFlashlayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 STOP 1").IgnoreCase, "Wrong set command string.")


        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgStopCommand(1, 2, 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-2 STOP 10").IgnoreCase, "Wrong set command string.")


        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCgUpdate()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CgUpdateCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing data and flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing data and flashlayer.")
        End Try
        cmd.setData("empty")
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing flashlayer.")
        End Try
        cmd.setFlashlayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 UPDATE 1 ""empty""").IgnoreCase, "Wrong set command string.")

        ' Test destination
        TestUtils.testCommandDestination(cmd)


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CgUpdateCommand(1, 1, 1, "empty")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 UPDATE 1 ""empty""").IgnoreCase, "Wrong set command string.")
        cmd = New CgUpdateCommand(1, 1, 1, td.getData)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CG 1-1 UPDATE 1 " & tdStr).IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

End Class
