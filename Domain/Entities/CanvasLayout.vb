Option Strict On

Namespace Entities
    Public Class CanvasLayout
        Public Property CanvasId As Guid = Guid.NewGuid()
        Public Property Unit As String = "meter"
        Public Property ScaleFactor As Double = 1.0
        Public Property Elements As New List(Of CanvasElement)()
        Public Property Relationships As New List(Of CanvasElement)()
    End Class
End Namespace