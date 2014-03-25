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

Imports CasparCGNETConnector    

''' <summary>
''' This is an example of how to use the CasparCGNetConnector
''' </summary>
''' <remarks></remarks>
''' 
Public Module Example

    Private WithEvents connection As CasparCGConnection


    Public Sub Main()
        connection = New CasparCGConnection("localhost", 5250)
        Console.WriteLine(" CasparCGNetConnector ")
        Console.WriteLine("======================")
        connect()
        Console.WriteLine("Please choose the function to run or enter Q to exit:")
        Console.WriteLine(vbTab & "1. Test_Manual_Command")
        Console.WriteLine(vbTab & "2. Test_Automatic_Command")
        Console.WriteLine(vbTab & "3. Test_Plaintext_Command")
        Console.WriteLine(vbTab & "4. Test_CasparCGMedia_Classes")
        Console.WriteLine(vbTab & "5. Test_ConnectionBreakdown")
        Console.WriteLine(vbTab & "C. Connect to casparCG")
        Console.WriteLine(vbTab & "Q. Quit program")
        Dim i As String = Console.ReadKey().KeyChar
        While i.ToUpper <> "Q"
            Console.WriteLine()
            Select Case i.ToUpper
                Case "1"
                    Console.WriteLine("==================================")
                    Console.WriteLine("=== Start Test_Manual_Command: ===")
                    Console.WriteLine("==================================")
                    Test_Manual_Command()
                Case "2"
                    Console.WriteLine("=====================================")
                    Console.WriteLine("=== Start Test_Automatic_Command: ===")
                    Console.WriteLine("=====================================")
                    Test_Automatic_Command()
                Case "3"
                    Console.WriteLine("=====================================")
                    Console.WriteLine("=== Start Test_Plaintext_Command: ===")
                    Console.WriteLine("=====================================")
                    Test_Plaintext_Command()
                Case "4"
                    Console.WriteLine("=========================================")
                    Console.WriteLine("=== Start Test_CasparCGMedia_Classes: ===")
                    Console.WriteLine("=========================================")
                    Test_CasparCGMedia_Classes()
                Case "5"
                    Console.WriteLine("=======================================")
                    Console.WriteLine("=== Start Test_ConnectionBreakdown: ===")
                    Console.WriteLine("=======================================")
                    Test_ConnectionBreakdown()
                Case "T"
                    test()
                Case "C"
                    connect()
            End Select
            Console.WriteLine()
            Console.WriteLine("Please choose the function to run or enter Q to exit:")
            Console.WriteLine(vbTab & "1. Test_Manual_Command")
            Console.WriteLine(vbTab & "2. Test_Automatic_Command")
            Console.WriteLine(vbTab & "3. Test_Plaintext_Command")
            Console.WriteLine(vbTab & "4. Test_CasparCGMedia_Classes")
            Console.WriteLine(vbTab & "5. Test_ConnectionBreakdown")
            Console.WriteLine(vbTab & "C. Connect to casparCG")
            Console.WriteLine(vbTab & "Q. Quit program")
            i = Console.ReadKey().KeyChar
        End While
        connection.close()
    End Sub

    Private Sub test()
        ' Quick tests for debugging
        Dim clear As New ClearCommand(1)
        Dim cmd As New InfoCommand(2)
        Dim play As New PlayCommand(2, 1, "amb", True)

        clear.execute(connection)
        play.execute(connection)
        play.setLayer(4)
        play.setMedia("go1080p25")
        play.execute(connection)

        Console.WriteLine(cmd.execute(connection).getServerMessage)
    End Sub

    Public Sub connect()
        If Not connection.isConnected Then
            Console.WriteLine("Try to connected to " & connection.getServerAddress & ":" & connection.getServerPort)
            If Not connection.connect() Then
                Console.WriteLine("Could not connect to casparCG.")
            End If
        Else
            Console.WriteLine("Already connected to " & connection.getVersion & " @ " & connection.getServerAddress & ":" & connection.getServerPort)
        End If
    End Sub

    Private Sub handleConnect(ByRef sender As Object) Handles connection.connected
        Console.WriteLine("Connected to CasparCG " & connection.getVersion & " @ " & connection.getServerAddress & ":" & connection.getServerPort)
    End Sub

    Private Sub handleDisconnected(ByRef sender As Object) Handles connection.disconnected
        Console.WriteLine("Connection has been dropped.")
    End Sub

    Public Sub Test_ConnectionBreakdown()
        Console.WriteLine("Warning! This will start an endless loop that will only stop if you break the connection to the server.")
        Console.WriteLine("Press Y to procced or any other key to cancel:")
        If Console.ReadKey().KeyChar.ToString.ToUpper.Equals("Y") Then
            Console.WriteLine("Started loop...")
            While connection.isConnected
                Threading.Thread.Sleep(250)
            End While
            Console.WriteLine("...stopped loop.")
        End If
        Console.WriteLine("You have to reconnect in order to use the other examples!")
    End Sub

    ''' <summary>
    ''' In this Sub, we will use the manual way of using the Command classes to execute a command on the server
    ''' </summary>
    Public Sub Test_Manual_Command()
        '' We will do 3 things here
        ''  1. Play a clip
        ''  2. poll some info about that clip
        ''  3. Stop that clip

        '' At first, we need to define a command object of the desired type
        '' In our case, the play command
        '' You can eighter define the parameters at instanciation or later
        '' This time, we will do it all at once and create a play command for
        '' channel 1, layer 1, clip "amb", that loops
        Dim command As AbstractCommand = New PlayCommand(1, 1, "amb", True) '' I used AbstractCommand as Type, so I can reuse command for all other commands

        '' Now that we have the command ready, we can execute it.
        '' to execute the command, we need the connection to the server via which
        '' we will execute it.

        '' execute command
        command.execute(connection)

        '' To check the response of the sever we can access the CasparCGResponse via the command.getResponse or the return value of execute()
        '' Lets first check if everything worked as expected:
        If command.getResponse.isOK Then
            '' Looks good, print what we got
            Console.WriteLine("CasparCG responded " & command.getResponse.getCode)
            Console.WriteLine("Date received: " & command.getResponse.getData)
            Console.WriteLine("Complete server message: " & command.getResponse.getServerMessage)
        End If

        '' Ok, now lets have a look at an other way to set the parameter for a command
        command = New InfoCommand()

        '' List the parameters of the command
        Console.WriteLine("Paramters of " & command.getName)
        For Each param In command.getCommandParameters
            Console.WriteLine(vbTab & param.getName & ":")
            Console.WriteLine(vbTab & vbTab & "type: " & param.getGenericType.ToString & vbNewLine & vbTab & vbTab & "is optional: " & param.isOptional)
        Next

        '' Lets assume we know the parameter and their types, then we can cast directly and set them
        DirectCast(command.getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(1)
        DirectCast(command.getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(1)
        DirectCast(command.getCommandParameter("only foreground"), CommandParameter(Of Boolean)).setValue(True)

        '' If we have used a specific type of command instead of the general AbstractCommand
        '' we can use the named getter/setter methods for the parameters
        Dim info As InfoCommand = command
        info.setChannel(1)
        info.setLayer(1)
        info.setOnlyForeground(True)


        '' The command is ready to be executed
        '' This time, we check the response directly from the execute return
        If command.execute(connection).isOK Then
            '' Info returns XML data, so we can use getXMLData to see them
            Console.WriteLine("The xml responded by the command: " & vbNewLine & command.getResponse.getXMLData)
        End If

        '' At last, we're going to stop the looped playback of amb
        command = New StopCommand(1, 1)
        command.execute(connection)
    End Sub


    ''' <summary>
    ''' In this sub, we will see how to explore and use the commands and their parameters in code. This is usefull if you want to built an UI.
    ''' </summary>
    Public Sub Test_Automatic_Command()
        '' We will list all commands understood by this library,
        '' display them and their parameters.
        '' Then we create a random command and ask the user for the paramter inputs

        '' List all commands and their parameters
        Dim commands As New List(Of AbstractCommand)
        For Each c In [Enum].GetValues(GetType(CasparCGCommandFactory.Command))
            Console.WriteLine("Found Command " & c.ToString)
            commands.Add(CasparCGCommandFactory.getCommand(c))
            For Each param In commands.Last.getCommandParameters
                Console.WriteLine(vbTab & param.getName & ":")
                Console.WriteLine(vbTab & vbTab & "type: " & param.getGenericType.ToString & vbNewLine & vbTab & vbTab & "is optional: " & param.isOptional)
            Next
        Next

        '' Pick a random command and fill the needed parameters by asking the user
        Dim r As New System.Random(System.DateTime.Now.Millisecond)
        Dim cmd As AbstractCommand = commands.Item(r.Next(0, commands.Count - 1))
        Console.WriteLine("Picked " & cmd.getName & " as command. " & vbNewLine & "Description: " & cmd.getDescription & vbNewLine & "Please fill the following parameters:")

        '' Now comes the dirty part.
        '' we need to dynamically cast the parameter object and its value object
        For Each p In cmd.getCommandParameters
            Dim parameter = CTypeDynamic(p, p.getGenericParameterType)
            If Not parameter.isOptional Then
                Dim value = CTypeDynamic(parameter.getDefault(), p.getGenericType)
                Console.WriteLine("Parameter: " & p.getName & " (Description: " & p.getDescription & "): ")
                value = CTypeDynamic(Console.ReadLine(), p.getGenericType)
                parameter.setValue(value)
            End If
        Next

    End Sub


    ''' <summary>
    ''' In this method, we will look how to send plaintext commands like in old school style ;-)
    ''' </summary>
    Public Sub Test_Plaintext_Command()
        '' For plaintext commands you can simply pass a string to the 
        '' CasparCGConnection.sendCommand() function.
        '' As with the execute() of the command classes, you will get 
        '' a CasparCGResponse in return.

        '' check if connected
        If connection.isConnected() Then
            '' When sending commands via sendCommand, you don't need to end with a new line,
            '' but you have to take a look at correct character escaping
            '' To make that easier, you can use CasparCGCommandFactory.esacep(String) as String.
            '' But beware, it corrects esacaping, it can't gues if e.g. a name has to be escaped or not.
            Console.WriteLine("Please type the command you want to send to the server:")
            Dim cmd As String = Console.ReadLine()

            Console.WriteLine("Try to excute plaintext command '" & cmd & "'")
            Dim response As CasparCGResponse = connection.sendCommand(cmd)
            If response.isOK Then
                Console.WriteLine("Yeah, everthing went well!")
            ElseIf response.isERR Then
                Console.WriteLine("Well, something went wrong... - What's the error code and description:" & vbNewLine & response.getCode & " = " & [Enum].GetName(GetType(CasparCGResponse.CasparReturnCode), response.getCode))
            ElseIf response.isUNKNOWN Then
                Console.WriteLine("Hmm, that is funny, let's see the whole server message: " & response.getServerMessage)
            End If
        Else
            Console.WriteLine("Not connected!")
        End If
    End Sub

    ''' <summary>
    ''' We will have a look at the CasparCGMedia classes here and how we can use them. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Test_CasparCGMedia_Classes()
        '' The CasparCGMedia classes are ment as objects, representing the media files on the casparCG server
        '' There are 5 subclasses: 
        ''      CasparCGMovie
        ''      CasparGAudio
        ''      CasparCGStill
        ''      CasparCGColor 
        ''      CasparCGTemplate
        '' In the simplest way, they are just a store for the name of a media. But they offer some very cool functions for your client, too.
        '' You can use them to store metadata. They are designed to hold the metadata from casparCG server, but you can add your own two.
        ''
        '' If you call Info for a layer with some media loaded or Info template for a specified template, you retrieve a xml string form the server.
        '' In this string you find a lot of usefull information.
        '' With the CasparCGMedia classes you can retrieve and save them automatically just by calling .fillMediaInfo.
        '' You have to specify the connection and the channel to use for the information retrieval. This is important,
        '' as you can use the same CasparCGMedia with a lot of different servers and connections. Beside that, retrieving information
        '' of media other than templates requires to load the clip/still or audio at least to the background. If you're using this function
        '' with in a client, it should not work on a channel that is live to minimize the chance of an accidently collision with the live playout.
        ''
        '' Ok, enought words, let's code something:
        '' We will define a CasparCGMedia with a Movie
        '' retrieve the information of it,
        '' set a thumbnail if possible,
        '' add some custom metadata
        '' add show everthing we know about it

        '' Define new media with name amb
        Console.WriteLine("Creating media..")
        Dim media As AbstractCasparCGMedia = New CasparCGMovie("amb")

        '' Fill the informations on channel 1 - the layer will automatically be choosen to be the first free layer.
        Console.WriteLine("Filling media...")
        media.fillMediaInfo(connection, 1)

        '' Add the thumbnail if it is supported by the server (> 2.0.4)
        Console.WriteLine("Retrieving thumbnail...")
        Dim cmd As New ThumbnailRetrieveCommand(media)

        '' Check if the command is runnable with our connection
        '' We don't need to do this, the execute command will check anyway,
        '' but so we can print a nice message ;-)
        If cmd.isCompatible(connection) Then
            Console.WriteLine("Thumbs are supported by the server.")
            cmd.execute(connection)
            media.setBase64Thumb(cmd.getResponse.getData)
        Else
            Console.WriteLine("Sorry bro, no thumbs on that server :-(")
        End If
        '' there is also a handy function for that task:
        ' media.fillThumbnail(connection) 


        '' Add custom metadata
        Console.WriteLine("Add custom metadata...")
        media.addInfo("isFavorite", "yes")
        media.addInfo("Played number of times", "0")

        '' Assume we would play the media now...
        '' ...
        '' Then we would increment the Played number of times metadata
        Console.WriteLine("Set custom metadata...")
        media.setInfo("Played number of times", Integer.Parse(media.getInfo("Played number of times")) + 1)


        '' show everything we know about the media
        '' this could be done with media.toString() too,
        '' but I wanted to show the members of media
        Console.WriteLine("Media name: " & media.getName)
        Console.WriteLine("Media type: " & media.getMediaType.ToString)
        Console.WriteLine("Media full name (with path): " & media.getFullName)
        Console.WriteLine("Media path: " & media.getPath)
        Console.WriteLine("Media has thumbnail: " & (media.getBase64Thumb.Length > 0).ToString)
        Console.WriteLine("Metadata of media:")
        For Each info As String In media.getInfos.Keys
            Console.WriteLine(vbTab & info & ": " & media.getInfo(info))
        Next

    End Sub

End Module
