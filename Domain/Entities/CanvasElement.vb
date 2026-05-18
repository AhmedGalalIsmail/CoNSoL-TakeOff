Option Strict On

Imports Domain.Entities

Namespace Entities
    Public Class CanvasElement
        Public Property ParentElementId As String
        Public Property Id As Guid = Guid.NewGuid()
        Public Property Type As String
        Public Property Layer As String
        Public Property GeometryJson As String
        Public Property BusinessJson As String
        Public Property RelationshipType As ElementRelationshipType
        Public Property ChildElementId As String
    End Class
End Namespace