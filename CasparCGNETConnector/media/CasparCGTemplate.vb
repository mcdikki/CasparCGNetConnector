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

    Public Overrides Function clone() As ICasparCGMedia
        Dim media As New CasparCGTemplate(FullName)
        For Each info In Infos
            media.addInfo(info.Key, info.Value)
        Next
        For Each comp As CasparCGTemplateComponent In components.Values
            media.addComponent(comp)
        Next
        media.data = getData.clone
        Return media
    End Function

    Public Overrides ReadOnly Property MediaType As ICasparCGMedia.MediaTypes
        Get
            Return ICasparCGMedia.MediaTypes.TEMPLATE
        End Get
    End Property

    Public Overrides Sub parseXML(ByVal xml As String)
        Dim configDoc As New Xml.XmlDocument
        configDoc.LoadXml(xml)
        If configDoc.HasChildNodes Then
            '' Attribute verarbeiten
            For Each attrib As Xml.XmlAttribute In configDoc.SelectSingleNode("template").Attributes
                setInfo(attrib.Name, attrib.Value)
            Next

            '' Components verarbeiten
            For Each comp As Xml.XmlNode In configDoc.GetElementsByTagName("component")
                addComponent(New CasparCGTemplateComponent(comp.OuterXml))
            Next

            '' Instances
            For Each instance As Xml.XmlNode In configDoc.GetElementsByTagName("instance")
                If Not IsDBNull(instance.Attributes.GetNamedItem("type")) AndAlso Not IsDBNull(instance.Attributes.GetNamedItem("name")) AndAlso containsComponent(instance.Attributes.GetNamedItem("type").FirstChild.Value) Then
                    Dim c As CasparCGTemplateComponent = getComponent(instance.Attributes.GetNamedItem("type").FirstChild.Value)
                    Dim i As New CasparCGTemplateInstance(instance.Attributes.GetNamedItem("name").FirstChild.Value, c)
                    data.addInstance(i)
                End If
            Next
        End If
    End Sub

    Public Overrides Sub fillMediaInfo(ByRef connection As ICasparCGConnection, Optional channel As Integer = 1)
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

    Public Function hasData() As Boolean
        Return getData.hasData
    End Function

    Public Function getData() As CasparCGTemplateData
        Return data
    End Function

    Public Function getDataString() As String
        Return data.getDataString
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
                out = out & vbNewLine & vbTab & "Property Name: '" & prop.Name & "' Type: '" & prop.Type & "' Desc: '" & prop.Info & "'"
                out = out & vbNewLine & vbTab & instance.getName '& " = " & instance.getData(prop)
            Next
        Next
        Return out
    End Function

    Public Overrides Function toXml() As Xml.XmlDocument
        Dim domDoc As Xml.XmlDocument = MyBase.toXml
        Dim pnode As Xml.XmlNode = domDoc.FirstChild
        Dim node As Xml.XmlElement = domDoc.createElement("template")
        pnode.removeChild(pnode.selectSingleNode("infos"))

        ' Add attributes to template tag
        For Each info In Infos
            node.setAttribute(info.Key, info.Value)
        Next

        ' add components
        Dim cnode = domDoc.createElement("components")
        For Each comp In getComponents()
            cnode.AppendChild(domDoc.ImportNode(comp.toXML.FirstChild, False))
        Next
        node.appendChild(cnode)

        ' add instances
        Dim inode = domDoc.createElement("instances")
        For Each inst In data.getInstances
            inode.AppendChild(domDoc.ImportNode(inst.toXML.FirstChild, False))
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
        If Not IsNothing(instance) AndAlso Not containsInstance(instance.getName) Then
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

    Public Function containsInstance(ByVal instanceName As String) As Boolean
        Return instances.ContainsKey(instanceName)
    End Function

    Public Function hasData() As Boolean
        For Each i In getInstances()
            If i.hasData Then Return True
        Next
        Return False
    End Function

    Public Function toDataXML() As Xml.XmlDocument
        Dim domDoc As New Xml.XmlDocument
        domDoc.AppendChild(domDoc.CreateElement("templateData"))

        For Each instance As CasparCGTemplateInstance In instances.Values
            domDoc.FirstChild.AppendChild(domDoc.ImportNode(instance.toDataXML.FirstChild, True))
        Next

        Return domDoc
    End Function

    Public Function getDataString() As String
        Return toDataXML.OuterXml
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
        Dim configDoc As New Xml.XmlDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            name = configDoc.FirstChild.Attributes.GetNamedItem("name").FirstChild.Value
            For Each prop As Xml.XmlNode In configDoc.getElementsByTagName("property")
                addProperty(New CasparCGTemplateComponentProperty(prop.OuterXml))
            Next
        End If
    End Sub

    Public Sub addProperty(ByRef componentProperty As CasparCGTemplateComponentProperty)
        If Not properties.ContainsKey(componentProperty.Name) AndAlso Not properties.ContainsValue(componentProperty) Then
            properties.Add(componentProperty.Name, componentProperty)
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

    Public Function toXML() As Xml.XmlDocument
        Dim domDoc As New Xml.XmlDocument
        Dim pnode As Xml.XmlElement = domDoc.createElement("component")
        pnode.setAttribute("name", name)

        For Each prop As CasparCGTemplateComponentProperty In properties.Values
            pnode.AppendChild(domDoc.ImportNode(prop.toXML.FirstChild, False))
        Next
        domDoc.appendChild(pnode)

        Return domDoc
    End Function

End Class

Public Class CasparCGTemplateComponentProperty

    Public Property Name As String = ""
    Public Property Type As String = "none"
    Public Property Info As String = "This property is not initialized"
    Public Property Value As String = Nothing

    Public Sub New(ByVal name As String, ByVal type As String, ByVal info As String, Optional ByVal value As String = Nothing)
        Me.Name = name
        Me.Type = type
        Me.Info = info
        Me.Value = value
    End Sub

    Public Sub New(ByVal xml As String)
        Dim configDoc As New Xml.XmlDocument
        configDoc.loadXML(xml)
        If configDoc.hasChildNodes Then
            If Not IsNothing(configDoc.selectSingleNode("property")) AndAlso Not IsDBNull(configDoc.selectSingleNode("property")) Then
                Name = configDoc.SelectSingleNode("property").Attributes.GetNamedItem("name").FirstChild.Value
                Type = configDoc.SelectSingleNode("property").Attributes.GetNamedItem("type").FirstChild.Value
                Info = configDoc.SelectSingleNode("property").Attributes.GetNamedItem("info").FirstChild.Value
            End If
        End If
    End Sub

    Public Function isSet() As Boolean
        Return Not IsNothing(Value)
    End Function

    Public Function toXML() As Xml.XmlDocument
        Dim domDoc As New Xml.XmlDocument
        Dim pnode As Xml.XmlElement = domDoc.CreateElement("property")
        pnode.SetAttribute("name", Name)
        pnode.SetAttribute("type", Type)
        pnode.SetAttribute("info", Info)
        domDoc.appendChild(pnode)
        Return domDoc
    End Function

    Public Function toDataXML() As Xml.XmlDocument
        Dim domDoc As New Xml.XmlDocument
        If Not IsNothing(Value) Then
            Dim pnode As Xml.XmlElement = domDoc.CreateElement("data")
            pnode.SetAttribute("id", Name)
            pnode.SetAttribute("value", Value)
            domDoc.AppendChild(pnode)
        End If
        Return domDoc
    End Function

End Class



Public Class CasparCGTemplateInstance
    '' Ist eine Instance einer CasparCG Componente in einem Template und beinhaltet den namen, die Componente und die Daten
    Private name As String
    Private component As CasparCGTemplateComponent
    Private properties As Dictionary(Of String, CasparCGTemplateComponentProperty)

    Public Sub New(ByVal name As String, ByVal component As CasparCGTemplateComponent)
        Me.name = name
        Me.component = component
        Me.properties = New Dictionary(Of String, CasparCGTemplateComponentProperty)
        For Each p In component.getProperties
            properties.Add(p.Name, New CasparCGTemplateComponentProperty(p.toXML.FirstChild.OuterXml))
        Next
    End Sub

    Public Function clone() As CasparCGTemplateInstance
        Dim instance As New CasparCGTemplateInstance(name, component)
        Return instance
    End Function

    Public Function getComponent() As CasparCGTemplateComponent
        Return component
    End Function

    Public Function getProperties() As IEnumerable(Of CasparCGTemplateComponentProperty)
        Return properties.Values
    End Function

    Public Function getProperty(ByVal propertyName As String) As CasparCGTemplateComponentProperty
        If properties.ContainsKey(propertyName) Then
            Return properties.Item(propertyName)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Sets the given property to the given value and returns true.
    ''' If no such property exists in this instance, nothing will be set false returned.
    ''' </summary>
    ''' <param name="propertyName"></param>
    ''' <param name="value"></param>
    ''' <returns>true if, and only if, the property exists
    ''' false otherwise</returns>
    Public Function setValue(ByVal propertyName As String, ByVal value As String) As Boolean
        If properties.ContainsKey(propertyName) Then
            properties.Item(propertyName).Value = value
            Return True
        End If
        Return False
    End Function

    Public Function getName() As String
        Return name
    End Function

    Public Function hasData() As Boolean
        For Each p In getProperties()
            If p.isSet Then Return True
        Next
        Return False
    End Function

    Public Function toDataXML() As Xml.XmlDocument
        Dim domDoc As New Xml.XmlDocument
        Dim pnode As Xml.XmlElement = domDoc.CreateElement("componentData")
        pnode.SetAttribute("id", getName)

        For Each prop In getProperties()
            pnode.AppendChild(domDoc.ImportNode(prop.toDataXML.FirstChild, True))
        Next
        domDoc.AppendChild(pnode)

        Return domDoc
    End Function

    Public Function toXML() As Xml.XmlDocument
        Dim domDoc As New Xml.XmlDocument
        Dim pnode As Xml.XmlElement = domDoc.CreateElement("instance")
        pnode.setAttribute("name", getName)
        pnode.setAttribute("type", getComponent.getName)
        domDoc.appendChild(pnode)

        Return domDoc
    End Function
End Class