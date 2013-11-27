CasparCGNetConnector
====================

A DotNet Connector for [CasparCG Server][1]


Table of contents
-----------------

* [License](#license)
* [Features](#features)
* [Quick Start](#quick-start)
* [Development](#development)



License
-------

This software is licensed under the [Gnu GPL v3] [2].



Features
--------

This connector framework was written for [CasparsPlayoutClient][3] and offers the follwing features:  
  * easy connection
  * duplex communication with the [CasparCG Server][1]
  * built in [CasparCG Server][1] command classes 
    * with version control
    * easy runtime exploreation of commands and their paramters - perfect for UI building
    * command and parameter discriptions
  * ready to use media objects for OO usage
  * plaintext command sending


Quick Start
-----------

See the [Wiki][4] for a more detailed and complete Guide.

Add CasparCGNetConnector to your project and import the namespace where needed
<pre>import CasparCGNetConnector</pre>

Now you need a CasparCGNetConnector.CasparCGConnection object
<pre>Dim con as new CasparCGConnection(server,port)</pre>
where *server* is the IP or DNS name of the [CasparCG Server][1] and *port* the TCP port nummber.  

For sending commands to the [CasparCG Server][1], there are two possible ways:
  * creating a CasparCGNetConnector.Command object and call it's execute method
    <pre>
Dim cmd as new PlayCommand(channel,layer,medianame)
cmd.execute(con)</pre>
  * send a plaintext command
    <pre>
con.sendCommand("play 1-1 amb")</pre>  

If you are interrested in the [CasparCG Server][1] response, you can check the CasparCGResponse.
Both, the ***execute()*** and the ***sendCommand()*** method is returning a CasparCGResponse object holding:
  * the [CasparCG Server][1] response code
    <pre>response.getCode() as integer</pre>
  * the received data as string
    <pre>response.getData()</pre>
  * the received data as xml (if any)
    <pre>response.getXML()</pre>
  * the executed command as string
    <pre>response.getCommand()</pre>
  * the raw server message as it was received
    <pre>response.getServerMessage()</pre>
  * the response status
    <pre>response.isOK()
response.isERR()
response.isINFO()
response.isUNKNOWN()</pre>  

This should be enough to give you a good start.
There are more functions to explore.
Just check out the code.


Development
-----------

CasparCGNetConnector is still under development and has reached an alpha state now.
This means you can use it and I will provide compiled dlls, but the chances are given that the interface of the classes change over the releases. With the first beta release, interface changes will only occur with big version steps.
Any contributions are welcome. 
The first development phase was under high time pressure, so some parts of the code are not or only hardly documented so far. But it's getting better ;-)

[1]: https://github.com/CasparCG/Server "CasparCG Server"
[2]: http://www.gnu.org/licenses/gpl-3.0-standalone.html "Gnu General Public License Version 3"
[3]: https://github.com/mcdikki/CasparsPlayoutClient "CasparsPlayoutClient"
[4]: wiki/home "CasparCGNetConnector Wiki"
