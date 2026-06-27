---
aliases:
  - CoNSoL-TakeOff Component Reference � Detailed Implementation Guide
doc_id: 702
status: draft
version: 1.0.0-draft
owner: engineering
audience: AI coding agents + solo developer
related_docs:
  - 0701_CanvasControl_Technical_Architecture.md
  - Project-Example -(to-be-deleted)/ZRGPictureBox/Documents/ZRGPictureBoxControl-in-Details.md
last_updated: 2026-06
---

# ?? CoNSoL-TakeOff Component Reference � Detailed Implementation

> Companion to `0701_CanvasControl_Technical_Architecture.md`, this document provides **detailed component-by-component documentation** following the style of `ZRGPictureBoxControl-in-Details.md`. It covers individual services, controls, and entities with code snippets, workflows, and integration patterns.

---

## Table of Contents

1. [CanvasControl � Detailed Implementation](#1-canvascontrol--detailed-implementation)
2. [LayerPanel � Layer Management UI](#2-layerpanel--layer-management-ui)
3. [PropertiesPanel � Context-Sensitive Properties](#3-propertiespanel--context-sensitive-properties)
4. [LayerManager Service](#4-layermanager-service)
5. [CoordinateConverter Utility](#5-coordinateconverter-utility)
6. [TakeOffService Orchestrator](#6-takeoffservice-orchestrator)
7. [AiIntakeService � AI Pipeline](#7-aiintakeservice--ai-pipeline)
8. [Material Management](#8-material-management)
9. [Shape Model Implementations](#9-shape-model-implementations)
10. [Usage Examples & Patterns](#10-usage-examples--patterns)

---

## 1. CanvasControl — Detailed Implementation

### 1.1 File Structure & Organization

**Location:** `Desktop/Controls/CanvasControl.vb`

**Regions:**
- `Info. & Imports` — imports and file header
- `canvas control` — main class definition
- `Fields and Properties` — state management
- `Events` — event definitions
- `Mouse Handling` — mouse event handlers
- `Drawing & Rendering` — paint logic and helpers
- `Tool Implementation` — tool-specific methods
- `Serialization` — save/load support

### 1.2 Private State Variables (Detailed)

```vb
#Region "Drawing State"
	' Shape storage and selection
	Private ReadOnly _shapes As New List(Of ShapeBase)()
	Private _selected As ShapeBase = Nothing
	Private _selectedList As List(Of ShapeBase) = Nothing ' Future: multi-select

	' Drawing in-progress state
	Private _isDrawing As Boolean = False
	Private _startPt As PointF ' Logical coordinate when mouse down
	Private _currPt As PointF  ' Logical coordinate current
	Private _tempShape As ShapeBase ' Live preview during draw

	' Current tool mode
	Private _tool As ToolType = ToolType.SelectTool

	' Context menu
	Private ReadOnly _shapeMenu As New ContextMenuStrip()
#End Region

#Region "View State"
	' Layout and content
	Private _currentLayout As CanvasLayout

	' Zoom and pan
	Private _zoom As Single = 1.0F
	Private _pan As PointF = New PointF(0, 0)

	' Grid and snap
	Private _gridSize As Integer = 20
	Private _showGrid As Boolean = True
	Private _snapToGrid As Boolean = True
	Private _gridKind As GridKind = GridKind.Lines
	Private _snapEnabled As Boolean = False

	' Rulers and UI
	Private _showRulers As Boolean = True
#End Region

#Region "Rendering"
	' Double-buffer for smooth animation
	Private _backBuffer As Bitmap = Nothing
	Private _backGraphics As Graphics = Nothing

	' Background image
	Private _backgroundImage As Image = Nothing
	Private _backgroundOpacity As Single = 0.5F
#End Region

#Region "Constraints"
	Private myMinLogicalWindowSize As Size = New Size(2000, 2000)
	Private myMaxLogicalWindowSize As Size = New Size(100000000, 100000000)
#End Region
```

### 1.3 Key Method: OnMouseDown Implementation

```vb
Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
	MyBase.OnMouseDown(e)

	' Convert physical mouse coords to logical
	Dim logicalPt = PhysicalToLogical(e.Location)

	Select Case _tool
		Case ToolType.SelectTool
			' Check if clicked on existing shape
			Dim hitShape = FindShapeAtPoint(logicalPt)
			If hitShape IsNot Nothing Then
				SelectShape(hitShape)
				If e.Button = MouseButtons.Right Then
					ShowContextMenu(e.Location)
				End If
			Else
				ClearSelection()
			End If

		Case ToolType.LineTool
			' Start line drawing
			_isDrawing = True
			_startPt = logicalPt
			_tempShape = New LineShape(_startPt, _startPt)
			Invalidate()

		Case ToolType.RectTool
			' Start rectangle drawing
			_isDrawing = True
			_startPt = logicalPt
			_tempShape = New RectangleShape(_startPt, 0, 0)
			Invalidate()

		Case ToolType.PanTool
			' Store pan start point (physical coords for panning)
			_lastPanPoint = e.Location
			Cursor = Cursors.Hand

		Case ToolType.ZoomTool
			' Start zoom box
			_isDrawing = True
			_startPt = logicalPt
			Invalidate()
	End Select
End Sub
```

### 1.4 Key Method: OnMouseMove Implementation

```vb
Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
	MyBase.OnMouseMove(e)

	Dim logicalPt = PhysicalToLogical(e.Location)

	Select Case _tool
		Case ToolType.SelectTool
			' Show hand cursor when over shape
			Dim hitShape = FindShapeAtPoint(logicalPt)
			Cursor = If(hitShape IsNot Nothing, Cursors.Hand, Cursors.Default)

		Case ToolType.LineTool, ToolType.RectTool
			' Update temporary shape for live preview
			If _isDrawing AndAlso _tempShape IsNot Nothing Then
				_currPt = logicalPt

				' Apply snap if enabled
				If _snapToGrid Then
					_currPt = SnapToGrid(_currPt)
				End If

				If TypeOf _tempShape Is LineShape Then
					CType(_tempShape, LineShape).EndPoint = _currPt
				ElseIf TypeOf _tempShape Is RectangleShape Then
					Dim rect = CType(_tempShape, RectangleShape)
					rect.Width = Math.Abs(_currPt.X - _startPt.X)
					rect.Height = Math.Abs(_currPt.Y - _startPt.Y)
				End If

				Invalidate()
			End If

		Case ToolType.PanTool
			' Pan the view
			If _isDrawing Then
				Dim deltaX = e.Location.X - _lastPanPoint.X
				Dim deltaY = e.Location.Y - _lastPanPoint.Y
				_pan = New PointF(_pan.X + deltaX, _pan.Y + deltaY)
				_lastPanPoint = e.Location
				Invalidate()
			End If

		Case ToolType.ZoomTool
			' Update zoom box preview
			If _isDrawing Then
				_currPt = logicalPt
				Invalidate()
			End If
	End Select
End Sub
```

### 1.5 Key Method: OnMouseUp Implementation

```vb
Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
	MyBase.OnMouseUp(e)

	Dim logicalPt = PhysicalToLogical(e.Location)

	Select Case _tool
		Case ToolType.LineTool
			If _isDrawing AndAlso _tempShape IsNot Nothing Then
				Dim line = CType(_tempShape, LineShape)

				' Validate: not degenerate
				If line.GetLength() > 0.1 Then ' Tolerance for floating point
					_shapes.Add(line)
					_currentLayout.AddElement(line.ToElement())
					RaiseEvent ShapeCreated(line)
				End If

				_isDrawing = False
				_tempShape = Nothing
				Invalidate()
			End If

		Case ToolType.RectTool
			If _isDrawing AndAlso _tempShape IsNot Nothing Then
				Dim rect = CType(_tempShape, RectangleShape)

				' Validate: not degenerate
				If rect.Width > 1 AndAlso rect.Height > 1 Then
					_shapes.Add(rect)
					_currentLayout.AddElement(rect.ToElement())
					RaiseEvent ShapeCreated(rect)
				End If

				_isDrawing = False
				_tempShape = Nothing
				Invalidate()
			End If

		Case ToolType.ZoomTool
			' Zoom to selected area
			If _isDrawing Then
				Dim rect = New RECT(CInt(_startPt.X), CInt(_startPt.Y), CInt(_currPt.X), CInt(_currPt.Y))
				rect.NormalizeRect()

				If Not rect.IsZeroSized Then
					ZoomToLogicalArea(rect)
				End If

				_isDrawing = False
				Invalidate()
			End If

		Case ToolType.PanTool
			Cursor = Cursors.Default
	End Select
End Sub
```

### 1.6 Paint/Rendering Method (Double-Buffer Composition)

```vb
Protected Overrides Sub OnPaint(e As PaintEventArgs)
	' Step 1: Ensure backbuffer exists and matches client size
	If _backBuffer Is Nothing OrElse _backBuffer.Width <> ClientSize.Width OrElse _backBuffer.Height <> ClientSize.Height Then
		If _backGraphics IsNot Nothing Then _backGraphics.Dispose()
		If _backBuffer IsNot Nothing Then _backBuffer.Dispose()

		_backBuffer = New Bitmap(ClientSize.Width, ClientSize.Height)
		_backGraphics = Graphics.FromImage(_backBuffer)
		_backGraphics.SmoothingMode = SmoothingMode.AntiAlias
	End If

	' Step 2: Clear background
	_backGraphics.Clear(Color.White)

	' Step 3: Draw layers in order

	' Layer 1: Background image
	If _backgroundImage IsNot Nothing AndAlso _showBackgroundImage Then
		DrawBackgroundImage(_backGraphics)
	End If

	' Layer 2: Grid
	If _showGrid Then
		DrawGrid(_backGraphics)
	End If

	' Layer 3: Rulers (if enabled)
	If _showRulers Then
		DrawRulers(_backGraphics)
	End If

	' Layer 4: Committed shapes (filtered by visibility)
	For Each shape In _shapes
		If IsShapeVisible(shape) Then
			DrawShape(_backGraphics, shape, highlighted:=False)
		End If
	Next

	' Layer 5: Temporary shape (live preview during drawing)
	If _tempShape IsNot Nothing Then
		DrawShape(_backGraphics, _tempShape, highlighted:=True)
	End If

	' Layer 6: Selection highlight
	If _selected IsNot Nothing Then
		DrawSelectionHighlight(_backGraphics, _selected)
	End If

	' Layer 7: Zoom box preview
	If _tool = ToolType.ZoomTool AndAlso _isDrawing Then
		DrawZoomBox(_backGraphics)
	End If

	' Layer 8: Measurement ruler (if active)
	If _tool = ToolType.MeasureTool Then
		DrawMeasurementOverlay(_backGraphics)
	End If

	' Step 4: Composite backbuffer to screen
	e.Graphics.DrawImageUnscaled(_backBuffer, 0, 0)

	MyBase.OnPaint(e)
End Sub

Private Sub DrawShape(g As Graphics, shape As ShapeBase, Optional highlighted As Boolean = False)
	Dim pen As Pen

	If highlighted Then
		pen = New Pen(Color.Red, 2)
	Else
		Dim layer = _currentLayout.GetLayer(shape.LayerId)
		Dim layerColor = ColorTranslator.FromHtml(layer.Color)
		pen = New Pen(layerColor, 1)
	End If

	shape.Draw(g, _zoom, _pan, _currentLayout)
	pen.Dispose()
End Sub

Private Sub DrawGrid(g As Graphics)
	If _gridSize <= 0 Then Return

	' Convert grid size to physical pixels
	Dim gridPixels = _gridSize * _zoom
	If gridPixels < 2 Then Return ' Too dense, skip

	Dim gridPen = New Pen(Color.LightGray, 0.5F)

	' Calculate visible range in logical coords
	Dim visibleLogicalRect = GetVisibleLogicalRect()

	' Draw grid based on _gridKind
	Select Case _gridKind
		Case GridKind.Lines
			' Draw grid lines at _gridSize intervals
			Dim x = CInt(Math.Ceiling(visibleLogicalRect.Left / _gridSize) * _gridSize)
			While x < visibleLogicalRect.Right
				Dim physicalX = CInt((x - _currentLayout.LogicalOrigin.X) * _zoom + _pan.X)
				g.DrawLine(gridPen, physicalX, 0, physicalX, ClientSize.Height)
				x += _gridSize
			End While

			Dim y = CInt(Math.Ceiling(visibleLogicalRect.Top / _gridSize) * _gridSize)
			While y < visibleLogicalRect.Bottom
				Dim physicalY = CInt((y - _currentLayout.LogicalOrigin.Y) * _zoom + _pan.Y)
				g.DrawLine(gridPen, 0, physicalY, ClientSize.Width, physicalY)
				y += _gridSize
			End While

		Case GridKind.Points
			' Draw grid points
			Dim x = CInt(Math.Ceiling(visibleLogicalRect.Left / _gridSize) * _gridSize)
			While x < visibleLogicalRect.Right
				Dim y = CInt(Math.Ceiling(visibleLogicalRect.Top / _gridSize) * _gridSize)
				While y < visibleLogicalRect.Bottom
					Dim physicalPt = LogicalToPhysical(New PointF(x, y))
					g.FillEllipse(Brushes.Gray, physicalPt.X - 1, physicalPt.Y - 1, 2, 2)
					y += _gridSize
				End While
				x += _gridSize
			End While
	End Select

	gridPen.Dispose()
End Sub

Private Sub DrawSelectionHighlight(g As Graphics, shape As ShapeBase)
	Dim boundingBox = shape.GetBoundingBox()
	Dim physicalRect = LogicalRectToPhysical(boundingBox)

	' Draw selection rectangle
	Dim selPen = New Pen(Color.Blue, 2)
	g.DrawRectangle(selPen, physicalRect)
	selPen.Dispose()

	' Draw corner and edge handles
	Const HandleSize = 6
	Dim handles = New Rectangle() {
		New Rectangle(physicalRect.Left - HandleSize/2, physicalRect.Top - HandleSize/2, HandleSize, HandleSize),
		New Rectangle(physicalRect.Right - HandleSize/2, physicalRect.Top - HandleSize/2, HandleSize, HandleSize),
		New Rectangle(physicalRect.Left - HandleSize/2, physicalRect.Bottom - HandleSize/2, HandleSize, HandleSize),
		New Rectangle(physicalRect.Right - HandleSize/2, physicalRect.Bottom - HandleSize/2, HandleSize, HandleSize)
	}

	For Each handle In handles
		g.FillRectangle(Brushes.Blue, handle)
		g.DrawRectangle(Pens.White, handle)
	Next
End Sub
```

### 1.7 Coordinate Conversion Helpers

```vb
Private Function PhysicalToLogical(physicalPt As PointF) As PointF
	Dim logicalX = (physicalPt.X - _pan.X) / _zoom + _currentLayout.LogicalOrigin.X
	Dim logicalY = (physicalPt.Y - _pan.Y) / _zoom + _currentLayout.LogicalOrigin.Y
	Return New PointF(logicalX, logicalY)
End Function

Private Function LogicalToPhysical(logicalPt As PointF) As PointF
	Dim physicalX = (logicalPt.X - _currentLayout.LogicalOrigin.X) * _zoom + _pan.X
	Dim physicalY = (logicalPt.Y - _currentLayout.LogicalOrigin.Y) * _zoom + _pan.Y
	Return New PointF(physicalX, physicalY)
End Function

Private Function LogicalRectToPhysical(logicalRect As RECT) As Rectangle
	Dim topLeft = LogicalToPhysical(New PointF(logicalRect.Left, logicalRect.Top))
	Dim bottomRight = LogicalToPhysical(New PointF(logicalRect.Right, logicalRect.Bottom))
	Return New Rectangle(CInt(topLeft.X), CInt(topLeft.Y), CInt(bottomRight.X - topLeft.X), CInt(bottomRight.Y - topLeft.Y))
End Function

Private Function SnapToGrid(logicalPt As PointF) As PointF
	If Not _snapToGrid OrElse _gridSize <= 0 Then
		Return logicalPt
	End If

	Dim snappedX = CInt(Math.Round(logicalPt.X / _gridSize) * _gridSize)
	Dim snappedY = CInt(Math.Round(logicalPt.Y / _gridSize) * _gridSize)
	Return New PointF(snappedX, snappedY)
End Function

Private Function FindShapeAtPoint(logicalPt As PointF) As ShapeBase
	' Check shapes in reverse order (top to bottom, last drawn is topmost)
	For i = _shapes.Count - 1 To 0 Step -1
		If _shapes(i).Contains(logicalPt) Then
			Return _shapes(i)
		End If
	Next
	Return Nothing
End Function

Private Function GetVisibleLogicalRect() As RECT
	' Compute the logical rectangle currently visible on screen
	Dim topLeft = PhysicalToLogical(New PointF(0, 0))
	Dim bottomRight = PhysicalToLogical(New PointF(ClientSize.Width, ClientSize.Height))
	Return New RECT(CInt(topLeft.X), CInt(topLeft.Y), CInt(bottomRight.X), CInt(bottomRight.Y))
End Function
```

### 1.8 Selection and Shape Management

```vb
Public Sub SelectShape(shape As ShapeBase)
	If _selected <> shape Then
		_selected = shape
		RaiseEvent SelectionChanged(_selected)
		Invalidate()
	End If
End Sub

Public Sub ClearSelection()
	If _selected IsNot Nothing Then
		_selected = Nothing
		RaiseEvent SelectionChanged(Nothing)
		Invalidate()
	End If
End Sub

Public Function DeleteSelectedShape() As Boolean
	If _selected Is Nothing Then
		Return False
	End If

	If _shapes.Remove(_selected) Then
		_currentLayout.RemoveElement(_selected.Id)
		ClearSelection()
		RaiseEvent LayoutChanged(_currentLayout)
		Invalidate()
		Return True
	End If

	Return False
End Function

Private Sub IsShapeVisible(shape As ShapeBase) As Boolean
	If _showOnlyActiveLayer Then
		Dim activeLayer = _currentLayout.GetActiveLayer()
		If shape.LayerId <> activeLayer.Id Then
			Return False
		End If
	End If

	Dim layer = _currentLayout.GetLayer(shape.LayerId)
	Return layer IsNot Nothing AndAlso layer.IsVisible
End Sub
```

### 1.9 Events

```vb
#Region "Events"
	Public Event ShapeCreated(shape As ShapeBase)
	Public Event SelectionChanged(shape As ShapeBase)
	Public Event ToolChanged(tool As ToolType)
	Public Event LayoutChanged(layout As CanvasLayout)
	Public Event ShowContextMenuRequired(location As Point)
#End Region
```

**Event Usage Example (MainForm):**

```vb
Private Sub Canvas_ShapeCreated(sender As Object, e As ShapeBase) Handles _canvas.ShapeCreated
	' Update layer panel object count
	Dim layer = _layerManager.GetLayer(e.LayerId)
	layer.ObjectCount += 1
	_layerPanel.RefreshLayer(layer)

	' Update status bar
	_statusBar.Text = $"Shape created: {e.GetType().Name} on layer {layer.Name}"
End Sub

Private Sub Canvas_SelectionChanged(sender As Object, e As ShapeBase) Handles _canvas.SelectionChanged
	' Update properties panel
	If e Is Nothing Then
		_propertiesPanel.DisplayNothing()
	Else
		_propertiesPanel.DisplayShape(e)
	End If

	' Update toolbar
	_toolbarDeleteButton.Enabled = (e IsNot Nothing)
End Sub
```

---

## 2. LayerPanel — Layer Management UI

### 2.1 Purpose

`LayerPanel` (Desktop/Controls/LayerPanel.vb) is a `UserControl` that displays the layer list with:
- Visibility toggle (eye icon)
- Lock toggle (lock icon)
- Active layer indicator
- Layer color preview
- Object count per layer
- Add/Delete/Duplicate buttons

### 2.2 Main Members

```vb
Public Class LayerPanel
	Inherits UserControl

	Private ReadOnly _layerListBox As New ListBox()
	Private _layerManager As LayerManager = Nothing
	Private _selectedLayer As Layer = Nothing

	' Events
	Public Event ActiveLayerChanged(layer As Layer)
	Public Event LayerVisibilityToggled(layerId As Guid)
	Public Event LayerLockToggled(layerId As Guid)
	Public Event LayerDeleted(layerId As Guid)
End Class
```

### 2.3 Key Methods

```vb
Public Sub SetLayerManager(manager As LayerManager)
	_layerManager = manager
	RefreshLayerList()
	AddHandler _layerManager.LayersChanged, AddressOf OnLayersChanged
End Sub

Private Sub RefreshLayerList()
	_layerListBox.Items.Clear()

	For Each layer In _layerManager.GetAllLayers()
		Dim item = New LayerListItem With {
			.Layer = layer,
			.IsActive = (layer.Id = _layerManager.GetActiveLayer().Id)
		}
		_layerListBox.Items.Add(item)
	Next
End Sub

Private Sub OnLayerDoubleClick(layerId As Guid)
	' Set as active layer
	_layerManager.SetActiveLayer(layerId)
	RefreshLayerList()
	RaiseEvent ActiveLayerChanged(_layerManager.GetLayer(layerId))
End Sub

Public Sub OnVisibilityToggle(layerId As Guid)
	Dim layer = _layerManager.GetLayer(layerId)
	If layer IsNot Nothing Then
		layer.IsVisible = Not layer.IsVisible
		RefreshLayerList()
		RaiseEvent LayerVisibilityToggled(layerId)
	End If
End Sub

Public Sub OnLockToggle(layerId As Guid)
	Dim layer = _layerManager.GetLayer(layerId)
	If layer IsNot Nothing Then
		layer.IsLocked = Not layer.IsLocked
		RefreshLayerList()
		RaiseEvent LayerLockToggled(layerId)
	End If
End Sub

Public Sub OnDeleteLayer(layerId As Guid)
	' UC-007: Delete layer with reassignment dialog
	If _layerManager.GetAllLayers().Count <= 1 Then
		MessageBox.Show("Cannot delete the last layer")
		Return
	End If

	Dim result = MessageBox.Show("Delete layer? Choose action:", "Delete Layer", MessageBoxButtons.YesNoCancel)
	If result = DialogResult.Yes Then
		' Show reassignment dialog
		Dim reassignForm = New LayerReassignmentDialog(_layerManager)
		If reassignForm.ShowDialog() = DialogResult.OK Then
			_layerManager.DeleteLayer(layerId, reassignForm.SelectedLayerId)
			RefreshLayerList()
			RaiseEvent LayerDeleted(layerId)
		End If
	End If
End Sub
```

---

## 3. PropertiesPanel — Context-Sensitive Properties

### 3.1 Purpose

`PropertiesPanel` (Desktop/Controls/PropertiesPanel.vb) displays and edits properties for:
- Nothing selected ? canvas properties
- Single shape ? full properties
- Multi-selection same type ? shared properties + `(mixed)` placeholders
- Multi-selection mixed types ? universal only

### 3.2 Context Sensitivity Logic

```vb
Public Class PropertiesPanel
	Inherits UserControl

	Private _currentMode As ContextMode = ContextMode.Nothing
	Private _selectedShapes As List(Of ShapeBase) = New List(Of ShapeBase)()

	Enum ContextMode
		Nothing
		CanvasProperties
		SingleShape
		MultiSameType
		MultiMixedType
	End Enum

	Public Sub DisplayShape(shape As ShapeBase)
		_currentMode = ContextMode.SingleShape
		_selectedShapes = New List(Of ShapeBase) From {shape}

		' Show all properties for this shape type
		DisplayCommonProperties(shape)
		DisplayTypeSpecificProperties(shape)
	End Sub

	Public Sub DisplayShapes(shapes As List(Of ShapeBase))
		If shapes.Count = 0 Then
			DisplayNothing()
			Return
		End If

		If shapes.Count = 1 Then
			DisplayShape(shapes(0))
			Return
		End If

		_selectedShapes = shapes

		' Check if all same type
		Dim allSameType = shapes.Skip(1).All(Function(s) s.GetType() = shapes(0).GetType())

		If allSameType Then
			_currentMode = ContextMode.MultiSameType
			DisplayMultiSameProperties(shapes)
		Else
			_currentMode = ContextMode.MultiMixedType
			DisplayUniversalPropertiesOnly(shapes)
		End If
	End Sub

	Private Sub DisplayCommonProperties(shape As ShapeBase)
		' Layer
		_layerDropdown.SelectedValue = shape.LayerId

		' Logical 3D (if applicable)
		If shape.HasLogical3D Then
			_heightTextBox.Text = shape.LogicalHeight.ToString()
			_widthTextBox.Text = shape.LogicalWidth.ToString()
			_lengthTextBox.Text = shape.LogicalLength.ToString()
			_quantityTextBox.Text = shape.Quantity.ToString()
			_unitPriceTextBox.Text = shape.UnitPrice.ToString("0.00")
			_totalCostLabel.Text = (shape.Quantity * shape.UnitPrice).ToString("0.00")
		End If
	End Sub

	Private Sub DisplayTypeSpecificProperties(shape As ShapeBase)
		Select Case shape.GetType()
			Case GetType(LineShape)
				Dim line = CType(shape, LineShape)
				_startXTextBox.Text = line.StartPoint.X.ToString("0.0")
				_startYTextBox.Text = line.StartPoint.Y.ToString("0.0")
				_endXTextBox.Text = line.EndPoint.X.ToString("0.0")
				_endYTextBox.Text = line.EndPoint.Y.ToString("0.0")
				_lengthLabel.Text = $"{line.GetLength():0.00}"

			Case GetType(RectangleShape)
				Dim rect = CType(shape, RectangleShape)
				_topLeftXTextBox.Text = rect.TopLeft.X.ToString("0.0")
				_topLeftYTextBox.Text = rect.TopLeft.Y.ToString("0.0")
				_widthTextBox.Text = rect.Width.ToString("0.0")
				_heightTextBox.Text = rect.Height.ToString("0.0")
				_areaLabel.Text = $"{rect.GetArea():0.00}"
		End Select
	End Sub

	Private Sub DisplayMultiSameProperties(shapes As List(Of ShapeBase))
		' Show shared properties
		DisplayCommonProperties(shapes(0))

		' Check for mixed values
		Dim firstLayerId = shapes(0).LayerId
		If shapes.Any(Function(s) s.LayerId <> firstLayerId) Then
			_layerDropdown.Text = "(mixed)"
		End If

		' Type-specific: show if consistent, otherwise (mixed)
		If TypeOf shapes(0) Is LineShape Then
			Dim lines = shapes.Cast(Of LineShape)().ToList()
			Dim firstStart = lines(0).StartPoint

			If lines.All(Function(l) l.StartPoint = firstStart) Then
				_startXTextBox.Text = firstStart.X.ToString("0.0")
				_startYTextBox.Text = firstStart.Y.ToString("0.0")
			Else
				_startXTextBox.Text = "(mixed)"
				_startYTextBox.Text = "(mixed)"
			End If
		End If
	End Sub

	Private Sub DisplayUniversalPropertiesOnly(shapes As List(Of ShapeBase))
		' Only show universal properties: Layer, visibility, lock, notes
		Dim firstLayerId = shapes(0).LayerId
		If shapes.Any(Function(s) s.LayerId <> firstLayerId) Then
			_layerDropdown.Text = "(mixed)"
		Else
			_layerDropdown.SelectedValue = firstLayerId
		End If

		' Hide type-specific fields
		_typeSpecificPanel.Visible = False
	End Sub

	Public Sub DisplayNothing()
		_currentMode = ContextMode.Nothing
		_selectedShapes.Clear()
		ClearAllFields()
		_propertiesGroupBox.Text = "Properties"
	End Sub

	Private Sub OnLayerDropdownChanged()
		If _selectedShapes.Count = 0 Then Return

		Dim newLayerId = CGuid(_layerDropdown.SelectedValue)
		For Each shape In _selectedShapes
			shape.LayerId = newLayerId
		Next

		RaiseEvent PropertiesChanged()
	End Sub

	Private Sub OnHeightChanged()
		If _selectedShapes.Count = 0 Then Return
		If Not Double.TryParse(_heightTextBox.Text, _height) Then Return

		For Each shape In _selectedShapes.Where(Function(s) s.HasLogical3D)
			shape.LogicalHeight = _height
		Next

		' Update auto-calculated fields
		UpdateTotalCost()
		RaiseEvent PropertiesChanged()
	End Sub
End Class
```

---

## 4. LayerManager Service

### 4.1 Purpose

`LayerManager` (Domain/Services/LayerManager.vb) manages layer collections, active state, and layer-related operations.

### 4.2 Implementation

```vb
Public Class LayerManager
	Private ReadOnly _layers As New List(Of Layer)()
	Private _activeLayer As Layer = Nothing

	Public Event LayersChanged()
	Public Event ActiveLayerChanged(layer As Layer)

	Public Sub New()
		' Create default layer
		Dim defaultLayer = New Layer With {
			.Name = "Default",
			.IsVisible = True,
			.IsLocked = False,
			.IncludeInCalculation = True
		}
		_layers.Add(defaultLayer)
		_activeLayer = defaultLayer
	End Sub

	Public Function GetAllLayers() As List(Of Layer)
		Return _layers.ToList()
	End Function

	Public Function GetLayer(layerId As Guid) As Layer
		Return _layers.FirstOrDefault(Function(l) l.Id = layerId)
	End Function

	Public Function GetActiveLayer() As Layer
		Return _activeLayer
	End Function

	Public Sub SetActiveLayer(layerId As Guid)
		Dim layer = GetLayer(layerId)
		If layer Is Nothing Then
			Throw New ArgumentException($"Layer {layerId} not found")
		End If

		_activeLayer = layer
		RaiseEvent ActiveLayerChanged(layer)
		RaiseEvent LayersChanged()
	End Sub

	Public Sub AddLayer(name As String) As Layer
		Dim layer = New Layer With {.Name = name}
		layer.Validate()

		_layers.Add(layer)
		RaiseEvent LayersChanged()
		Return layer
	End Sub

	Public Sub DeleteLayer(layerId As Guid, reassignLayerId As Guid)
		' Validate
		If layerId = _activeLayer.Id Then
			Throw New InvalidOperationException("Cannot delete active layer")
		End If

		If _layers.Count <= 1 Then
			Throw New InvalidOperationException("Cannot delete last layer")
		End If

		' Reassign shapes (done by caller)
		_layers.RemoveAll(Function(l) l.Id = layerId)
		RaiseEvent LayersChanged()
	End Sub

	Public Function GetVisibleLayers() As List(Of Layer)
		Return _layers.Where(Function(l) l.IsVisible).ToList()
	End Function

	Public Function GetCalculationLayers() As List(Of Layer)
		Return _layers.Where(Function(l) l.IncludeInCalculation).ToList()
	End Function
End Class
```

---

## 5. CoordinateConverter Utility

### 5.1 Single Responsibility

`CoordinateConverter` (Domain/Utilities/CoordinateConverter.vb) handles **only** the mathematical transformation between coordinate spaces. It has no UI dependencies.

### 5.2 Implementation

```vb
Public Class CoordinateConverter
	Public Shared Function ToLogical(
		physicalPt As PointF,
		zoom As Single,
		pan As PointF,
		layout As CanvasLayout) As PointF

		If zoom <= 0 Then
			Throw New ArgumentException("Zoom must be > 0")
		End If

		Dim logicalX = (physicalPt.X - pan.X) / zoom + layout.LogicalOrigin.X
		Dim logicalY = (physicalPt.Y - pan.Y) / zoom + layout.LogicalOrigin.Y
		Return New PointF(logicalX, logicalY)
	End Function

	Public Shared Function ToPhysical(
		logicalPt As PointF,
		zoom As Single,
		pan As PointF,
		layout As CanvasLayout) As PointF

		If zoom <= 0 Then
			Throw New ArgumentException("Zoom must be > 0")
		End If

		Dim physicalX = (logicalPt.X - layout.LogicalOrigin.X) * zoom + pan.X
		Dim physicalY = (logicalPt.Y - layout.LogicalOrigin.Y) * zoom + pan.Y
		Return New PointF(physicalX, physicalY)
	End Function

	Public Shared Function ToLogicalRect(
		physicalRect As Rectangle,
		zoom As Single,
		pan As PointF,
		layout As CanvasLayout) As RECT

		Dim topLeft = ToLogical(New PointF(physicalRect.Left, physicalRect.Top), zoom, pan, layout)
		Dim bottomRight = ToLogical(New PointF(physicalRect.Right, physicalRect.Bottom), zoom, pan, layout)

		Return New RECT(CInt(topLeft.X), CInt(topLeft.Y), CInt(bottomRight.X), CInt(bottomRight.Y))
	End Function

	Public Shared Function ToPhysicalRect(
		logicalRect As RECT,
		zoom As Single,
		pan As PointF,
		layout As CanvasLayout) As Rectangle

		Dim topLeft = ToPhysical(New PointF(logicalRect.Left, logicalRect.Top), zoom, pan, layout)
		Dim bottomRight = ToPhysical(New PointF(logicalRect.Right, logicalRect.Bottom), zoom, pan, layout)

		Return New Rectangle(CInt(topLeft.X), CInt(topLeft.Y), CInt(bottomRight.X - topLeft.X), CInt(bottomRight.Y - topLeft.Y))
	End Function

	' Test symmetry
	Public Shared Sub AssertSymmetry(logicalPt As PointF, zoom As Single, pan As PointF, layout As CanvasLayout)
		Dim physical = ToPhysical(logicalPt, zoom, pan, layout)
		Dim backToLogical = ToLogical(physical, zoom, pan, layout)

		Dim tolerance = 0.001F
		If Math.Abs(backToLogical.X - logicalPt.X) > tolerance OrElse Math.Abs(backToLogical.Y - logicalPt.Y) > tolerance Then
			Throw New InvalidOperationException("Coordinate conversion not symmetric")
		End If
	End Sub
End Class
```

---

## 6. TakeOffService Orchestrator

### 6.1 Responsibilities

`TakeOffService` (Application/Services/TakeOffService.vb) orchestrates calculation, aggregation, and export workflows.

### 6.2 Core Methods

```vb
Public Class TakeOffService
	Private ReadOnly _calculator As TakeOffCalculator
	Private ReadOnly _materialService As MaterialService
	Private ReadOnly _logger As ILogger

	Public Function GetSummary(
		layout As CanvasLayout,
		groupBy As AggregationMode,
		Optional filterByLayer As Guid? = Nothing,
		Optional filterByType As ShapeType? = Nothing) As TakeOffResult

		_logger.LogInfo($"Computing take-off summary: groupBy={groupBy}, filterLayer={filterByLayer}")

		Dim result = New TakeOffResult()
		Dim relevantElements = FilterElements(layout.Elements, filterByLayer, filterByType)

		For Each element In relevantElements
			Dim qty = _calculator.Calculate(element, layout)
			Dim groupKey = GetGroupKey(element, groupBy)

			If Not result.Groups.ContainsKey(groupKey) Then
				result.Groups(groupKey) = New TakeOffGroup With {.Key = groupKey}
			End If

			Dim group = result.Groups(groupKey)
			group.TotalQuantity += qty.Quantity
			group.Unit = qty.Unit
			group.Items.Add(element)
		Next

		_logger.LogInfo($"Take-off complete: {result.Groups.Count} groups")
		Return result
	End Function

	Public Function GetCostSummary(layout As CanvasLayout) As CostBreakdown
		_logger.LogInfo("Computing cost summary")

		Dim result = New CostBreakdown()

		For Each element In layout.Elements
			Dim qty = _calculator.Calculate(element, layout)
			Dim material = _materialService.GetMaterial(element.MaterialRef)

			If material IsNot Nothing Then
				Dim cost = qty.Quantity * material.UnitPrice
				result.TotalCost += cost

				result.LineItems.Add(New LineItem With {
					.Description = material.Name,
					.Quantity = qty.Quantity,
					.Unit = qty.Unit,
					.UnitPrice = material.UnitPrice,
					.TotalPrice = cost
				})
			End If
		Next

		_logger.LogInfo($"Cost summary complete: Total={result.TotalCost}")
		Return result
	End Function

	Private Function FilterElements(
		elements As List(Of DrawingElement),
		filterByLayer As Guid?,
		filterByType As ShapeType?) As List(Of DrawingElement)

		Dim filtered = elements

		If filterByLayer.HasValue Then
			filtered = filtered.Where(Function(e) e.LayerId = filterByLayer.Value).ToList()
		End If

		If filterByType.HasValue Then
			filtered = filtered.Where(Function(e) e.Type = filterByType.Value).ToList()
		End If

		Return filtered
	End Function
End Class
```

---

## 7. AiIntakeService → AI Pipeline

### 7.1 Pipeline Orchestration

```vb
Public Class AiIntakeService
	Private ReadOnly _importService As IDrawingImportService
	Private ReadOnly _ocrService As IOcrService
	Private ReadOnly _scaleDetector As IScaleDetectionService
	Private ReadOnly _geometryDetector As IGeometryDetectionService
	Private ReadOnly _classifier As IObjectClassificationService
	Private ReadOnly _logger As ILogger

	Public Async Function IntakeDrawingAsync(
		filePath As String,
		scaleHint As Single? = Nothing) As Task(Of AiIntakeResult)

		_logger.LogInfo($"Starting AI intake: {filePath}")

		' Step 1: Load source
		_logger.LogInfo("Step 1: Loading image")
		Dim importResult = Await _importService.LoadAsync(filePath)

		' Step 2: OCR
		_logger.LogInfo("Step 2: Extracting text")
		Dim ocrResult = Await _ocrService.ExtractAsync(importResult.Image)

		' Step 3: Scale detection
		_logger.LogInfo("Step 3: Detecting scale")
		Dim scaleResult = If(scaleHint.HasValue,
			New ScaleDetectionResult With {.ScaleFactor = scaleHint.Value, .Confidence = 100, .RequiresUserConfirmation = False},
			Await _scaleDetector.DetectAsync(importResult.Image, ocrResult.ExtractedText))

		If scaleResult.RequiresUserConfirmation Then
			_logger.LogWarning($"Scale detection requires user confirmation: {scaleResult.ScaleFactor} px/mm (confidence: {scaleResult.Confidence}%)")
		End If

		' Step 4: Geometry detection
		_logger.LogInfo("Step 4: Detecting geometry")
		Dim geometryResult = Await _geometryDetector.DetectAsync(importResult.Image, scaleResult.ScaleFactor)
		_logger.LogInfo($"Detected {geometryResult.Candidates.Count} candidate shapes")

		' Step 5: Classification
		_logger.LogInfo("Step 5: Classifying objects")
		Dim classificationResult = Await _classifier.ClassifyAsync(geometryResult.Candidates, ocrResult)

		' Step 6: Package for review
		Dim reviewPackage = New AiIntakeResult With {
			.ImportSession = importResult,
			.OcrResults = ocrResult,
			.ScaleDetection = scaleResult,
			.GeometryCandidates = geometryResult.Candidates,
			.Classifications = classificationResult
		}

		_logger.LogInfo("AI intake complete, ready for user review")
		Return reviewPackage
	End Function

	Public Sub AcceptCandidate(candidate As GeometryCandidate, layout As CanvasLayout)
		_logger.LogInfo($"Accepting candidate: {candidate.Type}, confidence={candidate.Confidence}%")

		Dim element = New DrawingElement With {
			.Id = Guid.NewGuid(),
			.Type = candidate.Type,
			.GeometryJson = JsonConvert.SerializeObject(candidate.Geometry),
			.DimensionMode = InferDimensionMode(candidate.Type)
		}

		layout.AddElement(element)
	End Sub

	Private Function InferDimensionMode(shapeType As ShapeType) As DimensionMode
		Select Case shapeType
			Case ShapeType.Line
				Return DimensionMode.D1
			Case ShapeType.Rectangle, ShapeType.Circle
				Return DimensionMode.D2
			Case Else
				Return DimensionMode.D0
		End Select
	End Function
End Class
```

---

## 8. Material Management

### 8.1 Material Entity

```vb
Public Class Material
	Public Property Id As String
	Public Property Name As String
	Public Property Unit As String
	Public Property UnitPrice As Decimal
	Public Property Category As String

	Public Sub Validate()
		If String.IsNullOrWhiteSpace(Name) Then
			Throw New ValidationException("Material name is required")
		End If
		If UnitPrice < 0 Then
			Throw New ValidationException("Unit price must be >= 0")
		End If
	End Sub
End Class
```

### 8.2 MaterialService

```vb
Public Class MaterialService
	Private ReadOnly _store As MaterialJsonStore
	Private _materialsCache As List(Of Material) = Nothing

	Public Function GetMaterial(materialId As String) As Material
		LoadCache()
		Return _materialsCache.FirstOrDefault(Function(m) m.Id = materialId)
	End Function

	Public Sub SaveMaterial(material As Material)
		material.Validate()
		LoadCache()

		Dim existing = _materialsCache.FirstOrDefault(Function(m) m.Id = material.Id)
		If existing IsNot Nothing Then
			_materialsCache.Remove(existing)
		End If

		_materialsCache.Add(material)
		_store.SaveMaterials(_materialsCache)
	End Sub

	Private Sub LoadCache()
		If _materialsCache Is Nothing Then
			_materialsCache = _store.LoadMaterials()
		End If
	End Sub
End Class
```

---

## 9. Shape Model Implementations

### 9.1 ShapeBase Abstract Class

```vb
Public MustInherit Class ShapeBase
	Public Property Id As Guid = Guid.NewGuid()
	Public Property LayerId As Guid
	Public Property Type As ShapeType

	' Logical 3D properties (optional)
	Public Property LogicalHeight As Double = 0
	Public Property LogicalWidth As Double = 0
	Public Property LogicalLength As Double = 0
	Public Property Quantity As Integer = 1
	Public Property UnitPrice As Decimal = 0
	Public Property HasLogical3D As Boolean = False

	Public MustOverride Function GetBoundingBox() As RECT
	Public MustOverride Sub Draw(g As Graphics, zoom As Single, pan As PointF, layout As CanvasLayout, Optional highlighted As Boolean = False)
	Public MustOverride Function Contains(logicalPt As PointF) As Boolean
	Public MustOverride Function ToElement() As DrawingElement
End Class
```

### 9.2 LineShape Implementation

```vb
Public Class LineShape
	Inherits ShapeBase

	Public Property StartPoint As PointF
	Public Property EndPoint As PointF

	Public Sub New(startPt As PointF, endPt As PointF)
		MyBase.New()
		StartPoint = startPt
		EndPoint = endPt
		Type = ShapeType.Line
	End Sub

	Public Overrides Function GetBoundingBox() As RECT
		Return New RECT(
			CInt(Math.Min(StartPoint.X, EndPoint.X)),
			CInt(Math.Min(StartPoint.Y, EndPoint.Y)),
			CInt(Math.Max(StartPoint.X, EndPoint.X)),
			CInt(Math.Max(StartPoint.Y, EndPoint.Y)))
	End Function

	Public Overrides Sub Draw(g As Graphics, zoom As Single, pan As PointF, layout As CanvasLayout, Optional highlighted As Boolean = False)
		Dim physStart = CoordinateConverter.ToPhysical(StartPoint, zoom, pan, layout)
		Dim physEnd = CoordinateConverter.ToPhysical(EndPoint, zoom, pan, layout)
		Dim pen = If(highlighted, New Pen(Color.Red, 2), New Pen(Color.Black, 1))
		g.DrawLine(pen, physStart, physEnd)
		pen.Dispose()
	End Sub

	Public Overrides Function Contains(logicalPt As PointF) As Boolean
		Return SEGMENT.PointOnSegment(logicalPt, StartPoint, EndPoint, tolerance:=10)
	End Function

	Public Function GetLength() As Double
		Return Math.Sqrt(Math.Pow(EndPoint.X - StartPoint.X, 2) + Math.Pow(EndPoint.Y - StartPoint.Y, 2))
	End Function

	Public Overrides Function ToElement() As DrawingElement
		Return New DrawingElement With {
			.Id = Me.Id,
			.Type = ShapeType.Line,
			.LayerId = Me.LayerId,
			.GeometryJson = JsonConvert.SerializeObject(New With {
				.startX = StartPoint.X,
				.startY = StartPoint.Y,
				.endX = EndPoint.X,
				.endY = EndPoint.Y
			})
		}
	End Function
End Class
```

### 9.3 RectangleShape Implementation

```vb
Public Class RectangleShape
	Inherits ShapeBase

	Public Property TopLeft As PointF
	Public Property Width As Double
	Public Property Height As Double

	Public Sub New(topLeft As PointF, width As Double, height As Double)
		MyBase.New()
		TopLeft = topLeft
		Width = width
		Height = height
		Type = ShapeType.Rectangle
	End Sub

	Public Overrides Function GetBoundingBox() As RECT
		Return New RECT(
			CInt(TopLeft.X),
			CInt(TopLeft.Y),
			CInt(TopLeft.X + Width),
			CInt(TopLeft.Y + Height))
	End Function

	Public Overrides Sub Draw(g As Graphics, zoom As Single, pan As PointF, layout As CanvasLayout, Optional highlighted As Boolean = False)
		Dim physRect = CoordinateConverter.ToPhysicalRect(GetBoundingBox(), zoom, pan, layout)
		Dim pen = If(highlighted, New Pen(Color.Red, 2), New Pen(Color.Black, 1))
		g.DrawRectangle(pen, physRect)
		pen.Dispose()
	End Sub

	Public Overrides Function Contains(logicalPt As PointF) As Boolean
		Return logicalPt.X >= TopLeft.X AndAlso logicalPt.X <= TopLeft.X + Width AndAlso
			   logicalPt.Y >= TopLeft.Y AndAlso logicalPt.Y <= TopLeft.Y + Height
	End Function

	Public Function GetArea() As Double
		Return Width * Height
	End Function

	Public Overrides Function ToElement() As DrawingElement
		Return New DrawingElement With {
			.Id = Me.Id,
			.Type = ShapeType.Rectangle,
			.LayerId = Me.LayerId,
			.GeometryJson = JsonConvert.SerializeObject(New With {
				.topLeftX = TopLeft.X,
				.topLeftY = TopLeft.Y,
				.width = Width,
				.height = Height
			})
		}
	End Function
End Class
```

---

## 10. Usage Examples & Patterns

### 10.1 Complete Workflow: Draw → Calculate → Export

```vb
' MainForm.vb
Private Sub CompleteWorkflow()
	' 1. Set up layout
	Dim layout = New CanvasLayout With {.Name = "Project 1"}
	_canvas.SetLayout(layout)

	' 2. User draws shapes (via UI)
	' - Canvas raises ShapeCreated events
	' - Layer panel updates object counts
	' - Properties panel shows shape properties

	' 3. User configures materials
	' - MaterialService stores material definitions

	' 4. Assign materials and logical 3D to shapes
	' - Via PropertiesPanel UI

	' 5. Calculate take-off
	Dim takeOffService = New TakeOffService()
	Dim summary = takeOffService.GetSummary(layout, AggregationMode.ByMaterial)

	' 6. Export to Excel
	Dim exporter = New ExcelExporter()
	exporter.ExportToExcel(summary, "C:\takeoff-report.xlsx")
End Sub
```

### 10.2 AI Intake Workflow

```vb
' MainForm (AI Review Tab)
Private Async Sub OnUploadDrawing()
	Dim dialog = New OpenFileDialog With {.Filter = "Image files|*.jpg;*.png|PDF files|*.pdf"}
	If dialog.ShowDialog() = DialogResult.OK Then
		_statusLabel.Text = "Processing..."
		Try
			Dim aiService = New AiIntakeService()
			Dim result = Await aiService.IntakeDrawingAsync(dialog.FileName)

			' Show AI candidates for user review
			DisplayAiCandidatesForReview(result)
		Catch ex As Exception
			MessageBox.Show($"Error: {ex.Message}")
		End Try
	End If
End Sub

Private Sub OnAcceptAiCandidate(candidate As GeometryCandidate)
	' Convert AI candidate to drawing element
	Dim aiService = New AiIntakeService()
	aiService.AcceptCandidate(candidate, _canvas.GetLayout())

	' Add to canvas
	Dim shape = ShapeFactory.CreateShapeFromCandidate(candidate)
	_canvas._shapes.Add(shape)
	_canvas.Invalidate()

	' Update layers if auto-created
	_layerPanel.Refresh()
End Sub
```

### 10.3 Multi-Selection and Bulk Edit

```vb
' PropertiesPanel
Public Sub OnApplyPropertiesToMultiSelection()
	If _selectedShapes.Count <= 1 Then
		Return
	End If

	' Get values from UI
	Dim newLayerId = CGuid(_layerDropdown.SelectedValue)
	Dim newHeight = CDbl(_heightTextBox.Text)

	' Apply to all selected
	For Each shape In _selectedShapes
		shape.LayerId = newLayerId
		If shape.HasLogical3D Then
			shape.LogicalHeight = newHeight
		End If
	Next

	' Refresh canvas
	_canvas.Invalidate()
	RaiseEvent PropertiesChanged()
End Sub
```

---

## 11. Design Patterns Used

### Double Buffering
Prevents flicker by composing off-screen then copying atomically.

### Event-Driven Architecture
Components communicate via events, not direct references.

### Single Responsibility
Each service/control has one reason to change.

### Coordinate Transformation at Boundaries
Convert between coordinate spaces only at UI/logic boundaries.

### Dependency Injection
Services accept dependencies via constructor, enabling testing.

---

## 12. Testing Strategy

### Unit Tests

```vb
Public Class CoordinateConverterTests
	<TestMethod>
	Public Sub ToLogicalAndBack_ShouldBeSymmetric()
		Dim layout = New CanvasLayout()
		Dim logical = New PointF(1000, 2000)
		Dim zoom = 2.0F
		Dim pan = New PointF(100, 100)

		Dim physical = CoordinateConverter.ToPhysical(logical, zoom, pan, layout)
		Dim backToLogical = CoordinateConverter.ToLogical(physical, zoom, pan, layout)

		Assert.AreEqual(logical.X, backToLogical.X, 0.01)
		Assert.AreEqual(logical.Y, backToLogical.Y, 0.01)
	End Sub
End Class
```

### Integration Tests

```vb
Public Class CanvasWorkflowTests
	<TestMethod>
	Public Sub DrawLine_ThenCalculate_ProducesCorrectQuantity()
		Dim layout = New CanvasLayout()
		Dim canvas = New CanvasControl()
		canvas.SetLayout(layout)

		' Simulate drawing a line
		Dim line = New LineShape(New PointF(0, 0), New PointF(1000, 0))
		canvas._shapes.Add(line)
		layout.AddElement(line.ToElement())

		' Calculate
		Dim calc = New TakeOffCalculator()
		Dim qty = calc.Calculate(line.ToElement(), layout)

		Assert.AreEqual(1000, qty.Quantity, 0.1)
		Assert.AreEqual("mm", qty.Unit)
	End Sub
End Class
```

---

**End ? CoNSoL-TakeOff Component Reference (v1.0.0-draft)**

