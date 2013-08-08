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

    Private connection As CasparCGConnection


    Public Sub Main()
        connection = New CasparCGConnection("localhost", 5250)
        connection.connect()
        Console.WriteLine("==================================")
        Console.WriteLine("=== Start Test_Manual_Command: ===")
        Console.WriteLine("==================================")
        Test_Manual_Command()
        Console.WriteLine()
        Console.WriteLine("=====================================")
        Console.WriteLine("=== Start Test_Automatic_Command: ===")
        Console.WriteLine("=====================================")
        Test_Automatic_Command()
        Console.WriteLine()
        Console.WriteLine("=====================================")
        Console.WriteLine("=== Start Test_Plaintext_Command: ===")
        Console.WriteLine("=====================================")
        Test_Plaintext_Command()
        connection.close()
        Console.WriteLine("Press any key to quit program") 
        Console.ReadKey()
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
        For Each param In command.getParameters
            Console.WriteLine(vbTab & param.getName & ":")
            Console.WriteLine(vbTab & vbTab & "type: " & param.getGenericType.ToString & vbNewLine & vbTab & vbTab & "is optional: " & param.isOptional)
        Next

        '' Lets assume we know the parameter and their types, then we can cast directly and set them
        DirectCast(command.getParameter("channel"), CommandParameter(Of Integer)).setValue(1)
        DirectCast(command.getParameter("layer"), CommandParameter(Of Integer)).setValue(1)
        DirectCast(command.getParameter("only foreground"), CommandParameter(Of Boolean)).setValue(True)


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
        Dim commands As New List(Of ICommand)
        For Each c In [Enum].GetValues(GetType(CasparCGCommandFactory.Command))
            Console.WriteLine("Found Command " & c.ToString)
            commands.Add(CasparCGCommandFactory.getCommand(c))
            For Each param In commands.Last.getParameters
                Console.WriteLine(vbTab & param.getName & ":")
                Console.WriteLine(vbTab & vbTab & "type: " & param.getGenericType.ToString & vbNewLine & vbTab & vbTab & "is optional: " & param.isOptional)
            Next
        Next

        '' Pick a random command and fill the needed parameters by asking the user
        Dim r As New System.Random(System.DateTime.Now.Millisecond)
        Dim cmd As ICommand = commands.Item(r.Next(0, commands.Count - 1))
        Console.WriteLine("Picked " & cmd.getName & " as command. " & vbNewLine & "Describtion: " & cmd.getDescribtion & vbNewLine & "Please fill the following parameters:")

        '' Now comes the dirty part.
        '' we need to dynamically cast the parameter object and its value object
        For Each p In cmd.getParameters
            Dim parameter = CTypeDynamic(p, p.getGenericParameterType)
            If Not parameter.isOptional Then
                Dim value = CTypeDynamic(parameter.getDefault(), p.getGenericType)
                Console.WriteLine("Parameter: " & p.getName & " (Describtion: " & p.getDescribtion & "): ")
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
            Console.WriteLine("Please type the command you want to send to the server:")
            Dim cmd As String = Console.ReadLine()

            Console.WriteLine("Try to excute plaintext command '" & cmd & "'")
            Dim response As CasparCGResponse = connection.sendCommand(cmd)
            If response.isOK Then
                Console.WriteLine("Yeah, everthing went well!")
            ElseIf response.isERR Then
                Console.WriteLine("Well, something went wrong... - What's the error code and describtion:" & vbNewLine & response.getCode & " = " & [Enum].GetName(GetType(CasparCGResponse.CasparReturnCode), response.getCode))
            ElseIf response.isUNKNOWN Then
                Console.WriteLine("Hmm, that is funny, let's see the whole server message: " & response.getServerMessage)
            End If
        Else
            Console.WriteLine("Not connected!")
        End If
    End Sub

End Module
