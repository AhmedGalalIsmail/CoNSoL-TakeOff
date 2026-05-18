' src\CoNSoL.Application\TakeOffCalculator.vb
' Modify TakeOffContext — place in Application namespace and import domain entities (Block, Material, Formula stubs are in domain)

Imports Domain.Utilities
Imports Domain.Entities
Imports System.Text.Json
Imports Domain
Imports System.Linq



Public Class TakeOffCalculator
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="layout"></param>
    ''' <param name="ctx"></param>
    ''' <returns></returns>
    Public Function Calculate(
        layout As CanvasLayout,
        ctx As TakeOffContext
    ) As TakeOffResult

        Dim result As New TakeOffResult()

        For Each element In layout.Elements
            If String.IsNullOrWhiteSpace(element.BusinessJson) Then Continue For

            Dim def = JsonSerializer.Deserialize(Of BusinessDefinition)(element.BusinessJson)
            Dim qty = CalculateElementQuantity(element, def, GetRelationships(layout))
            'Dim breakdown = ApplyFormula(def, qty, ctx)

            Dim breakdown = ApplyFormula(def, qty, ctx)
            Dim total = breakdown.Values.Sum()
            result.Add(def.BlockCode, total)

            'result.Add(def.BlockCode, breakdown)
        Next
        Return result
    End Function

    Private Shared Function GetRelationships(layout As CanvasLayout) As Object
        Return layout.Relationships
    End Function

    '''<sammry>Calculate Element Quantity</sammry>
    Private Function CalculateElementQuantity(
    element As CanvasElement,
    def As BusinessDefinition,
    rels As IEnumerable(Of ElementRelationship)
) As Decimal
        Select Case def.DimensionMode
            Case "D0" : Return 1
            Case "D1" : Return Geometry1.Length(element.GeometryJson)
            Case "D2" : Return Geometry1.Area(element.GeometryJson)
            Case "D3" : Return Geometry1.Volume(element.GeometryJson, def.Parameters)
            Case Else : Throw New InvalidOperationException("Unknown dimension mode")
        End Select
    End Function

    ''' <summary>
    ''' Apply a formula to calculate breakdown costs/quantities from a quantity and context.
    ''' </summary>
    ''' <param name="def">The business definition containing formula references.</param>
    ''' <param name="qty">The calculated quantity.</param>
    ''' <param name="ctx">The takeoff context with material costs and pricing.</param>
    ''' <returns>A breakdown dictionary of costs by material.</returns>
    Private Shared Function ApplyFormula(def As BusinessDefinition, qty As Decimal, ctx As TakeOffContext) As Dictionary(Of String, Decimal)
        ' TODO: Implement formula application logic
        ' For now, return empty breakdown
        Return New Dictionary(Of String, Decimal)()
    End Function
End Class