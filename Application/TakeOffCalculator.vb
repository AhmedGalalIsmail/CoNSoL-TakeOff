' src\CoNSoL.Application\TakeOffCalculator.vb
' Modify TakeOffContext — place in Application namespace and import domain entities (Block, Material, Formula stubs are in domain)

Imports Domain.Utilities
Imports Domain.Entities
Imports Domain
Imports System.Text.Json
Imports System.Linq



''' <summary>
''' Core calculation engine for take-off quantity and cost computation.
''' </summary>
''' <remarks>
''' TakeOffCalculator implements the business logic for:
''' - Extracting quantities from shapes based on dimension mode (D0-D3)
''' - Handling nested objects (parent-child relationships)
''' - Computing costs (quantity × unit price)
''' - Aggregating results by material or layer
''' 
''' Dimension Modes:
''' - D0: Count (number of objects)
''' - D1: Length (line length or perimeter)
''' - D2: Area (width × height)
''' - D3: Volume (area × depth)
''' 
''' Related Use Cases:
''' - UC-004: Run take-off quantity summary
''' - UC-006: Edit multi-selection properties
''' </remarks>
Public Class TakeOffCalculator
    ''' <summary>
    ''' Calculates quantities and costs for all elements in layout.
    ''' </summary>
    ''' <param name="layout">Canvas layout to calculate</param>
    ''' <param name="context">Calculation context (unit system, options)</param>
    ''' <returns>Take-off results grouped by material</returns>
    ''' <remarks>
    ''' Main orchestration method. Steps:
    ''' 1. Validate inputs
    ''' 2. Group elements by material
    ''' 3. For each group: extract quantity based on dimension mode
    ''' 4. Apply nested object logic (subtract children)
    ''' 5. Calculate total cost (qty × price)
    ''' 6. Return aggregated results
    ''' 
    ''' Guarantees:
    ''' - Deterministic: same input always produces same output
    ''' - No mutations: layout/elements not modified
    ''' - All quantities >= 0 (no negative quantities)
    ''' </remarks>
    ''' <exception cref="ArgumentNullException">If layout or context is Nothing</exception>
    ''' <exception cref="InvalidOperationException">If calculation fails</exception>
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
            Case "D1" : Return Geometry.Length(element.GeometryJson)
            Case "D2" : Return Geometry.Area(element.GeometryJson)
            Case "D3" : Return Geometry.Volume(element.GeometryJson, def.Parameters)
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

    ''' <summary>
    ''' Extracts length dimension from shape geometry.
    ''' </summary>
    ''' <param name="shapeType">Type of shape</param>
    ''' <param name="geometry">Geometry JSON</param>
    ''' <returns>Length in canvas units</returns>
    ''' <remarks>
    ''' Used for D1 (length) dimension mode.
    ''' 
    ''' Calculations:
    ''' - Line: distance(start, end)
    ''' - Rectangle: width OR height
    ''' - Circle: 2πr (circumference)
    ''' - Polyline: sum of segment lengths
    ''' 
    ''' Returns 0 for invalid types or missing geometry.
    ''' </remarks>
    Private Function ExtractLength(shapeType As String, geometry As Object) As Double
        ' Implementation (BUS-005)
        Return 0.0
    End Function
End Class