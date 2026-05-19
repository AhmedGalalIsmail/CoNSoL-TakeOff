---
color: var(--mk-color-blue)
---
# CoNSoL-TakeOff Implementation Task Matrix

## Purpose: 
Map SDLC requirements (Mega-File.md) to concrete implementation tasks across all layers  

## Alignment: 
SDLC 0104-SRS §5 (Functional Requirements) → Implementation Checklist  

## Status Tracking: 
Coverage map to ensure no requirements are missed

---

# 📋 Executive Summary

This document provides:
1. **Use Case → Task Mapping** - Which SDLC use cases map to which implementation tasks
2. **Layer Coverage Matrix** - Track implementation progress per layer
3. **Task Breakdown** - Atomic tasks for each use case/feature
4. **Verification Checklist** - QA/testing requirements per task

---

# 🎯 Foundation Layer Tasks

**Scope:** Domain entities, utilities, infrastructure  
**Dependency:** None (independent)  
**Testing:** Unit tests + integration tests with repos

---

## Foundation Task Matrix

| Task ID | SDLC Ref | Feature | Entity | Layer | Status | Priority |
|---------|----------|---------|--------|-------|--------|----------|
| FND-001 | 0104-5.1.1 | Create CanvasLayout entity | CanvasLayout | Domain | 🔲 TODO | P0 |
| FND-002 | 0104-5.1.1 | Implement CanvasLayout invariants | CanvasLayout | Domain | 🔲 TODO | P0 |
| FND-003 | 0104-5.1.1 | Add CanvasLayout validation module | CanvasLayout | Domain | 🔲 TODO | P0 |
| FND-004 | 0104-5.1.1 | Add/Delete layer with reassignment logic | CanvasLayout | Domain | 🔲 TODO | P1 |
| FND-005 | 0104-5.1.1 | Create CanvasElement entity | CanvasElement | Domain | 🔲 TODO | P0 |
| FND-006 | 0104-5.1.1 | Implement CanvasElement invariants | CanvasElement | Domain | 🔲 TODO | P0 |
| FND-007 | 0104-5.1.1 | Add CanvasElement validation module | CanvasElement | Domain | 🔲 TODO | P0 |
| FND-008 | 0104-5.1.1 | Add geometry validation by shape type | CanvasElement | Domain | 🔲 TODO | P0 |
| FND-009 | 0104-5.1.1 | Implement parent-child relationship validation | CanvasElement | Domain | 🔲 TODO | P1 |
| FND-010 | 0104-5.1.1 | Create BusinessDefinition entity | BusinessDef | Domain | 🔲 TODO | P0 |
| FND-011 | 0104-5.1.1 | Implement BusinessDefinition invariants | BusinessDef | Domain | 🔲 TODO | P0 |
| FND-012 | 0104-5.1.1 | Dimension mode validation (D0-D3) | BusinessDef | Domain | 🔲 TODO | P0 |
| FND-013 | 0104-5.1.1 | Create Layer entity | Layer | Domain | 🔲 TODO | P0 |
| FND-014 | 0104-5.1.1 | Implement Layer visibility/lock logic | Layer | Domain | 🔲 TODO | P1 |
| FND-015 | 0104-5.1.1 | Create AppConfig with validation | AppConfig | Infrastructure | 🔲 TODO | P0 |
| FND-016 | 0104-5.1.1 | Load config from file (environment-aware) | AppConfig | Infrastructure | 🔲 TODO | P0 |
| FND-017 | 0104-5.1.1 | JSON serialization wrapper | JSON | Infrastructure | 🔲 TODO | P0 |
| FND-018 | 0104-5.1.1 | Round-trip serialization validation | JSON | Infrastructure | 🔲 TODO | P1 |
| FND-019 | 0104-5.1.1 | File I/O service (.takeoff files) | FileStore | Infrastructure | 🔲 TODO | P1 |
| FND-020 | 0104-5.1.1 | Material JSON store | MatStore | Infrastructure | 🔲 TODO | P1 |

---

## Foundation Layer Verification

### FND-001 through FND-004: CanvasLayout Entity

#### **Requirement:** 
UC-001, UC-002, UC-007 depend on this  
#### **SDLC Reference:** 
0104-SRS §5.2 (Canvas & Coordinate System)
#### **Task Breakdown:**

```
FND-001: Create CanvasLayout entity
├─ Define properties: Id, Name, Unit, ScaleFactor, Elements[], Layers[], ActiveLayerId
├─ Implement Guid.NewGuid for Id initialization
├─ Ensure Elements list initialized (not null)
├─ Ensure Layers list initialized (not null)
└─ Document default values in code

FND-002: Implement CanvasLayout invariants
├─ Validate Id ≠ Guid.Empty
├─ Validate Name is not null/whitespace
├─ Validate Unit ∈ {"metric", "imperial"}
├─ Validate ScaleFactor > 0
├─ Validate Layers.Count >= 1
├─ Validate all Elements reference valid LayerId
├─ Validate ActiveLayerId exists in Layers (if set)
└─ Add property setters that enforce invariants

FND-003: Add CanvasLayout validation module
├─ Create CanvasLayoutValidation module
├─ Implement ValidateLayout() public method
├─ Implement layer existence checks
├─ Implement element-layer reference checks
├─ Implement active layer checks
└─ Add detailed error messages for debugging

FND-004: Add/Delete layer with reassignment logic
├─ Implement AddLayer(name) method
├─ Implement DeleteLayer(id, moveToLayerId?) method
├─ Validate layer name uniqueness
├─ Validate active layer cannot be deleted
├─ Implement reassignment of elements on delete
├─ Test with 0 elements, 1+ elements
└─ Test with nested objects
```

#### **Verification Checklist:**

- [ ] Unit test: Empty CanvasLayout creation
- [ ] Unit test: AddElement with valid layer
- [ ] Unit test: AddElement with invalid layer → exception
- [ ] Unit test: CreateLayer with duplicate name → exception
- [ ] Unit test: DeleteLayer with active layer → exception
- [ ] Unit test: DeleteLayer with reassignment
- [ ] Unit test: DeleteLayer with element deletion
- [ ] Integration test: Serialize/deserialize layout
- [ ] Integration test: Layer and element round-trip

---

### FND-005 through FND-012: CanvasElement & BusinessDefinition

#### **Requirement:** 
UC-001 (Draw), UC-003 (Tags), UC-004 (Aggregation)  
#### **SDLC Reference:** 
0104-SRS §5.1 (Drawing Tools)
#### **Task Breakdown:**

```
FND-005: Create CanvasElement entity
├─ Define properties: Id, Type, Layer, GeometryJson, BusinessJson
├─ Define relationship properties: ParentElementId, RelationshipType
├─ Implement Create() factory method
├─ Add geometry serialization support
├─ Add business metadata serialization support
└─ Document JSON structure in comments

FND-006: Implement CanvasElement invariants
├─ Validate Id ≠ Guid.Empty
├─ Validate Type ∈ {Line, Rectangle, Circle, Polyline, Text, Symbol, Dimension, Arc, Spline, Bezier}
├─ Validate Layer is valid GUID string
├─ Validate GeometryJson is valid JSON
├─ Validate BusinessJson is valid JSON
├─ Validate geometry dimensions > 0 (no zero-size shapes)
├─ Validate no cyclic parent-child relationships
└─ Add property setters that enforce invariants

FND-007: Add CanvasElement validation module
├─ Create CanvasElementValidation module
├─ Implement ValidateElement() for all invariants
├─ Implement geometry type-specific validation
├─ Implement cyclic reference detection (BFS)
├─ Implement detailed error messages
└─ Support batch validation

FND-008: Add geometry validation by shape type
├─ For Line: validate start ≠ end
├─ For Rectangle: validate width > 0 AND height > 0
├─ For Circle: validate radius > 0
├─ For Ellipse: validate major/minor > 0
├─ For Polyline: validate 2+ points, all distinct
├─ For Text: validate content not empty
├─ For Arc: validate start/end angles differ
├─ For Spline/Bezier: validate 2+ control points
└─ Return detailed error messages per type

FND-009: Implement parent-child relationship validation
├─ Create ElementRelationship enum (Nested, Stacked, etc.)
├─ Implement SetParent() method with cycle detection
├─ Implement BFS cycle detection algorithm
├─ Document relationship types in comments
├─ Test with 2-level nesting (parent → child)
├─ Test with 3-level nesting (detect cycle)
├─ Test with multiple children on same parent
└─ Test with sibling relationships

FND-010 through FND-012: BusinessDefinition
├─ Create entity with BlockId, DimensionMode, FormulaCode, MaterialId
├─ Validate DimensionMode ∈ {D0, D1, D2, D3}
├─ Validate Quantity >= 0
├─ Validate UnitPrice >= 0
├─ Implement GetTotalCost() = Quantity * UnitPrice
├─ Implement ValidateDimensionMode(geometryType) to check allowed modes per type
└─ Test all dimension modes
```

#### **Verification Checklist:**

- [ ] Unit test: Create Line element
- [ ] Unit test: Create Rectangle element
- [ ] Unit test: Create Circle element
- [ ] Unit test: Zero-size rectangle → exception
- [ ] Unit test: Invalid geometry JSON → exception
- [ ] Unit test: Business JSON round-trip
- [ ] Unit test: Parent-child relationship (valid)
- [ ] Unit test: Parent-child cycle detection
- [ ] Unit test: Dimension mode validation per type
- [ ] Integration test: Full element creation workflow

---

# 🎨 Rendering Layer Tasks

**Scope:** 2D canvas, shape rendering, visual feedback  
**Dependency:** Foundation (Domain entities)  
**Testing:** Visual tests + screenshot comparison

---

## Rendering Task Matrix

| Task ID | SDLC Ref | Feature | Component | Layer | Status | Priority |
|---------|----------|---------|-----------|-------|--------|----------|
| RND-001 | 0104-5.2 | Create CanvasControl (user control) | CanvasControl | Desktop | 🔲 TODO | P0 |
| RND-002 | 0104-5.2 | Implement coordinate transformation (physical ↔ logical) | CanvasControl | Desktop | 🔲 TODO | P0 |
| RND-003 | 0104-5.2 | Implement OnPaint rendering loop | CanvasControl | Desktop | 🔲 TODO | P0 |
| RND-004 | 0104-5.2 | Render visible layers only | CanvasControl | Desktop | 🔲 TODO | P0 |
| RND-005 | 0104-5.2 | Render Line shapes | LineShape | Desktop | 🔲 TODO | P0 |
| RND-006 | 0104-5.2 | Render Rectangle shapes | RectangleShape | Desktop | 🔲 TODO | P0 |
| RND-007 | 0104-5.2 | Render Circle shapes | CircleShape | Desktop | 🔲 TODO | P0 |
| RND-008 | 0104-5.2 | Render selection highlights | SelectionRenderer | Desktop | 🔲 TODO | P1 |
| RND-009 | 0104-5.2 | Render preview geometry (rubber-band) | PreviewRenderer | Desktop | 🔲 TODO | P0 |
| RND-010 | 0104-5.2 | Implement zoom (scale factor) | CanvasControl | Desktop | 🔲 TODO | P1 |
| RND-011 | 0104-5.2 | Implement pan (offset) | CanvasControl | Desktop | 🔲 TODO | P1 |
| RND-012 | 0104-5.2 | Render grid background | GridRenderer | Desktop | 🔲 TODO | P2 |
| RND-013 | 0104-5.2 | Hit test for shape selection | HitTestService | Desktop | 🔲 TODO | P1 |
| RND-014 | 0104-5.2 | Shape factory for type-specific rendering | ShapeFactory | Desktop | 🔲 TODO | P1 |

---

## Rendering Layer Verification

### RND-001 through RND-004: Canvas Control Foundation

#### **Requirement:** 
UC-001 (Draw), UC-006 (Multi-select)  
#### **SDLC Reference:** 
0104-SRS §5.2 (Canvas & Coordinate System)
#### **Task Breakdown:**

```
RND-001: Create CanvasControl (user control)
├─ Inherit from UserControl
├─ Add CurrentLayout property (CanvasLayout)
├─ Add CurrentTool property (ToolType)
├─ Add ZoomLevel property (default 1.0)
├─ Add PanOffset property (default 0,0)
├─ Add SetLayout() public method
├─ Implement IDisposable for graphics resources
└─ Document public API

RND-002: Implement coordinate transformation
├─ Create PhysicalToLogical(screenPoint) method
│  └─ Return (X-PanX)/Zoom, (Y-PanY)/Zoom
├─ Create LogicalToPhysical(logicalPoint) method
│  └─ Return (X*Zoom+PanX), (Y*Zoom+PanY)
├─ Create ValidateCoordinate() for bounds checking
├─ Test with various zoom levels (0.5, 1.0, 2.0)
├─ Test pan offsets positive/negative
└─ Document coordinate system

RND-003: Implement OnPaint rendering loop
├─ Override OnPaint(PaintEventArgs)
├─ Set graphics quality: SmoothingMode.AntiAlias
├─ Set TextRenderingHint.AntiAlias
├─ Clear background (white or configurable)
├─ Call RenderGrid()
├─ Call RenderLayers() for each visible layer
├─ Call RenderSelectionHighlights()
├─ Call RenderPreview() if drawing
└─ Handle errors gracefully (catch and log)

RND-004: Render visible layers only
├─ Iterate CurrentLayout.Layers
├─ Skip if layer.Visible == False
├─ Filter CurrentLayout.Elements by layer
├─ Call RenderElement() for each element
├─ Respect layer color/style defaults
├─ Support layer lock indicator (visual)
└─ Test with hidden layers
```

#### **Verification Checklist:**

- [ ] Visual test: Canvas renders without crashes
- [ ] Visual test: Zoom 0.5x displays correctly
- [ ] Visual test: Zoom 2.0x displays correctly
- [ ] Visual test: Pan left/right/up/down works
- [ ] Visual test: Hidden layer not rendered
- [ ] Visual test: Layer with color applied to shapes
- [ ] Unit test: PhysicalToLogical(0,0) at origin
- [ ] Unit test: LogicalToPhysical round-trip
- [ ] Screenshot test: Compare before/after zoom

---

### RND-005 through RND-009: Shape Rendering

#### **Requirement:** 
UC-001 (Draw), FR-DT-001 through FR-DT-033  
#### **SDLC Reference:** 
0104-SRS §5.1 (Drawing Tools)
#### **Task Breakdown:**

```
RND-005: Render Line shapes
├─ Create LineShape class
├─ Add StartPoint, EndPoint properties
├─ Add Color, LineWidth properties
├─ Implement Render(graphics, zoom, isSelected) method
├─ Draw line with scaled width
├─ Draw selection handles (blue circles at ends) if selected
├─ Implement HitTest(point, tolerance) method
├─ Test rendering at various zoom levels
└─ Document color inheritance from layer

RND-006: Render Rectangle shapes
├─ Create RectangleShape class
├─ Add TopLeft, Width, Height properties
├─ Add Rotation (optional) property
├─ Implement Render() with rotation support
├─ Draw rectangle outline + optional fill
├─ Draw selection handles (4 corners) if selected
├─ Implement HitTest() for rectangle
├─ Support corner radius (optional)
└─ Test rotation at 0°, 45°, 90°

RND-007: Render Circle shapes
├─ Create CircleShape class
├─ Add Center, Radius properties
├─ Implement Render() method
├─ Draw circle outline + optional fill
├─ Draw selection handles (4 cardinal points) if selected
├─ Implement HitTest() for circle
├─ Test at various zoom levels
└─ Ensure circular appearance regardless of zoom

RND-008: Render selection highlights
├─ Create SelectionRenderer helper
├─ Implement highlight drawing (dashed outline)
├─ Draw blue handles at control points
├─ Make highlight non-destructive (overlay only)
├─ Support multi-selection (multiple outlines)
├─ Test with single selection
├─ Test with multi-selection (5+ objects)
└─ Ensure handles don't overlap badly

RND-009: Render preview geometry (rubber-band)
├─ Create PreviewRenderer helper
├─ Implement temporary line drawing while dragging
├─ Draw with dashed or lighter color
├─ Update on every MouseMove event
├─ Clear on tool deactivation or commit
├─ Test with Line tool (start → current mouse)
├─ Test with Rectangle tool (corner → current mouse)
└─ Ensure smooth update on drag
```

#### **Verification Checklist:**

- [ ] Visual test: Line renders at various angles
- [ ] Visual test: Rectangle renders with correct aspect ratio
- [ ] Visual test: Circle appears circular (not oval)
- [ ] Visual test: Selection highlight visible and clear
- [ ] Visual test: Rubber-band preview smooth on drag
- [ ] Unit test: HitTest Line at various distances
- [ ] Unit test: HitTest Rectangle inside/outside
- [ ] Unit test: HitTest Circle inside/outside
- [ ] Screenshot test: Compare various zoom levels

---

# ⌨️ Interaction Layer Tasks

**Scope:** User input, tool state machines, multi-selection  
**Dependency:** Foundation + Rendering  
**Testing:** Integration tests with mouse simulation

---

## Interaction Task Matrix

| Task ID | SDLC Ref | Feature | Component | Layer | Status | Priority |
|---------|----------|---------|-----------|-------|--------|----------|
| INT-001 | 0104-6.2 | Create BaseTool abstract class | ToolSystem | Desktop | 🔲 TODO | P0 |
| INT-002 | 0104-6.2 | Implement SelectTool | SelectTool | Desktop | 🔲 TODO | P0 |
| INT-003 | 0104-6.2 | Implement LineTool | LineTool | Desktop | 🔲 TODO | P0 |
| INT-004 | 0104-6.2 | Implement RectangleTool | RectangleTool | Desktop | 🔲 TODO | P0 |
| INT-005 | 0104-6.2 | Implement CircleTool | CircleTool | Desktop | 🔲 TODO | P0 |
| INT-006 | 0104-6.2 | Implement PanTool | PanTool | Desktop | 🔲 TODO | P1 |
| INT-007 | 0104-6.2 | Implement ZoomTool | ZoomTool | Desktop | 🔲 TODO | P1 |
| INT-008 | 0104-6.2 | Tool manager (activate/deactivate) | ToolManager | Desktop | 🔲 TODO | P0 |
| INT-009 | 0104-6.2 | Create SelectionManager | SelectionManager | Desktop | 🔲 TODO | P0 |
| INT-010 | 0104-6.2 | Single selection (click) | SelectionManager | Desktop | 🔲 TODO | P0 |
| INT-011 | 0104-6.2 | Multi-selection (Ctrl+click) | SelectionManager | Desktop | 🔲 TODO | P0 |
| INT-012 | 0104-6.2 | Window selection (drag rectangle) | SelectionManager | Desktop | 🔲 TODO | P1 |
| INT-013 | 0104-6.2 | Escape key handling (cancel drawing) | InputHandler | Desktop | 🔲 TODO | P0 |
| INT-014 | 0104-6.2 | Keyboard shortcuts (file, edit, view) | KeyboardHandler | Desktop | 🔲 TODO | P2 |

---

## Interaction Layer Verification

### INT-001 through INT-008: Tool System

#### **Requirement:** 
UC-001 (Draw), UC-006 (Edit)  
#### **SDLC Reference:** 
0104-SRS §6.2 (Tool Interaction Model)
#### **Task Breakdown:**

```
INT-001: Create BaseTool abstract class
├─ Define abstract methods: OnMouseDown, OnMouseMove, OnMouseUp
├─ Define virtual methods: OnActivate, OnDeactivate
├─ Define property: _canvas (protected reference)
├─ Add _isActive, _previewGeometry fields
├─ Implement Activate() - set cursor, call OnActivate()
├─ Implement Deactivate() - clear preview, call OnDeactivate()
├─ Add GetCursor() abstract method
├─ Document tool lifecycle in comments

INT-002: Implement SelectTool
├─ OnMouseDown: HitTest at click point
│  ├─ If no hit: clear selection
│  ├─ If Ctrl pressed: toggle selection
│  ├─ Else: single selection
├─ OnMouseMove: Show hover feedback (optional)
├─ OnMouseUp: Finalize selection
├─ Implement window selection (drag rectangle)
├─ Test click on object
├─ Test click on empty space
├─ Test Ctrl+click for toggle
└─ Test drag for window selection

INT-003: Implement LineTool
├─ State: idle, waiting_for_end
├─ OnMouseDown first click: store start point, enter waiting_for_end
├─ OnMouseDown second click: commit line, return to idle
├─ OnMouseMove (while waiting): update preview geometry
├─ Support polyline mode (double-click to finish)
├─ Validate start ≠ end
├─ Test: click, move, click
├─ Test: multi-segment polyline
├─ Test: Escape to cancel
└─ Document polyline behavior

INT-004: Implement RectangleTool
├─ State: idle, drawing
├─ OnMouseDown: start rectangle, enter drawing state
├─ OnMouseMove (while drawing): update width/height preview
├─ OnMouseUp: commit rectangle, return to idle
├─ Validate width > 0 AND height > 0
├─ Support optional drag-from-center mode
├─ Test: drag from corner
├─ Test: drag from center
├─ Test: small rectangles
└─ Test: Escape to cancel

INT-005: Implement CircleTool
├─ State: idle, drawing
├─ OnMouseDown: center point, enter drawing state
├─ OnMouseMove (while drawing): update radius preview
├─ OnMouseUp: commit circle, return to idle
├─ Validate radius > 0
├─ Support 2-point (diameter) mode
├─ Test: small circle
├─ Test: large circle
├─ Test: 2-point mode
└─ Test: Escape to cancel

INT-006 & INT-007: Pan/Zoom Tools (optional P1)
├─ PanTool: drag to pan, update PanOffset
├─ ZoomTool: mouse wheel or drag-up/down to zoom
├─ Validate zoom bounds (0.1x to 10x)
├─ Smooth zoom/pan transitions
└─ Test at various zoom levels

INT-008: Tool manager
├─ Maintain current tool reference
├─ SetTool(toolType) method
├─ Deactivate current, activate new
├─ Route MouseDown/Move/Up to active tool
├─ Prevent tool switching during drawing
└─ Test tool switching
```

#### **Verification Checklist:**

- [ ] Unit test: SelectTool single selection
- [ ] Unit test: SelectTool toggle (Ctrl+click)
- [ ] Unit test: LineTool start → end commits
- [ ] Unit test: RectangleTool drag commits
- [ ] Unit test: CircleTool radius validation
- [ ] Integration test: Draw line, then rectangle
- [ ] Integration test: Multi-selection with 3 objects
- [ ] Integration test: Escape cancels drawing
- [ ] Integration test: Tool switching during idle
- [ ] Integration test: Tool cannot switch during drawing

---

### INT-009 through INT-014: Selection & Input Handling

#### **Requirement:** 
UC-002 (Assign layer), UC-006 (Multi-select)  
#### **SDLC Reference:** 
0104-SRS §6.4 (Multi-Selection Behavior)
#### **Task Breakdown:**

```
INT-009: Create SelectionManager
├─ Maintain List(Of CanvasElement) _selected
├─ Property SelectedElements (readonly IReadOnlyList)
├─ Property Count
├─ Event SelectionChanged
├─ Ensure no duplicates in list
├─ Validate all selected items exist in layout
├─ Method: ClearSelection()
├─ Method: IsUniformSelection()
└─ Method: GetSharedProperties()

INT-010: Single selection (click)
├─ SelectSingle(element) clears and selects one
├─ Fires SelectionChanged event
├─ Updates property panel to show element properties
├─ Updates canvas highlights
└─ Test: click different objects sequentially

INT-011: Multi-selection (Ctrl+click)
├─ ToggleSelection(element) adds/removes element
├─ Fires SelectionChanged event
├─ Support: Ctrl+click to add, Ctrl+click again to remove
├─ Display property panel with (mixed) values where they differ
├─ Test: Ctrl+click to add 3 objects
├─ Test: Ctrl+click to remove one
└─ Test: Shared property display

INT-012: Window selection (drag rectangle) - P1
├─ On mouse down in empty area: start window rect
├─ On mouse move: update window rect preview
├─ On mouse up: select all objects inside rect
├─ Support Shift modifier to add to selection
├─ Test: select 3 objects in window
├─ Test: Shift+drag to add more
└─ Test: no selection (window outside all objects)

INT-013: Escape key handling
├─ If drawing: cancel current tool, clear preview
├─ If selection: clear selection
├─ Global handler in MainForm
├─ Test: press Escape while drawing line
├─ Test: press Escape with objects selected
└─ Test: press Escape with nothing selected

INT-014: Keyboard shortcuts - P2 (optional)
├─ Ctrl+N: New drawing
├─ Ctrl+O: Open file
├─ Ctrl+S: Save file
├─ Ctrl+A: Select all
├─ Delete: Delete selected
├─ Test: Ctrl+N creates new layout
├─ Test: Delete removes selected objects
└─ Test: Ctrl+A selects all visible objects
```

#### **Verification Checklist:**

- [ ] Unit test: SelectionManager no duplicates
- [ ] Unit test: SelectionManager toggle logic
- [ ] Unit test: GetSharedProperties with same type
- [ ] Unit test: GetSharedProperties with mixed types
- [ ] Integration test: Click selects object
- [ ] Integration test: Ctrl+click adds to selection
- [ ] Integration test: Escape cancels drawing
- [ ] Integration test: Escape clears selection
- [ ] Integration test: Window selection in bounds
- [ ] Integration test: Keyboard Ctrl+A selects all

---

# 💼 Business Layer Tasks

**Scope:** Use case orchestration, calculations, aggregations  
**Dependency:** Foundation + Infrastructure  
**Testing:** Unit tests with mock data

---

## Business Task Matrix

| Task ID | SDLC Ref | Feature | Component | Layer | Status | Priority |
|---------|----------|---------|-----------|-------|--------|----------|
| BUS-001 | 0104-5.3 | Create TakeOffContext | Context | Application | 🔲 TODO | P0 |
| BUS-002 | 0104-5.3 | Create TakeOffResult | Result | Application | 🔲 TODO | P0 |
| BUS-003 | 0104-5.3 | Create TakeOffCalculator | Calculator | Application | 🔲 TODO | P0 |
| BUS-004 | 0104-5.4 | Implement Calculate() - main method | Calculator | Application | 🔲 TODO | P0 |
| BUS-005 | 0104-5.4 | Extract dimension (D1: length) | Calculator | Application | 🔲 TODO | P0 |
| BUS-006 | 0104-5.4 | Extract dimension (D2: area) | Calculator | Application | 🔲 TODO | P0 |
| BUS-007 | 0104-5.4 | Extract dimension (D3: volume) | Calculator | Application | 🔲 TODO | P1 |
| BUS-008 | 0104-5.4 | Apply nested object logic (subtract) | Calculator | Application | 🔲 TODO | P0 |
| BUS-009 | 0104-5.4 | Calculate total cost | Calculator | Application | 🔲 TODO | P0 |
| BUS-010 | 0104-5.4 | Handle formula application | Calculator | Application | 🔲 TODO | P1 |
| BUS-011 | 0104-UC-004 | Create TakeOffService | Service | Application | 🔲 TODO | P0 |
| BUS-012 | 0104-UC-004 | Aggregate by material | Service | Application | 🔲 TODO | P0 |
| BUS-013 | 0104-UC-004 | Aggregate by layer | Service | Application | 🔲 TODO | P1 |
| BUS-014 | 0104-UC-004 | Aggregate by object type | Service | Application | 🔲 TODO | P1 |
| BUS-015 | 0104-UC-004 | Export to CSV | Service | Application | 🔲 TODO | P1 |
| BUS-016 | 0104-UC-004 | Export to Excel | Service | Application | 🔲 TODO | P2 |
| BUS-017 | 0104-5.4 | Create MaterialService | Service | Application | 🔲 TODO | P1 |
| BUS-018 | 0104-5.4 | Material lookup by ID | Service | Application | 🔲 TODO | P1 |
| BUS-019 | 0104-5.4 | Material price lookup | Service | Application | 🔲 TODO | P1 |

---

## Business Layer Verification

### BUS-001 through BUS-010: Calculation Engine

#### **Requirement:** 
UC-004 (Take-off summary), FR-DT-040 through FR-DT-045  
#### **SDLC Reference:** 
0104-SRS §5.4 (Property Panel)
#### **Task Breakdown:**

```
BUS-001: Create TakeOffContext
├─ Property: UnitSystem ("metric" or "imperial")
├─ Property: ApplyFormulaOverrides (bool)
├─ Property: RoundMode (RoundHalfUp, Floor, Ceiling)
├─ Property: IncludeZeroQuantity (bool)
├─ Implement validation in constructor
├─ Document context parameters
└─ Test with various combinations

BUS-002: Create TakeOffResult
├─ Property: MaterialGroups (List(Of MaterialGroup))
├─ Property: GrandTotal (double)
├─ Property: TotalCost (double)
├─ Method: AddGroupResult(key, group)
├─ Method: CalculateGrandTotal()
├─ Implement Equals() for testing
└─ Serialize to JSON

BUS-003: Create TakeOffCalculator
├─ Constructor: inject ILogger
├─ Public Calculate(layout, context) → TakeOffResult
├─ Private CalculateElementQuantity(element, business, layout)
├─ Private ExtractLength(shapeType, geometry)
├─ Private ExtractArea(shapeType, geometry)
├─ Private ExtractVolume(shapeType, geometry, business)
├─ Private GetChildQuantityToSubtract(element, layout)
├─ Private CalculateCosts(result, context)
└─ Error handling and logging throughout

BUS-004: Implement Calculate() - main method
├─ Validate inputs (layout not null, context not null)
├─ Call ValidateLayout() 
├─ Group elements (by material by default)
├─ For each group: call CalculateGroupTotal()
├─ Apply nested object logic
├─ Calculate final costs
├─ Return result with no exceptions
├─ Log calculation completion
└─ Test with empty layout

BUS-005: Extract dimension (D1: length)
├─ For Line: distance from start to end
├─ For Rectangle: width OR height (choose one)
├─ For Circle: circumference
├─ For Polyline: total perimeter
├─ Return 0.0 for invalid types
├─ Test each type
└─ Test with various coordinates

BUS-006: Extract dimension (D2: area)
├─ For Rectangle: width * height
├─ For Circle: π * radius²
├─ For Ellipse: π * a * b
├─ For Polygon: shoelace formula
├─ Return 0.0 for non-2D types (Line, Text)
├─ Test with various sizes
├─ Verify π constant used
└─ Test zero-area handling

BUS-007: Extract dimension (D3: volume) - P1
├─ For 3D objects: area * depth
├─ Depth from logical 3D properties
├─ Return 0.0 if depth not available
├─ Test with depth property
└─ Validate depth > 0

BUS-008: Apply nested object logic
├─ Find parent for each element
├─ Recursively calculate child quantity
├─ Subtract child from parent
├─ Ensure result >= 0 (no negative)
├─ Test: wall with door (door area subtracts)
├─ Test: slab with opening (opening area subtracts)
├─ Test: 3-level nesting (grandparent → parent → child)
└─ Document subtraction rules

BUS-009: Calculate total cost
├─ For each MaterialGroup: cost = quantity * unitPrice
├─ Sum all costs for grand total
├─ Handle zero quantity (cost = 0)
├─ Handle missing unit price (default 0)
├─ Test with positive quantities
├─ Test with zero quantities
├─ Test with negative prices (invalid)
└─ Validate no arithmetic errors

BUS-010: Handle formula application - P1
├─ Look up formula by FormulaCode
├─ Parse formula expression
├─ Substitute variables (H, W, L)
├─ Evaluate expression safely
├─ Override quantity with formula result
├─ Test with simple formula (H * W)
├─ Test with complex formula (H * W * L + margin)
└─ Document formula syntax
```

#### **Verification Checklist:**

- [ ] Unit test: Calculate with empty layout
- [ ] Unit test: Calculate with 1 rectangle
- [ ] Unit test: Extract area (rectangle)
- [ ] Unit test: Extract area (circle)
- [ ] Unit test: Nested objects (wall - door)
- [ ] Unit test: Nested objects (3-level)
- [ ] Unit test: Cost calculation (quantity × price)
- [ ] Unit test: Grand total sum
- [ ] Unit test: Zero-area handling
- [ ] Unit test: Negative quantity (edge case)
- [ ] Integration test: Full workflow (draw → calculate → export)
- [ ] Integration test: Multiple materials

---

### BUS-011 through BUS-019: Aggregation & Services

#### **Requirement:** 
UC-004 (Take-off summary), FR-DT-043 through FR-DT-045  
#### **SDLC Reference:** 
0104-SRS §4 (Use Cases)
#### **Task Breakdown:**

```
BUS-011: Create TakeOffService
├─ Constructor: inject TakeOffCalculator, MaterialService
├─ Public method: AggregateByMaterial()
├─ Public method: AggregateByLayer()
├─ Public method: AggregateByObjectType()
├─ Public method: ExportToCsv()
├─ Public method: ExportToExcel() - optional
├─ Error handling and logging
└─ Document service responsibilities

BUS-012: Aggregate by material
├─ Call calculator.Calculate()
├─ Result already grouped by material
├─ Validate result
├─ Return MaterialGroups sorted by name
├─ Test with 3 different materials
├─ Test with same material on multiple objects
└─ Test with zero quantities

BUS-013: Aggregate by layer - P1
├─ Create temp layout per layer
├─ For each layer: call calculator.Calculate()
├─ Return Dictionary(Of LayerName, Result)
├─ Test with 2+ layers
├─ Test with empty layer
└─ Test with hidden layer (skip or include?)

BUS-014: Aggregate by object type - P1
├─ Group elements by Type (Line, Rectangle, Circle)
├─ For each group: sum quantities
├─ Return results sorted by type
├─ Test with mixed shapes
├─ Test with same type
└─ Test edge case: one type only

BUS-015: Export to CSV - P1
├─ Create StreamWriter to file
├─ Write header: "Material,Quantity,Unit,Price,Total"
├─ For each material group: write row
├─ Close stream properly
├─ Validate file created
├─ Test file content format
├─ Test with special characters in material name
└─ Test with empty result

BUS-016: Export to Excel - P2 (optional)
├─ Use EPPlus or similar library
├─ Create worksheet with same columns as CSV
├─ Format header row (bold, color)
├─ Format data rows (number formatting)
├─ Add summary row (grand total)
├─ Test file opens in Excel
├─ Test data accuracy
└─ Test with images/charts (future)

BUS-017 through BUS-019: MaterialService - P1
├─ Constructor: inject IRepository(Of Material)
├─ Method: GetMaterialById(id) → Material
├─ Method: GetPrice(materialId) → double
├─ Method: GetAllMaterials() → List(Of Material)
├─ Error handling (material not found)
├─ Caching (optional)
└─ Document lookup logic
```

#### **Verification Checklist:**

- [ ] Unit test: AggregateByMaterial groups correctly
- [ ] Unit test: ExportToCsv creates valid file
- [ ] Unit test: ExportToCsv with empty result
- [ ] Unit test: MaterialService GetPrice
- [ ] Unit test: MaterialService not found exception
- [ ] Integration test: Calculate → Aggregate → Export workflow
- [ ] Integration test: Multiple aggregation types
- [ ] File I/O test: CSV file readable
- [ ] File I/O test: Excel file opens (if implemented)
- [ ] Acceptance test: Compare export with manual calculation

---

# 🔗 Integration Layer Tasks

**Scope:** Dependency injection, form orchestration, events  
**Dependency:** All other layers  
**Testing:** Integration tests + UI tests

---

## Integration Task Matrix

| Task ID | SDLC Ref | Feature | Component | Layer | Status | Priority |
|---------|----------|---------|-----------|-------|--------|----------|
| IGN-001 | 0104-UC-008 | Create CompositionRoot | DI | Desktop | 🔲 TODO | P0 |
| IGN-002 | 0104-UC-008 | Register infrastructure services | DI | Desktop | 🔲 TODO | P0 |
| IGN-003 | 0104-UC-008 | Register application services | DI | Desktop | 🔲 TODO | P0 |
| IGN-004 | 0104-UC-008 | Register UI components | DI | Desktop | 🔲 TODO | P0 |
| IGN-005 | 0104-UC-008 | Implement Resolve(Of T)() method | DI | Desktop | 🔲 TODO | P0 |
| IGN-006 | 0104-UC-001 | Create MainForm | UI | Desktop | 🔲 TODO | P0 |
| IGN-007 | 0104-UC-001 | Add toolbar with tool buttons | UI | Desktop | 🔲 TODO | P0 |
| IGN-008 | 0104-UC-001 | Add canvas control | UI | Desktop | 🔲 TODO | P0 |
| IGN-009 | 0104-UC-001 | Add property panel | UI | Desktop | 🔲 TODO | P0 |
| IGN-010 | 0104-UC-001 | Add layer panel | UI | Desktop | 🔲 TODO | P1 |
| IGN-011 | 0104-UC-001 | Wire event handlers | UI | Desktop | 🔲 TODO | P0 |
| IGN-012 | 0104-UC-001 | Implement File menu | UI | Desktop | 🔲 TODO | P1 |
| IGN-013 | 0104-UC-001 | Implement Edit menu | UI | Desktop | 🔲 TODO | P1 |
| IGN-014 | 0104-UC-001 | Implement View menu | UI | Desktop | 🔲 TODO | P2 |
| IGN-015 | 0104-UC-001 | Create DomainEventPublisher | Events | Application | 🔲 TODO | P0 |
| IGN-016 | 0104-UC-001 | Publish ElementCreated event | Events | Application | 🔲 TODO | P0 |
| IGN-017 | 0104-UC-001 | Subscribe UI to element events | Events | Desktop | 🔲 TODO | P0 |
| IGN-018 | 0104-UC-001 | Publish CalculationCompleted event | Events | Application | 🔲 TODO | P1 |
| IGN-019 | 0104-UC-001 | Subscribe UI to calculation events | Events | Desktop | 🔲 TODO | P1 |

---

## Integration Layer Verification

### IGN-001 through IGN-005: Dependency Injection

#### **Requirement:** 
UC-008 (Deployment mode)  

#### **SDLC Reference:** 
0104-SRS §2.1 (Product Perspective)

#### **Task Breakdown:**

```
IGN-001: Create CompositionRoot
├─ Create public module or class
├─ Add Initialize() method
├─ Create ServiceCollection
├─ Register all services
├─ Build ServiceProvider
├─ Store in private static field
├─ Expose Resolve(Of T)() method
└─ Handle initialization errors gracefully

IGN-002: Register infrastructure services
├─ Register AppConfig as Singleton
│  └─ LoadFromFile() with error handling
├─ Register ILogger as Singleton
│  └─ Depend on AppConfig.LogFilePath
├─ Register IFileStore as Singleton
├─ Register IMaterialStore as Singleton
├─ Register JsonSerializer as Singleton
├─ Test: Resolve AppConfig returns same instance
├─ Test: Resolve ILogger returns same instance
└─ Test: Singleton behavior verified

IGN-003: Register application services
├─ Register TakeOffCalculator as Scoped
├─ Register TakeOffService as Scoped
├─ Register MaterialService as Scoped
├─ Register DomainEventPublisher as Singleton
├─ All services depend on infrastructure
├─ Test: Resolve service resolves dependencies
├─ Test: Each scope gets new instance
└─ Test: Singleton published events

IGN-004: Register UI components
├─ Register MainForm as Transient
├─ Register BlockAssignmentForm as Transient
├─ Register MaterialCrudForm as Transient
├─ Forms depend on services
├─ Test: Each resolve creates new form
└─ Test: Forms can access services

IGN-005: Implement Resolve(Of T)() method
├─ Get service from ServiceProvider
├─ Handle resolution failures gracefully
├─ Log errors
├─ Return service instance or throw
├─ Test: Resolve valid service succeeds
├─ Test: Resolve unregistered service throws
└─ Test: Error message is helpful
```

#### **Verification Checklist:**

- [ ] Unit test: DI initialized without errors
- [ ] Unit test: AppConfig singleton behavior
- [ ] Unit test: ILogger singleton behavior
- [ ] Unit test: TakeOffCalculator scoped behavior
- [ ] Unit test: Resolve unregistered service throws
- [ ] Unit test: Resolve with dependencies
- [ ] Integration test: Full DI graph
- [ ] Integration test: Circular dependencies prevented

---

### IGN-006 through IGN-019: Form Orchestration & Events

#### **Requirement:** 
UC-001 through UC-008 (All use cases)  
#### **SDLC Reference:** 
0104-SRS §4 (Use Cases)
#### **Task Breakdown:**

```
IGN-006: Create MainForm
├─ Inherit from Form
├─ Add InitializeComponent() (Designer)
├─ Add InitializeServices() method
├─ Add InitializeEventHandlers() method
├─ Constructor calls all three
├─ Add property: _currentLayout (CanvasLayout)
├─ Add property: _canvas (CanvasControl)
├─ Add property: _propertiesPanel (PropertiesPanel)
├─ Add property: _selectionManager (SelectionManager)
└─ Document form responsibilities

IGN-007: Add toolbar with tool buttons
├─ Create toolbar with buttons:
│  ├─ Select, Line, Rectangle, Circle
│  ├─ Pan, Zoom (optional)
│  └─ Separator + File, Edit, View menus
├─ Add icon/text to buttons
├─ Track currently active button (highlight)
├─ Wire Click events to OnTool* methods
├─ Test: Click button activates tool
├─ Test: Previous button deactivates
└─ Test: Cursor changes appropriately

IGN-008: Add canvas control
├─ Create CanvasControl instance
├─ Add to MainForm as central control
├─ Dock or anchor for resizing
├─ Set initial layout (new empty CanvasLayout)
├─ Wire canvas events:
│  ├─ SelectionChanged
│  ├─ ObjectCreated
│  └─ ObjectDeleted
├─ Test: Canvas renders without errors
├─ Test: Events fire on user action
└─ Test: Resize canvas resizes control

IGN-009: Add property panel
├─ Create PropertiesPanel user control
├─ Add to MainForm (docked right)
├─ Display properties of selected elements
├─ Show (mixed) for multi-selection
├─ Wire PropertyChanged event
├─ Test: Select object → properties display
├─ Test: Edit property → updates element
└─ Test: Multi-selection shows (mixed)

IGN-010: Add layer panel - P1
├─ Create LayerPanel user control
├─ Add to MainForm (docked left)
├─ List all layers with visibility/lock toggles
├─ Allow rename, create, delete layers
├─ Track active layer
├─ Wire layer events
├─ Test: Create layer appears
├─ Test: Delete layer prompts reassignment
└─ Test: Hide layer → not rendered

IGN-011: Wire event handlers
├─ Create InitializeEventHandlers() method
├─ Wire toolbar button clicks
├─ Wire menu item clicks
├─ Wire canvas events (selection, creation, deletion)
├─ Wire property panel changes
├─ Wire layer panel changes
├─ Wire keyboard events (Escape, Ctrl+S)
├─ Test: Clicking toolbar activates tool
├─ Test: Menu items work
└─ Test: Keyboard shortcuts work

IGN-012: Implement File menu - P1
├─ File → New (create new layout)
├─ File → Open (open .takeoff file)
├─ File → Save (save to .takeoff file)
├─ File → Save As (save with new name)
├─ File → Export (CSV, Excel)
├─ File → Exit (close app)
├─ Test: New creates blank layout
├─ Test: Save/Open round-trip
├─ Test: Export creates file
└─ Test: Exit closes app

IGN-013: Implement Edit menu - P1
├─ Edit → Undo (restore previous state)
├─ Edit → Redo (reapply undo)
├─ Edit → Cut (copy + delete)
├─ Edit → Copy (copy to clipboard)
├─ Edit → Paste (paste from clipboard)
├─ Edit → Delete (delete selected)
├─ Test: Delete removes selected objects
├─ Test: Copy → Paste duplicates
├─ Test: Undo/Redo work
└─ Test: Cut → Paste moves

IGN-014: Implement View menu - P2
├─ View → Zoom In (1.25x)
├─ View → Zoom Out (0.8x)
├─ View → Zoom Fit (fit all objects)
├─ View → Show Grid (toggle grid)
├─ View → Show Rulers (toggle rulers)
├─ View → Full Screen (toggle)
├─ Test: Zoom changes visible area
├─ Test: Zoom Fit shows all objects
└─ Test: Grid toggle updates display

IGN-015: Create DomainEventPublisher
├─ Implement IDomainEventPublisher interface
├─ Define events: ElementCreated, ElementModified, ElementDeleted
├─ Define events: CalculationCompleted
├─ Register as Singleton in DI
├─ Inject into Application services
├─ Publish events at appropriate points
├─ Subscribe in MainForm for UI updates
└─ Test: Event fires on element creation

IGN-016 through IGN-019: Event Publishing & Subscription
├─ Publish ElementCreated when line drawn
├─ Publish ElementModified when property changed
├─ Publish ElementDeleted when element deleted
├─ Publish CalculationCompleted after take-off
├─ Subscribe in MainForm
├─ Update UI on each event
├─ Test: Drawing fires event
├─ Test: UI updates on event
├─ Test: Multiple subscribers work
└─ Test: No memory leaks
```

#### **Verification Checklist:**

- [ ] Unit test: DI initialization succeeds
- [ ] Unit test: MainForm initializes without errors
- [ ] Unit test: DomainEventPublisher publishes events
- [ ] Integration test: MainForm + canvas interaction
- [ ] Integration test: File → New creates layout
- [ ] Integration test: Tool button clicks activate tools
- [ ] Integration test: Draw line → event fires → UI updates
- [ ] Integration test: Property change updates element
- [ ] Integration test: Layer change updates rendering
- [ ] Integration test: Delete selected removes objects
- [ ] UI test: Select object → properties panel updates
- [ ] UI test: Draw rectangle → canvas shows shape
- [ ] UI test: Pan/zoom work correctly
- [ ] End-to-end test: Full workflow (file → draw → calculate → export)

---

# 📊 Overall Coverage Matrix

## By Layer

| Layer | Tasks | P0 | P1 | P2 | Total |
|-------|-------|----|----|----|----|
| **Foundation** | FND-001 to FND-020 | 16 | 4 | 0 | **20** |
| **Rendering** | RND-001 to RND-014 | 8 | 5 | 1 | **14** |
| **Interaction** | INT-001 to INT-014 | 10 | 4 | 0 | **14** |
| **Business** | BUS-001 to BUS-019 | 8 | 10 | 1 | **19** |
| **Integration** | IGN-001 to IGN-019 | 10 | 8 | 1 | **19** |
| **TOTAL** | | **52** | **31** | **3** | **86** |

---

## By SDLC Use Case

| Use Case | Req Ref | Foundation | Rendering | Interaction | Business | Integration |
|----------|---------|-----------|-----------|-------------|----------|-------------|
| UC-001: Draw Line | 0104-5.1.1 | FND-001-012 | RND-001-009 | INT-001-008 | — | IGN-006-011 |
| UC-002: Assign Layer | 0104-5.1.1 | FND-001-004 | RND-004 | INT-009-011 | — | IGN-009 |
| UC-003: Attach Tag | 0104-5.1.1 | FND-005-008 | RND-008 | INT-009 | — | IGN-006 |
| UC-004: Take-off Summary | 0104-5.4 | FND-001-012 | — | — | BUS-001-019 | IGN-012 |
| UC-005: Insert Symbol | 0104-5.1.1 | FND-005-008 | RND-005-009 | INT-003 | — | IGN-007 |
| UC-006: Edit Multi-Selection | 0104-5.1.1 | FND-001-012 | RND-008 | INT-009-012 | — | IGN-009 |
| UC-007: Delete Layer | 0104-5.1.1 | FND-001-004 | RND-004 | INT-009 | — | IGN-010 |
| UC-008: Deployment Mode | 0104-UC-008 | FND-015-016 | — | — | — | IGN-001-005 |

---

# ✅ Priority-Based Execution Plan

## Phase 1: MVP (P0 only)

**Target:** Working prototype with basic drawing  
**Tasks:** 52 Foundation + Rendering + Interaction + Business P0 tasks

```
Week 1-2: Foundation Layer
└─ FND-001 through FND-020 (infrastructure + core entities)

Week 3-4: Rendering + Interaction
├─ RND-001 through RND-004 (canvas basics)
├─ INT-001 through INT-008 (tool system)
└─ RND-005-009 (shape rendering)

Week 5-6: Business + Integration
├─ BUS-001 through BUS-009 (calculation)
├─ IGN-001 through IGN-005 (DI)
└─ IGN-006 through IGN-011 (forms)

Week 7: Testing & Polish
├─ Unit tests for all P0 tasks
├─ Integration tests
└─ Bug fixes
```

**Deliverable:** User can draw line, rectangle, circle; calculate quantities; export CSV

---

## Phase 2: Full Feature Set (P1)

**Target:** Complete feature set per SDLC  
**Tasks:** 31 P1 tasks

```
Week 8-9: Advanced Rendering + Selection
├─ RND-010-014 (zoom, pan, grid, hit test)
├─ INT-009-014 (multi-selection, window select, keyboard)

Week 10-11: Business Services
├─ BUS-010-019 (formulas, aggregations, export)
├─ Materials service

Week 12-13: Integration & Forms
├─ IGN-010-019 (layer panel, menus, events)
├─ File operations (Open, Save)

Week 14: Testing
├─ Acceptance tests
├─ UI tests
└─ Performance profiling
```

**Deliverable:** Complete application per SDLC requirements

---

## Phase 3: Polish & Optimization (P2)

**Target:** Production-ready  
**Tasks:** 3 P2 tasks + performance

```
Week 15-16: Performance
├─ Optimize rendering (large drawings)
├─ Optimize calculations (many elements)

Week 17: Polish
├─ Menu bar completion
├─ Dark mode support
├─ Accessibility improvements

Week 18: Final Testing
├─ Full regression test
├─ UAT with stakeholders
└─ Release preparation
```

---

# 📝 Task Tracking Template

Use this template to track implementation progress:

```markdown
## Task: FND-001 - Create CanvasLayout Entity

**Status:** 🔲 TODO | 🟨 IN PROGRESS | ✅ DONE | ❌ BLOCKED

**SDLC Reference:** 0104-SRS §5.2

**Description:** Define CanvasLayout with properties and basic methods

**Subtasks:**
- [ ] Define properties (Id, Name, Unit, ScaleFactor, Elements, Layers, ActiveLayerId)
- [ ] Implement Guid.NewGuid initialization
- [ ] Initialize collections (Elements, Layers)
- [ ] Document in code comments

**Verification:**
- [ ] Code compiles without errors
- [ ] Unit test: Create empty layout
- [ ] Unit test: Serialize to JSON
- [ ] Code review passed

**Blocked By:** None  
**Blocks:** FND-002, FND-003, FND-004  
**Assigned To:** —  
**Due Date:** —  
**Estimated Hours:** 2h  
**Actual Hours:** —  

**Notes:**
- Follow CODING_ASSISTANCE_PLAN.md guidelines
- Reference Domain/Entities/CanvasLayout.vb
```

---

# 🚀 Getting Started

1. **Read CODING_ASSISTANCE_PLAN.md** - Understand layer architecture
2. **Pick FND-001** - Start with Foundation Layer
3. **Use Task Tracking Template** - Copy for each task
4. **Reference SDLC** - Check Mega-File.md for requirements
5. **Write Unit Tests** - For every task
6. **Code Review** - Before marking DONE

---

**Document Version:** 1.0  
**Last Updated:** 2025  
**Maintained by:** Architecture Team
