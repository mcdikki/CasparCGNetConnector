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

<Serializable()> _
Public Class CasparCGTemplate
    Inherits AbstractCasparCGMedia

    Private components As Dictionary(Of String, CasparCGTemplateComponent)
    Private data As CasparCGTemplateData

    Public Sub New(ByVal name As String)
        MyBase.New(name)
        data = New CasparCGTemplateData
        components = New Dictionary(Of String, CasparCGTemplateComponent)
    End Sub

    Public Sub New(ByVal name As String, ByVal xml As String)
        MyBase.New(name)
        data = New CasparCGTemplateData
        components = New Dictionary(Of String, CasparCGTemplateComponent)
        parseXML(xml)
    End Sub

    Public Overrides Function clone() As AbstractCasparCGMedia
        Dim media As New CasparCGTemplate(getFullName)
        For Each info As String In getInfos.Keys
            media.addInfo(info, getInfo(info))
        Next
        For Each comp As CasparCGTemplateComponent In components.Values
            media.addComponent(comp)
        Next
        media.data = getData.clone
        Return media
    End Function

    Public Overrides Function getMediaType() As AbstractCasparCGMedia.MediaType
        Return MediaType.TEMPLATE
    End Function

    Public Overrides Sub parseXML(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            '' Attribute verarbeiten
            For Each attrib As MSXML2.IXMLDOMNode In configDoc.selectSingleNode("template").attributes
                setInfo(attrib.nodeName, attrib.nodeTypedValue)
            Next

            '' Components verarbeiten
            For Each comp As MSXML2.IXMLDOMNode In configDoc.getElementsByTagName("component")
                addComponent(New CasparCGTemplateComponent(comp.xml))
            Next

            '' Instances
            For Each instance As MSXML2.IXMLDOMNode In configDoc.getElementsByTagName("instance")
                If Not IsDBNull(instance.attributes.getNamedItem("type")) AndAlso Not IsDBNull(instance.attributes.getNamedItem("name")) AndAlso containsComponent(instance.attributes.getNamedItem("type").nodeTypedValue) Then
                    Dim c As CasparCGTemplateComponent = getComponent(instance.attributes.getNamedItem("type").nodeTypedValue)
                    Dim i As New CasparCGTemplateInstance(instance.attributes.getNamedItem("name").nodeTypedValue, c)
                    data.addInstance(i)
                End If
            Next
        End If
    End Sub

    Public Overrides Sub fillMediaInfo(ByRef connection As CasparCGConnection, Optional channel As Integer = 1)
        If connection.isConnected Then
            Dim info As New InfoTemplateCommand(Me)
            If info.execute(connection).isOK Then
                parseXML(info.getResponse.getXMLData)
            Else
                logger.err("CasparCGTemplate.fillMediaInfo: Error loading xml data received from server for " & toString())
                logger.err("CasparCGTemplate.fillMediaInfo: ServerMessage dump: " & info.getResponse.getServerMessage)
            End If
        End If
    End Sub

    Private Sub addComponent(ByRef component As CasparCGTemplateComponent)
        If Not components.ContainsValue(component) Then
            components.Add(component.getName, component)
        End If
    End Sub

    Public Function getData() As CasparCGTemplateData
        Return data
    End Function

    Public Function getComponents() As IEnumerable(Of CasparCGTemplateComponent)
        Return components.Values
    End Function

    Public Function getComponent(ByVal componentName As String) As CasparCGTemplateComponent
        If components.ContainsKey(componentName) Then
            Return components.Item(componentName)
        Else : Return Nothing
        End If
    End Function

    Public Function containsComponent(ByVal compnentName As String) As Boolean
        Return components.ContainsKey(compnentName)
    End Function

    Public Overrides Function toString() As String
        Dim out As String = MyBase.toString & vbNewLine & "Components:"
        For Each comp In getComponents()
            out = out & vbNewLine & vbTab & comp.getName
        Next
        out = out & vbNewLine & "Instances and their properties:"
        For Each instance In data.getInstances
            For Each prop In instance.getComponent.getProperties
                out = out & vbNewLine & vbTab & "Property Name: '" & prop.propertyName & "' Type: '" & prop.propertyType & "' Desc: '" & prop.propertyInfo & "'"
                out = out & vbNewLine & vbTab & instance.getName & " = " & instance.getData(prop)
            Next
        Next
        Return out
    End Function

    Public Overrides Function toXml() As MSXML2.DOMDocument
        Dim domDoc As MSXML2.DOMDocument = MyBase.toXml
        Dim pnode As MSXML2.IXMLDOMNode = domDoc.firstChild
        Dim node As MSXML2.IXMLDOMElement = domDoc.createElement("template")
        pnode.removeChild(pnode.selectSingleNode("infos"))

        ' Add attributes to template tag
        For Each info As String In getInfos.Keys
            node.setAttribute(info, getInfo(info))
        Next

        ' add components
        Dim cnode = domDoc.createElement("components")
        For Each comp In getComponents()
            cnode.appendChild(comp.toXML.firstChild)
        Next
        node.appendChild(cnode)

        ' add instances
        Dim inode = domDoc.createElement("instances")
        For Each inst In data.getInstances
            inode.appendChild(inst.toXML.firstChild)
        Next
        node.appendChild(inode)

        pnode.appendChild(node)
        Return domDoc
    End Function

End Class

Public Class CasparCGTemplateData

    '' Beinhaltet alle Daten für das Template
    '' also seine Component Instances
    Private instances As Dictionary(Of String, CasparCGTemplateInstance)

    Public Sub New()
        instances = New Dictionary(Of String, CasparCGTemplateInstance)
    End Sub

    Public Sub New(ByRef instances As IEnumerable(Of CasparCGTemplateInstance))
        instances = New Dictionary(Of String, CasparCGTemplateInstance)
        For Each instance As CasparCGTemplateInstance In instances
            addInstance(instance)
        Next
    End Sub

    Public Function clone() As CasparCGTemplateData
        Dim data As New CasparCGTemplateData()
        For Each instance In instances.Values
            data.addInstance(instance.clone)
        Next
        Return data
    End Function

    Public Sub addInstance(ByRef instance As CasparCGTemplateInstance)
        If Not IsNothing(instance) AndAlso Not contains(instance.getName) Then
            instances.Add(instance.getName, instance)
        End If
    End Sub

    Public Function getInstance(ByVal instanceName As String) As CasparCGTemplateInstance
        If instances.ContainsKey(instanceName) Then
            Return instances.Item(instanceName)
        Else
            Return Nothing
        End If
    End Function

    Public Function getInstances() As IEnumerable(Of CasparCGTemplateInstance)
        Return instances.Values
    End Function

    Public Function contains(ByVal instanceName As String) As Boolean
        Return instances.ContainsKey(instanceName)
    End Function

    Public Function toXML() As MSXML2.DOMDocument
        Dim domDoc As New MSXML2.DOMDocument
        domDoc.appendChild(domDoc.createElement("templateData"))

        For Each instance As CasparCGTemplateInstance In instances.Values
            domDoc.firstChild.appendChild(instance.toXML.firstChild)
        Next 

        Return domDoc
    End Function

End Class

Public Class CasparCGTemplateComponent
    '' Beinhaltet alle Eigenschaften einer CasparCG Template Componente 
    Private name As String
    Private properties As Dictionary(Of String, CasparCGTemplateComponentProperty)

    Public Sub New(ByVal xml As String)
        properties = New Dictionary(Of String, CasparCGTemplateComponentProperty)
        parseXML(xml)
    End Sub

    Private Sub parseXML(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            name = configDoc.firstChild.attributes.getNamedItem("name").nodeTypedValue
            For Each prop As MSXML2.IXMLDOMNode In configDoc.getElementsByTagName("property")
                addProperty(New CasparCGTemplateComponentProperty(prop.xml))
            Next
        End If
    End Sub

    Public Sub addProperty(ByRef componentProperty As CasparCGTemplateComponentProperty)
        If Not properties.ContainsKey(componentProperty.propertyName) AndAlso Not properties.ContainsValue(componentProperty) Then
            properties.Add(componentProperty.propertyName, componentProperty)
        End If
    End Sub

    Public Function getName() As String
        Return name
    End Function

    Public Function getProperties() As IEnumerable(Of CasparCGTemplateComponentProperty)
        Return properties.Values
    End Function

    Public Function getProperty(ByVal name As String) As CasparCGTemplateComponentProperty
        If properties.ContainsKey(name) Then
            Return properties.Item(name)
        Else
            Return Nothing
        End If
    End Function

    Public Function containsProperty(ByVal name As String) As Boolean
        Return properties.ContainsKey(name)
    End Function

    Public Function toXML() As MSXML2.DOMDocument
        Dim domDoc As New MSXML2.DOMDocument
        Dim pnode As MSXML2.IXMLDOMElement = domDoc.createElement("component")
        pnode.setAttribute("name", name)

        For Each prop As CasparCGTemplateComponentProperty In properties.Values
            pnode.appendChild(prop.toXML.firstChild)
        Next
        domDoc.appendChild(pnode)

        Return domDoc
    End Function

End Class

Public Class CasparCGTemplateComponentProperty

    Public Property propertyName As String = ""
    Public Property propertyType As String = "none"
    Public Property propertyInfo As String = "This property is not initialized"

    Public Sub New(ByVal name As String, ByVal type As String, ByVal info As String)
        propertyName = name
        propertyType = type
        propertyInfo = info
    End Sub

    Public Sub New(ByVal xml As String)
        Dim configDoc As New MSXML2.DOMDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            If Not IsNothing(configDoc.selectSingleNode("property")) AndAlso Not IsDBNull(configDoc.selectSingleNode("property")) Then
                propertyName = configDoc.selectSingleNode("property").attributes.getNamedItem("name").nodeTypedValue
                propertyType = configDoc.selectSingleNode("property").attributes.getNamedItem("type").nodeTypedValue
                propertyInfo = configDoc.selectSingleNode("property").attributes.getNamedItem("info").nodeTypedValue
            End If
        End If
    End Sub

    Public Function toXML() As MSXML2.DOMDocument
        Dim domDoc As New MSXML2.DOMDocument
        Dim pnode As MSXML2.IXMLDOMElement = domDoc.createElement("property")
        pnode.setAttribute("name", propertyName)
        pnode.setAttribute("type", propertyType)
        pnode.setAttribute("info", propertyInfo)
        domDoc.appendChild(pnode)
        Return domDoc
        'Return "<property name='" & propertyName & "' type='" & propertyType & "' info='" & propertyInfo & "'/>"
    End Function

End Class

Public Class CasparCGTemplateInstance
    '' Ist eine Instance einer CasparCG Componente in einem Template und beinhaltet den namen, die Componente und die Daten
    Private name As String
    Private component As CasparCGTemplateComponent
    Private values As Dictionary(Of CasparCGTemplateComponentProperty, String)

    Public Sub New(ByVal name As String, ByRef component As CasparCGTemplateComponent)
        Me.name = name
        Me.component = component
        values = New Dictionary(Of CasparCGTemplateComponentProperty, String)
        For Each prop As CasparCGTemplateComponentProperty In component.getProperties
            values.Add(prop, "")
        Next
    End Sub

    Public Function clone() As CasparCGTemplateInstance
        Dim instance As New CasparCGTemplateInstance(name, component)
        For Each value In values.Keys
            instance.setData(value, getData(value))
        Next
        Return instance
    End Function

    Public Sub setData(ByRef componentProperty As CasparCGTemplateComponentProperty, ByVal value As String)
        If values.ContainsKey(componentProperty) Then
            values.Item(componentProperty) = value
        End If
    End Sub

    Public Function getData(ByRef componentProperty As CasparCGTemplateComponentProperty) As String
        If values.ContainsKey(componentProperty) Then
            Return values.Item(componentProperty)
        Else
            Return ""
        End If
    End Function

    Public Function getComponent() As CasparCGTemplateComponent
        Return component
    End Function

    Public Function getName() As String
        Return name
    End Function

    Public Function toXML() As MSXML2.DOMDocument
        Dim domDoc As New MSXML2.DOMDocument
        Dim pnode As MSXML2.IXMLDOMElement = domDoc.createElement("instance")
        pnode.setAttribute("name", getName)
        pnode.setAttribute("type", getComponent.getName)
        domDoc.appendChild(pnode)

        Return domDoc
    End Function

    'Public Function toXML() As MSXML2.DOMDocument
    '    Dim domDoc As New MSXML2.DOMDocument
    '    Dim pnode As MSXML2.IXMLDOMElement = domDoc.createElement("componentData")
    '    pnode.setAttribute("id", getName)

    '    Dim node As MSXML2.IXMLDOMElement
    '    'Dim xml As String = "<componentData id='" & getName() & "'>"
    '    For Each prop As CasparCGTemplateComponentProperty In values.Keys
    '        node = domDoc.createElement("data")
    '        node.setAttribute("id", prop.propertyName)
    '        node.setAttribute("value", values.Item(prop))
    '        pnode.appendChild(node)
    '        'xml = xml & "<data id='" & prop.propertyName & "' value='" & values.Item(prop) & "'>"
    '    Next
    '    domDoc.appendChild(pnode)

    '    Return domDoc
    '    'Return xml & "</componentData>"
    'End Function
End Class