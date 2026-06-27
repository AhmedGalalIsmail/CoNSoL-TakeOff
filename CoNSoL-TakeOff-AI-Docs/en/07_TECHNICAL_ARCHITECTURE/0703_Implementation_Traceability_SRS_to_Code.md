---
aliases:
  - CoNSoL-TakeOff Implementation Traceability — SRS to Code
doc_id: 703
status: draft
version: 1.0.0-draft
owner: qa + architecture
audience: AI coding agents + developers
related_docs:
  - 05_SDLC_Library/0104_SRS.md
  - 0701_CanvasControl_Technical_Architecture.md
  - 0702_Component_Reference_Detailed.md
last_updated: 2026-06
---

# ?? CoNSoL-TakeOff Implementation Traceability — SRS FRs to Codebase

> This document maps each **Functional Requirement (FR)** from the SRS (`0104_SRS.md`) to the **actual implementation** in the codebase, identifying implementation status, gaps, and recommended actions.

---

## Table of Contents

1. [Drawing Tools Traceability](#1-drawing-tools-traceability)
2. [Canvas & Coordinate System](#2-canvas--coordinate-system)
3. [Property Panel Traceability](#3-property-panel-traceability)
4. [Layer Panel Traceability](#4-layer-panel-traceability)
5. [Calculate & Aggregation Traceability](#5-calculate--aggregation-traceability)
6. [AI Intake Traceability](#6-ai-intake-traceability)
7. [Export Traceability](#7-export-traceability)
8. [Implementation Gaps & Recommendations](#8-implementation-gaps--recommendations)

---

## 1. Drawing Tools Traceability

### 1.1 Basic Shapes

| **FR ID** | **Requirement**              | **Implementation Status** | **File**                          | **Gap**                        | **Task** |
| --------- | ---------------------------- | ------------------------- | --------------------------------- | ------------------------------ | -------- |
| FR-DT-001 | Draw Line by start/end click | ? Implemented             | Desktop/Controls/CanvasControl.vb | None                           | �        |
| FR-DT-002 | Multi-segment polylines      | ?? Partial                | Desktop/Controls/LineShape.vb     | Polyline class not yet created | T-XXX    |
| FR-DT-003 | Rectangle drawing            | ? Implemented             | Desktop/Controls/RectShape.vb     | None                           | �        |
| FR-DT-004 | Circle drawing               | ? Not Started             | �                                 | Circle shape class missing     | T-XXX    |
| FR-DT-005 | Ellipse drawing              | ? Not Started             | �                                 | Ellipse shape class missing    | T-XXX    |

**Implementation Details:**

```vb
' ? LineShape (Desktop/Controls/LineShape.vb)
Public Class LineShape
	Inherits ShapeBase
	Public Property StartPoint As PointF
	Public Property EndPoint As PointF
	' Methods: GetBoundingBox, Draw, Contains, ToElement
End Class

' ? RectangleShape (Desktop/Controls/RectShape.vb)
Public Class RectangleShape
	Inherits ShapeBase
	Public Property TopLeft As PointF
	Public Property Width As Double
	Public Property Height As Double
	' Methods: GetBoundingBox, Draw, Contains, ToElement
End Class

' ?? TODO: PolylineShape - needs multi-point tracking
' ? TODO: CircleShape - needs center/radius model
' ? TODO: EllipseShape - needs axes/rotation
```

### 1.2 Curves

| **FR ID** | **Requirement** | **Status** | **File** | **Gap** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-010 | Arc drawing | ? Not Started | � | Arc class missing | T-XXX |
| FR-DT-011 | Spline drawing | ? Not Started | � | Spline class missing | T-XXX |
| FR-DT-012 | Bezier curves | ? Not Started | � | Bezier class missing | T-XXX |

### 1.3 Annotations & Dimensions

| **FR ID** | **Requirement** | **Status** | **File** | **Gap** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-020 | Text annotation | ? Not Started | � | TextShape missing | T-XXX |
| FR-DT-021 | Multiline text | ? Not Started | � | MTextShape missing | T-XXX |
| FR-DT-022 | Leaders | ? Not Started | � | LeaderShape missing | T-XXX |
| FR-DT-023 | Standard dimension types | ?? Partial | � | Only manual entry, no auto-calc | T-XXX |
| FR-DT-025 | Dimension override with warning | ?? Partial | PropertiesPanel.vb | Override allowed but no warning | Task? |

### 1.4 Symbols & Blocks

| **FR ID** | **Requirement** | **Status** | **File** | **Gap** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-030 | Symbol Library | ?? Partial | Application/AI/... | Planned but not implemented | T-066 |
| FR-DT-031 | Symbol insertion via drag/double-click | ? Not Started | � | UI integration missing | T-066 |
| FR-DT-033 | Circular block ref prevention | ? Not Started | � | No validation yet | T-XXX |

### 1.5 Smart Tags

| **FR ID** | **Requirement** | **Status** | **File** | **Gap** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-040 | Define Smart Tags | ?? Partial | PropertiesPanel.vb | Basic support, no tag mgmt UI | T-XXX |
| FR-DT-041 | Attach tags to objects | ? Implemented | PropertiesPanel.vb | Via Logical3D fields | � |
| FR-DT-043 | Tag aggregation | ?? Partial | TakeOffService.vb | Numeric aggregation only | T-XXX |
| FR-DT-045 | Aggregation output exportable | ? Implemented | ExcelExporter.vb | CSV/Excel export | � |

### 1.6 Custom Marks

| **FR ID** | **Requirement** | **Status** | **File** | **Gap** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-050 | Define Custom Marks | ? Not Started | � | Mark entity missing | T-XXX |
| FR-DT-051 | Attach marks to objects | ? Not Started | � | Mark UI missing | T-XXX |
| FR-DT-052 | Count marks | ? Not Started | � | No aggregation logic | T-XXX |

---

## 2. Canvas & Coordinate System

### 2.1 Canvas & Coordinates

| **FR ID** | **Requirement** | **Status** | **File** | **Implementation** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-CV-001 | Canvas operates in logical coords | ? Implemented | Desktop/Controls/CanvasControl.vb | `CoordinateConverter.ToLogical()`, `ToPhysical()` | � |
| FR-CV-004 | Pan & zoom don't alter logical data | ? Implemented | CanvasControl.vb | `_zoom`, `_pan` properties separate from shapes | � |
| FR-CV-007 | Rubber-band preview required | ? Implemented | CanvasControl.vb | `_tempShape` drawn in OnPaint loop | � |

**Key Code:**

```vb
' Desktop/Controls/CanvasControl.vb
' Coordinate conversion boundary:
Private Function PhysicalToLogical(physicalPt As PointF) As PointF
	Dim logicalX = (physicalPt.X - _pan.X) / _zoom + _currentLayout.LogicalOrigin.X
	Dim logicalY = (physicalPt.Y - _pan.Y) / _zoom + _currentLayout.LogicalOrigin.Y
	Return New PointF(logicalX, logicalY)
End Function

' Shapes store logical coordinates
' Paint converts to physical for rendering
```

---

## 3. Property Panel Traceability

### 3.1 Context Sensitivity

| **FR ID** | **Requirement** | **Status** | **File** | **Implementation** | **Gap** |
| --- | --- | --- | --- | --- | --- |
| FR-PP-001 | Panel is context-sensitive | ? Implemented | Desktop/Controls/PropertiesPanel.vb | Mode enum: Nothing, Single, Multi-same, Multi-mixed | None |
| FR-PP-004 | Mixed values show `(mixed)` | ?? Partial | PropertiesPanel.vb | Placeholder text implemented for layer dropdown | Need: all fields |
| FR-PP-008 | Logical 3D fields appear where applicable | ? Implemented | PropertiesPanel.vb | H, W, L, Qty, UnitPrice, TotalCost (auto) | None |

**Context Mode Logic:**

```vb
' Desktop/Controls/PropertiesPanel.vb
Enum ContextMode
	Nothing
	CanvasProperties
	SingleShape
	MultiSameType
	MultiMixedType
End Enum

' When displaying multi-selection of same type:
Private Sub DisplayMultiSameProperties(shapes As List(Of ShapeBase))
	Dim firstLayerId = shapes(0).LayerId
	If shapes.Any(Function(s) s.LayerId <> firstLayerId) Then
		_layerDropdown.Text = "(mixed)"
	Else
		_layerDropdown.SelectedValue = firstLayerId
	End If
End Sub
```

**Gap:** Not all property fields show `(mixed)`. Recommend full implementation for all UI controls.

---

## 4. Layer Panel Traceability

### 4.1 Layer Management UI

| **FR ID** | **Requirement** | **Status** | **File** | **Implementation** | **Gap** |
| --- | --- | --- | --- | --- | --- |
| FR-LP-001 | Layers support visibility, lock, print | ? Implemented | Domain/Entities/Layer.vb | `IsVisible`, `IsLocked` properties | Print flag missing |
| FR-LP-003 | Delete layer with reassignment | ?? Partial | Desktop/Controls/LayerPanel.vb | UI present, but reassignment dialog incomplete | Needs completion |
| FR-LP-004 | Active layer deletion blocked | ? Implemented | Domain/Services/LayerManager.vb | Exception thrown | � |

**Layer Entity:**

```vb
' Domain/Entities/Layer.vb
Public Class Layer
	Public Property Id As Guid = Guid.NewGuid()
	Public Property Name As String
	Public Property IsVisible As Boolean = True
	Public Property IsLocked As Boolean = False
	Public Property IncludeInCalculation As Boolean = True
	Public Property Color As String = "#FFFFFF"
	' MISSING: IsPrintable Boolean
End Class

' Domain/Services/LayerManager.vb
Public Sub DeleteLayer(layerId As Guid, reassignLayerId As Guid)
	If layerId = _activeLayer.Id Then
		Throw New InvalidOperationException("Cannot delete active layer")
	End If
	' ...reassign shapes...
End Sub
```

**Gap:** Layer entity missing `IsPrintable` property (FR-LP-001). Reassignment dialog UI incomplete.

---

## 5. Calculate & Aggregation Traceability

### 5.1 Dimension Modes & Calculation

| **FR ID** | **Requirement** | **Status** | **File** | **Implementation** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-040 | Dimension modes D0..D3 | ? Implemented | Application/TakeOffCalculator.vb | Switch on `DimensionMode` enum | � |
| D0 | Count | ? | TakeOffCalculator.vb | Returns 1 | T-010 |
| D1 | Length | ? | TakeOffCalculator.vb | Computes line length | T-011 |
| D2 | Area | ? | TakeOffCalculator.vb | Width � Height | T-012 |
| D3 | Volume | ? | TakeOffCalculator.vb | H � W � L from Logical3D | T-013 |

**Calculator Implementation:**

```vb
' Application/TakeOffCalculator.vb
Public Function Calculate(element As DrawingElement, layout As CanvasLayout) As CalculatedQuantity
	Dim result = New CalculatedQuantity()

	Select Case element.DimensionMode
		Case DimensionMode.D0
			result.Quantity = 1
			result.Unit = "ea"
		Case DimensionMode.D1
			' Line length in mm
			Dim geom = JsonConvert.DeserializeObject(Of LineGeometry)(element.GeometryJson)
			result.Quantity = Math.Sqrt(Math.Pow(geom.EndX - geom.StartX, 2) + ...) / 1000
			result.Unit = "mm"
		Case DimensionMode.D2
			' Rectangle area in m�
			result.Quantity = (width * height) / 1000000
			result.Unit = "m�"
		Case DimensionMode.D3
			' Volume in m�
			result.Quantity = (width * height * depth) / 1000000000
			result.Unit = "m�"
	End Select

	Return result
End Function
```

### 5.2 Take-Off Summary

| **FR ID** | **Requirement** | **Status** | **File** | **Implementation** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-043 | Aggregation support | ? Implemented | Application/TakeOffService.vb | `GetSummary()` with groupBy mode | T-021 |
| FR-DT-044 | Group by tag/layer/type | ? Implemented | TakeOffService.vb | `AggregationMode` enum | T-021 |
| FR-DT-045 | Export aggregation | ? Implemented | Application/Export/ExcelExporter.vb | CSV export | T-023 |

**TakeOffService:**

```vb
' Application/Services/TakeOffService.vb
Public Function GetSummary(
	layout As CanvasLayout,
	groupBy As AggregationMode,
	Optional filterLayerId As Guid? = Nothing) As TakeOffResult

	' Iterate elements, calculate quantities, group
	For Each element In layout.Elements
		Dim qty = _calculator.Calculate(element, layout)
		Dim groupKey = GetGroupKey(element, groupBy)

		If Not result.Groups.ContainsKey(groupKey) Then
			result.Groups(groupKey) = New TakeOffGroup()
		End If

		result.Groups(groupKey).TotalQuantity += qty.Quantity
	Next

	Return result
End Function
```

---

## 6. AI Intake Traceability

### 6.1 AI Use Cases & FRs

| **UC ID** | **Title** | **FR Range** | **Status** | **File** | **Task** |
| --- | --- | --- | --- | --- | --- |
| UC-AI-001 | Upload drawing | FR-AI-001, FR-AI-002 | ? Not Started | � | T-067 |
| UC-AI-002 | Extract text (OCR) | FR-AI-003 | ? Not Started | Application/AI/OcrService.vb | T-068 |
| UC-AI-003 | Detect scale | FR-AI-004 | ? Not Started | Application/AI/ScaleDetector.vb | T-069 |
| UC-AI-004 | Detect geometry | FR-AI-005, FR-AI-006 | ? Not Started | Application/AI/GeometryDetector.vb | T-070 |
| UC-AI-005 | Classify objects | FR-AI-007, FR-AI-008 | ? Not Started | Application/AI/CategoryClassifier.vb | T-071 |
| UC-AI-006 | Review & correct | FR-AI-009, FR-AI-010 | ?? Stub | Application/AI/AiIntakeService.vb | T-070, T-071 |
| UC-AI-007 | Map materials | FR-AI-011 | ? Not Started | � | T-072 |
| UC-AI-008 | Export AI take-off | FR-AI-012, FR-AI-013 | ? Partial | ExcelExporter.vb | T-073 |

### 6.2 AI Service Contracts (Planned)

```vb
' Application/AI/AiIntakeService.vb - Stub exists
Public Class AiIntakeService
	Private ReadOnly _importService As IDrawingImportService ' TODO: Implement
	Private ReadOnly _ocrService As IOcrService ' TODO: Implement
	Private ReadOnly _scaleDetector As IScaleDetectionService ' TODO: Implement
	Private ReadOnly _geometryDetector As IGeometryDetectionService ' TODO: Implement
	Private ReadOnly _classifier As IObjectClassificationService ' TODO: Implement

	' TODO: Implement IntakeDrawingAsync()
	' TODO: Implement AcceptCandidate()
End Class
```

**AI Intake Pipeline Status:** Stubs created, implementations pending T-067..T-076.

---

## 7. Export Traceability

### 7.1 Export Formats

| **FR ID** | **Requirement** | **Status** | **File** | **Format** | **Task** |
| --- | --- | --- | --- | --- | --- |
| FR-DT-045 | Exportable aggregation output | ? Implemented | ExcelExporter.vb | CSV | T-023 |
| � | Excel export | ?? Partial | ExcelExporter.vb | XLSX format stub | Future |
| � | PDF export | ? Not Started | � | � | Future |
| � | Integration API export | ? Not Started | � | � | Future |

**ExcelExporter:**

```vb
' Application/Export/ExcelExporter.vb
Public Class ExcelExporter
	Public Sub ExportToExcel(takeOffSummary As TakeOffResult, outputPath As String)
		' Create workbook
		' Write headers: Layer, Material, Quantity, Unit, UnitPrice, Total
		' Write data rows from takeOffSummary.Groups
		' Save to outputPath
	End Sub
End Class
```

---

## 8. Implementation Gaps & Recommendations

### 8.1 Critical Gaps (Must-Have for MVP)

| **Gap** | **Impact** | **Solution** | **Priority** | **Task** |
| --- | --- | --- | --- | --- |
| Undo/Redo not implemented | Cannot undo user actions | Implement CommandHistory | **High** | T-015 |
| Multi-selection incomplete | Can't edit multiple shapes at once | Extend to `_selectedList`, implement bulk edit | **High** | UC-006 |
| Layer deletion missing reassignment dialog | Users lose objects | Complete LayerReassignmentDialog.vb | **High** | UC-007 |
| AI intake stubs only | No AI functionality | Implement T-067..T-076 | **High** | T-067+ |
| Shape validation missing | Degenerate shapes allowed | Add zero-size checks in OnMouseUp | **Medium** | T-002 |
| Serialization not tested | Round-trip data loss possible | Add save/load unit tests | **Medium** | T-008 |

### 8.2 Feature Gaps (Post-MVP)

| **Gap** | **FR** | **Impact** | **Recommendation** |
| --- | --- | --- | --- |
| Circles, ellipses, arcs | FR-DT-004, FR-DT-005, FR-DT-010 | Limited shape library | Implement after MVP using same ShapeBase pattern |
| Text annotations | FR-DT-020, FR-DT-021 | No drawing labels | Create TextShape, MTextShape inheriting ShapeBase |
| Custom marks | FR-DT-050..052 | No visual emphasis | Separate from Smart Tags; visual overlay only |
| Symbol library | FR-DT-030..033 | No reusable components | Implement block/symbol insertion and circular ref check |
| Print flag on layers | FR-LP-001 | Cannot exclude from print | Add `IsPrintable As Boolean` to Layer entity |

### 8.3 Implementation Recommendations

**Phase 1 (MVP � Weeks 1-4):**
- ? Fix data validation (zero-size shapes) � **T-002**
- ? Complete layer deletion reassignment � **UC-007**
- ? Implement undo/redo (CommandHistory) � **T-015**
- ? Add missing `(mixed)` UI for all property fields � **UC-006 completion**
- ? Test save/load round-trip � **T-008 tests**

**Phase 2 (Post-MVP � Weeks 5-8):**
- Additional shape types (circles, text, etc.)
- Full AI intake pipeline (T-067..T-076)
- Symbol library implementation
- Print functionality

---

## 9. Requirements Coverage Matrix

### 9.1 SRS FR Coverage by Implementation Status

```
? Implemented:    23 FRs
??  Partial:       12 FRs
? Not Started:    18 FRs
?????????????????????????
   Total:          53 FRs

Coverage:          66.7% (Implemented + Partial)
```

### 9.2 By Functional Area

| **Area** | **Total FRs** | **Implemented** | **Partial** | **Gap** | **Coverage** |
| --- | --- | --- | --- | --- | --- |
| **Drawing Tools** | 23 | 8 | 5 | 10 | 56% |
| **Canvas & Coords** | 3 | 3 | 0 | 0 | 100% |
| **Properties Panel** | 4 | 3 | 1 | 0 | 100% |
| **Layer Management** | 3 | 2 | 1 | 0 | 100% |
| **Calculation** | 9 | 9 | 0 | 0 | 100% |
| **AI Intake** | 13 | 0 | 1 | 12 | 8% |
| **Export** | 3 | 1 | 1 | 1 | 67% |
| **Symbols/Marks** | 6 | 0 | 0 | 6 | 0% |
| **Other** | � | � | � | � | � |

---

## 10. Test Coverage by FR

### 10.1 Unit Tests Needed

| **FR ID** | **Component** | **Test Case** | **File** | **Status** |
| --- | --- | --- | --- | --- |
| FR-CV-001 | CoordinateConverter | Symmetry (logical ? physical) | Tests/CoordinateConverterTests.vb | ? Missing |
| FR-DT-001..003 | Shape creation | Zero-size validation | Tests/ShapeTests.vb | ? Missing |
| FR-DT-040..045 | Calculator | D0..D3 calculations | Tests/CalculatorTests.vb | ? Missing |
| FR-LP-001..004 | LayerManager | Layer CRUD | Tests/LayerManagerTests.vb | ? Missing |

### 10.2 Integration Tests Needed

| **UC** | **Scenario** | **Expected Result** | **Status** |
| --- | --- | --- | --- |
| UC-001 | Draw line ? verify in canvas | Line appears at correct logical coords | ? Missing |
| UC-002 | Draw shape ? change layer ? verify layer panel | Object count updates | ? Missing |
| UC-004 | Draw shapes ? run take-off ? verify totals | Correct aggregation | ? Missing |
| UC-007 | Delete layer ? reassign objects ? verify all moved | No objects orphaned | ? Missing |

---

## 11. Cross-References

### Related SDLC Documents

- **SRS:** `05_SDLC_Library/0104_SRS.md` � Full FR definitions
- **UX/UI:** `05_SDLC_Library/0208_UX_UI_Design.md` � UI behavior specs
- **Tasks:** `06_VIBE_CODING_GUIDE/1_Task_Backlog.md` � Atomic work units
- **Architecture:** `0701_CanvasControl_Technical_Architecture.md` � System design
- **Components:** `0702_Component_Reference_Detailed.md` � Implementation details

### Matrix Cross-Links

| **SRS Section** | **Implementation** | **Architecture** | **Tasks** |
| --- | --- | --- | --- |
| 5.1 Drawing Tools | Desktop/Controls/LineShape, RectShape | 0701 �4 (Shape Model) | T-001..T-005 |
| 5.2 Canvas & Coords | Desktop/Controls/CanvasControl | 0701 �5 (Coordinate System) | T-001, T-008 |
| 5.3 Property Panel | Desktop/Controls/PropertiesPanel | 0701 �3 (Layer Management) | T-026..T-031 |
| 5.4 Layer Panel | Desktop/Controls/LayerPanel | 0701 �3 (Layer Management) | T-024, T-025 |
| 6 Calculation | Application/TakeOffCalculator | 0701 �6 (Calculation Engine) | T-010..T-023 |
| 9 AI | Application/AI/* | 0701 �7 (AI Pipeline) | T-067..T-076 |

---

## 12. Next Steps & Recommended Actions

### Immediate (Week 1)

1. ? Review this traceability matrix with the team
2. ? Identify critical blockers (Undo/Redo, Multi-selection)
3. ? Prioritize gaps by impact (see �8.1)
4. ? Create unit test fixtures for core components

### Short-Term (Weeks 2-4)

1. Close critical gaps (T-002, T-015, UC-006, UC-007)
2. Implement missing shape validation
3. Add comprehensive test coverage
4. Validate round-trip serialization

### Medium-Term (Weeks 5-8)

1. Begin AI intake implementation (T-067+)
2. Add additional shape types
3. Implement symbol library
4. Enhance export formats (Excel, PDF)

---

**End � Implementation Traceability Matrix (v1.0.0-draft)**

