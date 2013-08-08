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
Public Class Example

    Private connection As CasparCGConnection

    Public Sub New()
        connection = New CasparCGConnection("localhost", 5250)

    End Sub


    Public Sub start()
        Test_Manual_Command()
        Test_Automatic_Command()
        Test_Plaintext_Commands()
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
        Dim command As ICommand = New PlayCommand(1, 1, "amb", True) '' I used ICommand as Type, so I can reuse command for all other commands

        '' Now that we have the command ready, we can execute it.
        '' to execute the command, we need the connection to the server via which
        '' we will execute it.

        '' Connect
        connection.connect()

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
        For Each param In command.getParameterNames
            Console.WriteLine(param & " of type " & command.getParameter(param).GetType.ToString & " is optional: " & command.getParameter(param).isOptional)
        Next

        '' Lets assume we know the parameter and their types, then we can cast directly and set them
        DirectCast(command.getParameter("channel"), CommandParameter(Of Integer)).setValue(1)
        DirectCast(command.getParameter("layer"), CommandParameter(Of Integer)).setValue(1)
        DirectCast(command.getParameter("only foreground"), CommandParameter(Of Boolean)).setValue(True)


        '' The command is ready to be executed
        '' This time, we check the response directly from the execute return
        If command.execute(connection).isOK Then
            '' Info returns XML data, so we can use getXMLData to see them
            Console.WriteLine(command.getResponse.getXMLData)
        End If

        '' At last, we're going to stop the looped playback of amb
        command = New StopCommand(1, 1)
        command.execute(connection)

        '' Closing the connection
        connection.close()
    End Sub


    ''' <summary>
    ''' In this sub, we will see how to explore and use the commands and their parameters in code. This is usefull if you want to built an UI.
    ''' </summary>
    Public Sub Test_Automatic_Command()
        '' We will list all commands understood by this library,
        '' display them and their parameters.
        '' Then we create a random command and ask the user for the paramter inputs

        '' List all commands and their parameters
        Dim commands As New List(Of ICommand)
        For Each c In [Enum].GetValues(GetType(CasparCGCommandFactory.Command))
            Console.WriteLine("Found Command " & c.ToString)
            commands.Add(CasparCGCommandFactory.getCommand(c))
            For Each param In commands.Last.getParameterNames
                Console.WriteLine(param & " of type " & commands.Last.getParameter(param).GetType.ToString & " is optional: " & commands.Last.getParameter(param).isOptional)
            Next
        Next

        '' Pick a random command and fill the needed parameters by asking the user
        Dim cmd As ICommand = commands.Item(Math.Abs(Rnd() * commands.Count))
        Console.WriteLine("Picked " & cmd.getName & " as command. Please fill the following parameters.")

        '' Now comes the dirty part.
        '' we need to dynamically cast the parameter object and its value object
        For Each p In cmd.getParameterNames
            Dim parameter = CTypeDynamic(cmd.getParameter(p), cmd.getParameter(p).getGenericParameterType)
            If Not parameter.isOptional Then
                Dim value = CTypeDynamic(parameter.getDefault(), cmd.getParameter(p).getGenericType)
                value = CTypeDynamic(Console.ReadLine("Parameter: " & p & " (Describtion: " & parameter.getDescribtion & ")"), cmd.getParameter(p).getGenericType)
                parameter.setValue(value)
            End If
        Next

    End Sub


    ''' <summary>
    ''' In this method, we will look how to send plaintext commands like in old school style ;-)
    ''' </summary>
    Public Sub Test_Plaintext_Commands()
        '' For plaintext commands you can simply pass a string to the 
        '' CasparCGConnection.sendCommand() function.
        '' As with the execute() of the command classes, you will get 
        '' a CasparCGResponse in return.

        '' Connect and check if connected
        If connection.connect() Then
            '' When sending commands via sendCommand, you don't need to end with a new line,
            '' but you have to take a look at correct character escaping
            Dim cmd As String = "Play 1-1 amb"

            Dim response As CasparCGResponse = connection.sendCommand(cmd)
            If response.isOK Then
                Console.WriteLine("Yeah, everthing went well!")
            ElseIf response.isERR Then
                Console.WriteLine("Well, something went wrong...")
            ElseIf response.isUNKNOWN Then
                Console.WriteLine("Hmm, that is funny, let's see the whole server message: " & response.getServerMessage)
            End If
            connection.close()
        End If
    End Sub

End Class
