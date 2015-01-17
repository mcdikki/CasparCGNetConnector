Imports NUnit.Framework
Imports CasparCGNETConnector

Public Class MixerCommandTests

    <Test>
    Public Sub testChannelGrid()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New ChannelGridCommand()

        ' Test commandstring
        Assert.That(cmd.getCommandString, [Is].EqualTo("CHANNEL_GRID").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerAnchor()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerAnchorCommand()

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ANCHOR").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setX(0.5)
        cmd.setY(0.6)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ANCHOR 0.5 0.6").IgnoreCase, "Wrong set command string.")
        ' ...and animated
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ANCHOR 0.5 0.6 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerAnchorCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ANCHOR").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerAnchorCommand(1, 1, 0.5, 0.6)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ANCHOR 0.5 0.6").IgnoreCase, "Wrong set command string.")
        ' ...and animated
        cmd = New MixerAnchorCommand(1, 1, 0.5, 0.6, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ANCHOR 0.5 0.6 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 7}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerBlend()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerBlendCommand()

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BLEND").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setBlendmode("SCREEN")
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BLEND SCREEN").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerBlendCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BLEND").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerBlendCommand(1, 1, "Screen")
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BLEND SCREEN").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerBrightness()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerBrightnessCommand()

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BRIGHTNESS").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setBrightness(0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BRIGHTNESS 0.5").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BRIGHTNESS 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerBrightnessCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BRIGHTNESS").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerBrightnessCommand(1, 1, 0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BRIGHTNESS 0.5").IgnoreCase, "Wrong set command string.")
        cmd = New MixerBrightnessCommand(1, 1, 0.5, 30, CasparCGUtil.Tweens.linear)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 BRIGHTNESS 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerChroma()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerChromaCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CHROMA").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setColor("GREEN")
        cmd.setSoftness(0.2)
        cmd.setThreshold(0.1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CHROMA GREEN 0.1 0.2").IgnoreCase, "Wrong set command string.")
        cmd.setColor("None")
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CHROMA NONE").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerChromaCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CHROMA").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerChromaCommand(1, 1, "green", 0.1, 0.2)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CHROMA GREEN 0.1 0.2").IgnoreCase, "Wrong set command string.")
        cmd = New MixerChromaCommand(1, 1, "none")
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CHROMA NONE").IgnoreCase, "Wrong set command string.")


        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 4}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerClear()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerClearCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLEAR").IgnoreCase, "Wrong querry command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerClearCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLEAR").IgnoreCase, "Wrong querry command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerClip()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerClipCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLIP").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setX(0.1)
        cmd.setY(0.2)
        cmd.setXscale(0.8)
        cmd.setYscale(0.9)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLIP 0.1 0.2 0.8 0.9").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLIP 0.1 0.2 0.8 0.9 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerClipCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLIP").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerClipCommand(1, 1, 0.1, 0.2, 0.8, 0.9)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLIP 0.1 0.2 0.8 0.9").IgnoreCase, "Wrong set command string.")
        cmd = New MixerClipCommand(1, 1, 0.1, 0.2, 0.8, 0.9, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CLIP 0.1 0.2 0.8 0.9 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerContrast()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerContrastCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CONTRAST").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setContrast(0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CONTRAST 0.5").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CONTRAST 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerContrastCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CONTRAST").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerContrastCommand(1, 1, 0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CONTRAST 0.5").IgnoreCase, "Wrong set command string.")
        cmd = New MixerContrastCommand(1, 1, 0.5, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CONTRAST 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerCrop()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerCropCommand()

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CROP").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setLeft(0.2)
        cmd.setTop(0.3)
        cmd.setRight(0.8)
        cmd.setBottom(0.9)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CROP 0.2 0.3 0.8 0.9").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CROP 0.2 0.3 0.8 0.9 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerCropCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CROP").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerCropCommand(1, 1, 0.2, 0.3, 0.8, 0.9)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CROP 0.2 0.3 0.8 0.9").IgnoreCase, "Wrong set command string.")
        cmd = New MixerCropCommand(1, 1, 0.2, 0.3, 0.8, 0.9, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 CROP 0.2 0.3 0.8 0.9 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 7}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerFill()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerFillCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 FILL").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setX(0.1)
        cmd.setY(0.2)
        cmd.setXscale(0.8)
        cmd.setYscale(0.9)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 FILL 0.1 0.2 0.8 0.9").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 FILL 0.1 0.2 0.8 0.9 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerFillCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 FILL").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerFillCommand(1, 1, 0.1, 0.2, 0.8, 0.9)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 FILL 0.1 0.2 0.8 0.9").IgnoreCase, "Wrong set command string.")
        cmd = New MixerFillCommand(1, 1, 0.1, 0.2, 0.8, 0.9, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 FILL 0.1 0.2 0.8 0.9 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerGrid()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerGridCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")

        ' Test a set command
        cmd.setChannel(1)
        cmd.setResolution(3)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 GRID 3").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 GRID 3 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a set command
        cmd = New MixerGridCommand(1, 3)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 GRID 3").IgnoreCase, "Wrong set command string.")
        cmd = New MixerGridCommand(1, 3, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 GRID 3 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerKeyer()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerKeyerCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 KEYER").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setKeyer(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 KEYER 1").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerKeyerCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 KEYER").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerKeyerCommand(1, 1, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 KEYER 1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerLevels()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerLevelsCommand()

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 LEVELS").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setMinInput(0.1)
        cmd.setMaxInput(0.9)
        cmd.setGamma(0.5)
        cmd.setMinOutput(0.0)
        cmd.setMaxOutput(1.0)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 LEVELS 0.1 0.9 0.5 0 1").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 LEVELS 0.1 0.9 0.5 0 1 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerLevelsCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 LEVELS").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerLevelsCommand(1, 1, 0.1, 0.9, 0.5, 0, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 LEVELS 0.1 0.9 0.5 0 1").IgnoreCase, "Wrong set command string.")
        cmd = New MixerLevelsCommand(1, 1, 0.1, 0.9, 0.5, 0, 1, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 LEVELS 0.1 0.9 0.5 0 1 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerMasterVolume()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerMastervolumeCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        ' construct default layer works
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")

        ' Test a query command
        cmd.setChannel(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 MASTERVOLUME").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setVolume(0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 MASTERVOLUME 0.5").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerMastervolumeCommand(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 MASTERVOLUME").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerMastervolumeCommand(1, 0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1 MASTERVOLUME 0.5").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerOpacity()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerOpacityCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 OPACITY").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setOpacity(0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 OPACITY 0.5").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 OPACITY 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerOpacityCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 OPACITY").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerOpacityCommand(1, 1, 0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 OPACITY 0.5").IgnoreCase, "Wrong set command string.")
        cmd = New MixerOpacityCommand(1, 1, 0.5, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 OPACITY 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerPerspective()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerPerspectiveCommand()

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 PERSPECTIVE").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setTopLeftX(0.2)
        cmd.setTopLeftY(0.2)
        cmd.setTopRightX(0.8)
        cmd.setTopRightY(0.2)
        cmd.setBottomRightX(0.8)
        cmd.setBottomRightY(0.8)
        cmd.setBottomLeftX(0.2)
        cmd.setBottomLeftY(0.8)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 PERSPECTIVE 0.2 0.2 0.8 0.2 0.8 0.8 0.2 0.8").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 PERSPECTIVE 0.2 0.2 0.8 0.2 0.8 0.8 0.2 0.8 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerPerspectiveCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 PERSPECTIVE").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerPerspectiveCommand(1, 1, 0.2, 0.2, 0.8, 0.2, 0.8, 0.8, 0.2, 0.8)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 PERSPECTIVE 0.2 0.2 0.8 0.2 0.8 0.8 0.2 0.8").IgnoreCase, "Wrong set command string.")
        cmd = New MixerPerspectiveCommand(1, 1, 0.2, 0.2, 0.8, 0.2, 0.8, 0.8, 0.2, 0.8, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 PERSPECTIVE 0.2 0.2 0.8 0.2 0.8 0.8 0.2 0.8 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 7}), "Wrong requiredVersion")
    End Sub

    <Test>
    Public Sub testMixerRotation()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerRotationCommand()

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ROTATION").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setAngle(180)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ROTATION 180").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ROTATION 180 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerRotationCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ROTATION").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerRotationCommand(1, 1, 180)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ROTATION 180").IgnoreCase, "Wrong set command string.")
        cmd = New MixerRotationCommand(1, 1, 180, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 ROTATION 180 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2, 0, 7}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerSaturation()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerSaturationCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 SATURATION").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setSaturation(0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 SATURATION 0.5").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 SATURATION 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerSaturationCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 SATURATION").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerSaturationCommand(1, 1, 0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 SATURATION 0.5").IgnoreCase, "Wrong set command string.")
        cmd = New MixerSaturationCommand(1, 1, 0.5, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 SATURATION 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerStraightAlphaOutput()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerStraightAlphaOutputCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 STRAIGHT_ALPHA_OUTPUT").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setActive(True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 STRAIGHT_ALPHA_OUTPUT 1").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerStraightAlphaOutputCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 STRAIGHT_ALPHA_OUTPUT").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerStraightAlphaOutputCommand(1, 1, True)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 STRAIGHT_ALPHA_OUTPUT 1").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({2}), "Wrong requiredVersion")

    End Sub

    <Test>
    Public Sub testMixerVolume()
        ' Tests with empty constructor
        '------------------------------
        Dim cmd As New MixerVolumeCommand

        ' Nothing should be set yet
        testCommandParameterUnset(cmd)

        ' Test destination
        testCommandDestination(cmd)

        ' Test a query command
        cmd.setChannel(1)
        cmd.setLayer(1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 VOLUME").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd.setVolume(0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 VOLUME 0.5").IgnoreCase, "Wrong set command string.")
        cmd.setDuration(30)
        cmd.setTween(CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 VOLUME 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")


        ' Tests with parameterized construtor
        '-------------------------------------
        ' Test a query command
        cmd = New MixerVolumeCommand(1, 1)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 VOLUME").IgnoreCase, "Wrong querry command string.")

        ' Test a set command
        cmd = New MixerVolumeCommand(1, 1, 0.5)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 VOLUME 0.5").IgnoreCase, "Wrong set command string.")
        cmd = New MixerVolumeCommand(1, 1, 0.5, 30, CasparCGUtil.Tweens.linear)
        Assert.That(cmd.getCommandString, [Is].EqualTo("MIXER 1-1 VOLUME 0.5 30 LINEAR").IgnoreCase, "Wrong set command string.")

        ' Test compatibility 
        Assert.That(cmd.getRequiredVersion, [Is].EqualTo({1}), "Wrong requiredVersion")
    End Sub


    '' Generic helper tests
    ''======================

    Private Sub testCommandParameterUnset(ByRef cmd As AbstractCommand)
        For Each cp In cmd.getCommandParameters
            Assert.That(cp.isSet, [Is].False, "Uninitialized command parameter marked as set.")
        Next
    End Sub

    Private Sub testCommandDestination(ByRef cmd As AbstractCommand)
        ' Channel and Layer parameter exists
        Assert.That(cmd.getCommandParameter("channel"), [Is].Not.Null, "Channel parameter missing.")
        Assert.That(cmd.getCommandParameter("layer"), [Is].Not.Null, "Layer parameter missing.")

        ' construct default layer works
        DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(5)
        Assert.That(DirectCast(cmd.getCommandParameter("channel"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(5), "Set channel failed.")
        Assert.That(cmd.getCommandString(), [Is].StringContaining("MIXER 5 ").IgnoreCase, "Default layer not set correctly.")

        ' Set layer works
        DirectCast(cmd.getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(2)
        Assert.That(DirectCast(cmd.getCommandParameter("layer"), CommandParameter(Of Integer)).getValue, [Is].EqualTo(2), "Set layer failed.")
        Assert.That(cmd.getCommandString(), [Is].StringContaining("MIXER 5-2 ").IgnoreCase, "Destination not set correctly.")
    End Sub

End Class
