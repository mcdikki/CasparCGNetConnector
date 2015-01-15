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

Public Class MixerPerspectiveCommand
    Inherits AbstractCommand

    Public Sub New()
        MyBase.New("MIXER PERSPECTIVE", "Returns or modifies the corners of the perspective transformation for a layer.")
        InitParameter()
    End Sub

    Public Sub New(ByVal channel As Integer, ByVal layer As Integer, ByVal topLeftX As Single, ByVal topLeftY As Single, ByVal topRightX As Single, ByVal topRightY As Single, ByVal bottomRightX As Single, ByVal bottomRightY As Single, ByVal bottomLeftX As Single, ByVal bottomLeftY As Single)
        MyBase.New("MIXER PERSPECTIVE", "Returns or modifies the corners of the perspective transformation for a layer.")
        InitParameter()
        setChannel(channel)
        If layer > -1 Then setLayer(layer)
        setTopLeftX(topLeftX)
        setTopLeftY(topLeftY)
        setTopRightX(topRightX)
        setTopRightY(topRightY)
        setBottomLeftX(bottomLeftX)
        setBottomLeftY(bottomLeftY)
        setBottomRightX(bottomRightX)
        setBottomRightY(bottomRightY)
    End Sub

    Private Sub InitParameter()
        addCommandParameter(New CommandParameter(Of Integer)("channel", "The channel", 1, False))
        addCommandParameter(New CommandParameter(Of Integer)("layer", "The layer", 0, True))
        addCommandParameter(New CommandParameter(Of Single)("topLeftX", "The x pos. of the top left edge.", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("topLeftY", "The y pos. of the top left edge.", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("topRightX", "The x pos. of the top right edge.", 1, False))
        addCommandParameter(New CommandParameter(Of Single)("topRightY", "The y pos. of the top right edge.", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("bottomRightX", "The x pos. of the bottom right edge.", 1, False))
        addCommandParameter(New CommandParameter(Of Single)("bottomRightY", "The y pos. of the bottom right edge.", 1, False))
        addCommandParameter(New CommandParameter(Of Single)("bottomLeftX", "The x pos. of the bottom left edge.", 0, False))
        addCommandParameter(New CommandParameter(Of Single)("bottomLeftY", "The y pos. of the bottom left edge.", 1, False))
    End Sub

    Public Overrides Function getCommandString() As String
        Dim cmd As String = "MIXER " & getDestination(getCommandParameter("channel"), getCommandParameter("layer")) & " PERSPECTIVE"

        cmd = cmd & " " & getTopLeftX()
        cmd = cmd & " " & getTopLeftY()
        cmd = cmd & " " & getTopRightX()
        cmd = cmd & " " & getTopRightY()
        cmd = cmd & " " & getBottomRightX()
        cmd = cmd & " " & getBottomRightY()
        cmd = cmd & " " & getBottomLeftX()
        cmd = cmd & " " & getBottomLeftY()

        Return cmd
    End Function

    Public Sub setChannel(ByVal channel As Integer)
        If channel > 0 Then
            DirectCast(getCommandParameter("channel"), CommandParameter(Of Integer)).setValue(channel)
        Else
            Throw New ArgumentException("Illegal argument channel=" + channel + ". The parameter channel has to be greater than 0.")
        End If
    End Sub

    Public Function getChannel() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("channel")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setLayer(ByVal layer As Integer)
        If layer < 0 Then
            Throw New ArgumentException("Illegal argument layer=" + layer + ". The parameter layer has to be greater or equal than 0.")
        Else
            DirectCast(getCommandParameter("layer"), CommandParameter(Of Integer)).setValue(layer)
        End If
    End Sub

    Public Function getLayer() As Integer
        Dim param As CommandParameter(Of Integer) = getCommandParameter("layer")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setTopLeftX(ByVal x As Single)
        If IsNothing(x) Then
            DirectCast(getCommandParameter("topLeftX"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("topLeftX"), CommandParameter(Of Single)).setValue(x)
        End If
    End Sub

    Public Function getTopLeftX() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("topLeftX")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setTopLeftY(ByVal y As Single)
        If IsNothing(y) Then
            DirectCast(getCommandParameter("topLeftY"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("topLeftY"), CommandParameter(Of Single)).setValue(y)
        End If
    End Sub

    Public Function getTopLeftY() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("topLeftY")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setTopRightX(ByVal x As Single)
        If IsNothing(x) Then
            DirectCast(getCommandParameter("topRightX"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("topRightX"), CommandParameter(Of Single)).setValue(x)
        End If
    End Sub

    Public Function getTopRightX() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("topRightX")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setTopRightY(ByVal y As Single)
        If IsNothing(y) Then
            DirectCast(getCommandParameter("topRightY"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("topRightY"), CommandParameter(Of Single)).setValue(y)
        End If
    End Sub

    Public Function getTopRightY() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("topRightY")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function



    Public Sub setBottomLeftX(ByVal x As Single)
        If IsNothing(x) Then
            DirectCast(getCommandParameter("bottomLeftX"), CommandParameter(Of Single)).setValue(0)
        Else
            DirectCast(getCommandParameter("bottomLeftX"), CommandParameter(Of Single)).setValue(x)
        End If
    End Sub

    Public Function getBottomLeftX() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("bottomLeftX")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setBottomLeftY(ByVal y As Single)
        If IsNothing(y) Then
            DirectCast(getCommandParameter("bottomLeftY"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("bottomLeftY"), CommandParameter(Of Single)).setValue(y)
        End If
    End Sub

    Public Function getBottomLeftY() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("bottomLeftY")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setBottomRightX(ByVal x As Single)
        If IsNothing(x) Then
            DirectCast(getCommandParameter("bottomRightX"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("bottomRightX"), CommandParameter(Of Single)).setValue(x)
        End If
    End Sub

    Public Function getBottomRightX() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("bottomRightX")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Sub setBottomRightY(ByVal y As Single)
        If IsNothing(y) Then
            DirectCast(getCommandParameter("bottomRightY"), CommandParameter(Of Single)).setValue(1)
        Else
            DirectCast(getCommandParameter("topRightY"), CommandParameter(Of Single)).setValue(y)
        End If
    End Sub

    Public Function getBottomRightY() As Single
        Dim param As CommandParameter(Of Single) = getCommandParameter("bottomRightY")
        If Not IsNothing(param) And param.isSet Then
            Return param.getValue
        Else
            Return param.getDefault
        End If
    End Function

    Public Overrides Function getRequiredVersion() As Integer()
        Return {2, 0, 7}
    End Function

    Public Overrides Function getMaxAllowedVersion() As Integer()
        Return {Integer.MaxValue}
    End Function
End Class
