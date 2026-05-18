' src\ CoNSoL.Domain \ Entities \ BlockModels.vb
' New: simple block / material / formula stubs (domain)
Option Strict On

Namespace Entities 'BlockModels
    Public Class BlockModels
        ''' <summary>
        ''' Block Info.
        ''' </summary>
        Public Class Block
            Public Property Code As String
            Public Property Description As String
            Public Property DimensionMode As String ' D0/D1/D2/D3
            Public Property Components As New List(Of BlockComponent)

        End Class

        ''' <summary>
        ''' Material Info.
        ''' </summary>
        Public Class Material
            Public Property Code As String          ' e.g. CEMENT_BAG
            Public Property Unit As String          ' bag, piece, item
            Public Property Density As String       ' optional / descriptive for now
            Public Property PricePerUnit As Decimal ' cost per unit
        End Class

        ''' <summary>
        ''' Formula Info.
        ''' </summary>
        Public Class Formula
            Public Property Code As String
            Public Property Expression As String
        End Class
    End Class
End Namespace