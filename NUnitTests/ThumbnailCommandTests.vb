Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class ThumbnailCommandTests

    Dim media As New CasparCGMovie("amb")

    <Test>
    Public Sub testThumbnailGenerateAll()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ThumbnailGenerateAllCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL GENERATE_ALL").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testThumbnailGenerate()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ThumbnailGenerateCommand

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
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL GENERATE ""amb""").IgnoreCase, "Wrong set command string.")
        cmd.setMedia("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL GENERATE ""test""").IgnoreCase, "Wrong set command string.")

        ' Test with parameterized constructor
        cmd = New ThumbnailGenerateCommand(media)
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL GENERATE ""amb""").IgnoreCase, "Wrong set command string.")
        cmd = New ThumbnailGenerateCommand("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL GENERATE ""test""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testThumbnailList()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ThumbnailListCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL LIST").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testThumbnailRetrieve()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ThumbnailRetrieveCommand

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
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL RETRIEVE ""amb""").IgnoreCase, "Wrong set command string.")
        cmd.setMedia("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL RETRIEVE ""test""").IgnoreCase, "Wrong set command string.")

        ' Test with parameterized constructor
        cmd = New ThumbnailRetrieveCommand(media)
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL RETRIEVE ""amb""").IgnoreCase, "Wrong set command string.")
        cmd = New ThumbnailRetrieveCommand("test")
        Assert.That(cmd.getCommandString, [Is].EqualTo("THUMBNAIL RETRIEVE ""test""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

End Class
