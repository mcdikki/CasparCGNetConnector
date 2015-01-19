Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class DataCommandTests


    <Test>
    Public Sub testDataList()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New DataListCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("DATA LIST").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testDataRemove()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New DataRemoveCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing key.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing key.")
        End Try

        cmd.setKey("key")
        Assert.That(cmd.getCommandString, [Is].EqualTo("DATA REMOVE ""key""").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        cmd = New DataRemoveCommand("key")
        Assert.That(cmd.getCommandString, [Is].EqualTo("DATA REMOVE ""key""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testDataRetrieve()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New DataRetrieveCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing key.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing key.")
        End Try

        cmd.setKey("key")
        Assert.That(cmd.getCommandString, [Is].EqualTo("DATA RETRIEVE ""key""").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        cmd = New DataRetrieveCommand("key")
        Assert.That(cmd.getCommandString, [Is].EqualTo("DATA RETRIEVE ""key""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testDataStore()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New DataStoreCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing key and data.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing key and data.")
        End Try
        cmd.setData("data")
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing key.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException), "Should have thrown an ArgumentNullExcpetion for missing key.")
        End Try
        cmd.setKey("key")
        Assert.That(cmd.getCommandString, [Is].EqualTo("DATA STORE ""key"" ""data""").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        cmd = New DataStoreCommand("key", "<data id=""test"" />")
        Assert.That(cmd.getCommandString, [Is].EqualTo("DATA STORE ""key"" ""<data id=\""test\"" />""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

End Class
