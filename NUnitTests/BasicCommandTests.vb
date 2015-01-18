Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class BasicCommandTests

    Private trans As New CasparCGTransition(CasparCGUtil.Transitions.MIX, 30, , CasparCGUtil.Tweens.linear)
    Private media As New CasparCGMovie("amb")

    <Test>
    Public Sub testAdd()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New AddCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")


        ' Test a set command
        cmd.setChannel(1)
        cmd.setConsumer("SCREEN")
        Assert.That(cmd.getCommandString, [Is].EqualTo("ADD 1 SCREEN").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New AddCommand(1, "FILE", {"Test.jpg", "SEPARATE_KEY"})
        Assert.That(cmd.getCommandString, [Is].EqualTo("ADD 1 FILE Test.jpg SEPARATE_KEY").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testCall()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New CallCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        cmd.setLooping(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP").IgnoreCase, "Wrong set command string.")
        cmd.setTransition(trans)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP MIX 30 LINEAR").IgnoreCase, "Wrong set command string.")
        cmd.setSeek(10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP MIX 30 LINEAR SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd.setLength(100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP MIX 30 LINEAR SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd.setFilter("hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New CallCommand(1, 1, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP").IgnoreCase, "Wrong set command string.")
        cmd = New CallCommand(1, 1, True, , , trans)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP MIX 30 LINEAR").IgnoreCase, "Wrong set command string.")
        cmd = New CallCommand(1, 1, True, 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd = New CallCommand(1, 1, True, 10, 100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd = New CallCommand(1, 1, True, 10, 100, trans, "hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("CALL 1-1 LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testClear()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ClearCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CLEAR 1-1").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New ClearCommand()
        Assert.That(cmd.getCommandString, [Is].EqualTo("CLEAR 1").IgnoreCase, "Wrong set command string.")
        cmd = New ClearCommand(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CLEAR 2").IgnoreCase, "Wrong set command string.")
        cmd = New ClearCommand(2, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("CLEAR 2-1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testKill()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New KillCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("KILL").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testLoadbg()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New LoadbgCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        cmd.setMedia(media)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb""").IgnoreCase, "Wrong set command string.")
        cmd.setAutostarting(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO").IgnoreCase, "Wrong set command string.")
        cmd.setLooping(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO LOOP").IgnoreCase, "Wrong set command string.")
        cmd.setTransition(trans)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO LOOP MIX 30 LINEAR").IgnoreCase, "Wrong set command string.")
        cmd.setSeek(10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO LOOP MIX 30 LINEAR SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd.setLength(100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO LOOP MIX 30 LINEAR SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd.setFilter("hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New LoadbgCommand(1, 1, "amb")
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb""").IgnoreCase, "Wrong set command string.")
        cmd = New LoadbgCommand(1, 1, media, True, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO LOOP").IgnoreCase, "Wrong set command string.")
        cmd = New LoadbgCommand(1, 1, media, True, , 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd = New LoadbgCommand(1, 1, media, , True, 10, 100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" LOOP SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd = New LoadbgCommand(1, 1, media, True, True, 10, 100, trans, "hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOADBG 1-1 ""amb"" AUTO LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testLoad()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New LoadCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        cmd.setMedia(media)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb""").IgnoreCase, "Wrong set command string.")
        cmd.setLooping(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP").IgnoreCase, "Wrong set command string.")
        cmd.setTransition(trans)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP MIX 30 LINEAR").IgnoreCase, "Wrong set command string.")
        cmd.setSeek(10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd.setLength(100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd.setFilter("hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New LoadCommand(1, 1, "amb")
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb""").IgnoreCase, "Wrong set command string.")
        cmd = New LoadCommand(1, 1, media, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP").IgnoreCase, "Wrong set command string.")
        cmd = New LoadCommand(1, 1, media, , 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd = New LoadCommand(1, 1, media, True, 10, 100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd = New LoadCommand(1, 1, media, True, 10, 100, trans, "hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("LOAD 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testPause()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New PauseCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PAUSE 1-1").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New PauseCommand
        Assert.That(cmd.getCommandString, [Is].EqualTo("PAUSE 1").IgnoreCase, "Wrong set command string.")
        cmd = New PauseCommand(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PAUSE 2").IgnoreCase, "Wrong set command string.")
        cmd = New PauseCommand(2, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PAUSE 2-1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testPlay()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New PlayCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1").IgnoreCase, "Wrong set command string.")
        cmd.setMedia(media)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb""").IgnoreCase, "Wrong set command string.")
        cmd.setLooping(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP").IgnoreCase, "Wrong set command string.")
        cmd.setTransition(trans)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP MIX 30 LINEAR").IgnoreCase, "Wrong set command string.")
        cmd.setSeek(10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd.setLength(100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd.setFilter("hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New PlayCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1").IgnoreCase, "Wrong set command string.")
        cmd = New PlayCommand(1, 1, "amb")
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb""").IgnoreCase, "Wrong set command string.")
        cmd = New PlayCommand(1, 1, media, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP").IgnoreCase, "Wrong set command string.")
        cmd = New PlayCommand(1, 1, media, , 10)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" SEEK 10").IgnoreCase, "Wrong set command string.")
        cmd = New PlayCommand(1, 1, media, True, 10, 100)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP SEEK 10 LENGTH 100").IgnoreCase, "Wrong set command string.")
        cmd = New PlayCommand(1, 1, media, True, 10, 100, trans, "hflip")
        Assert.That(cmd.getCommandString, [Is].EqualTo("PLAY 1-1 ""amb"" LOOP MIX 30 LINEAR SEEK 10 LENGTH 100 FILTER ""hflip""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testPrint()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New PrintCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")


        ' Test a set command
        cmd.setChannel(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PRINT 1").IgnoreCase, "Wrong set command string.")
        cmd.setFile("Screenshot.png")
        Assert.That(cmd.getCommandString, [Is].EqualTo("PRINT 1 ""Screenshot.png""").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New PrintCommand(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("PRINT 1").IgnoreCase, "Wrong set command string.")
        cmd = New PrintCommand(1, "Screenshot.png")
        Assert.That(cmd.getCommandString, [Is].EqualTo("PRINT 1 ""Screenshot.png""").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testRemove()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New RemoveCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")


        ' Test a set command
        cmd.setChannel(1)
        cmd.setConsumer("SCREEN")
        Assert.That(cmd.getCommandString, [Is].EqualTo("REMOVE 1 SCREEN").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New RemoveCommand(1, "DECKLINK", {"1"})
        Assert.That(cmd.getCommandString, [Is].EqualTo("REMOVE 1 DECKLINK 1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testRestart()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New RestartCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        Assert.That(cmd.getCommandString, [Is].EqualTo("RESTART").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testResume()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ResumeCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("RESUME 1-1").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New ResumeCommand
        Assert.That(cmd.getCommandString, [Is].EqualTo("RESUME 1").IgnoreCase, "Wrong set command string.")
        cmd = New ResumeCommand(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("RESUME 2").IgnoreCase, "Wrong set command string.")
        cmd = New ResumeCommand(2, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("RESUME 2-1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 7}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testRoute()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New RouteCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        cmd.setSourceChannel(2)
        cmd.setSourceLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("ROUTE 1-1 route://2-1").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New RouteCommand(1, 1, 2, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("ROUTE 1-1 route://2-1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testSet()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New SetCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)


        ' Test destination
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")


        ' Test a set command
        cmd.setChannel(1)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing videomode.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException))
        End Try
        cmd.setVidemode("PAL")
        Assert.That(cmd.getCommandString, [Is].EqualTo("SET 1 MODE PAL").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New SetCommand(1, "PAL")
        Assert.That(cmd.getCommandString, [Is].EqualTo("SET 1 MODE PAL").IgnoreCase, "Wrong set command string.")
        cmd = New SetCommand(1, Nothing)
        Try
            cmd.getCommandString()
            Assert.That(False, "Should have thrown an ArgumentNullExcpetion for missing videomode.")
        Catch ex As Exception
            Assert.That(ex, [Is].TypeOf(Of ArgumentNullException))
        End Try

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testStop()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New StopCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a set command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("STOP 1-1").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New StopCommand
        Assert.That(cmd.getCommandString, [Is].EqualTo("STOP 1").IgnoreCase, "Wrong set command string.")
        cmd = New StopCommand(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("STOP 2").IgnoreCase, "Wrong set command string.")
        cmd = New StopCommand(2, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("STOP 2-1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testSwap()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New SwapCommand

        ' Nothing should be set yet
        TestUtils.testCommandParameterUnset(cmd)

        ' Test a set command
        cmd.setChannelA(1)
        cmd.setChannelB(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("SWAP 1 2").IgnoreCase, "Wrong set command string.")
        cmd.setLayerB(2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("SWAP 1 2").IgnoreCase, "Wrong set command string.")
        cmd.setLayerA(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("SWAP 1-1 2-2").IgnoreCase, "Wrong set command string.")

        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New SwapCommand(1, 2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("SWAP 1 2").IgnoreCase, "Wrong set command string.")
        cmd = New SwapCommand(1, 2, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("SWAP 1 2").IgnoreCase, "Wrong set command string.")
        cmd = New SwapCommand(1, 2, 1, 2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("SWAP 1-1 2-2").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    '' Generic helper tests
    ''======================
    Private Sub testCommandDestination(ByRef cmd As AbstractCommand)
        ' Channel and Layer parameter exists
        Assert.That(cmd.getCommandParameter("channel"), [Is].Not.Null, "Channel parameter missing.")
        Assert.That(cmd.getCommandParameter("layer"), [Is].Not.Null, "Layer parameter missing.")

        ' construct default layer works
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")
        Assert.That(cmd.getCommandString(), [Is].StringContaining(cmd.getName & " 5").IgnoreCase, "Default layer not set correctly.")

        ' Set layer works
        DirectCast(cmd.getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(2)
        Assert.That(DirectCast(cmd.getCommandParameter("layer"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(2), "Set layer failed.")
        Assert.That(cmd.getCommandString(), [Is].StringContaining(cmd.getName & " 5-2").IgnoreCase, "Destination not set correctly.")
    End Sub

End Class
