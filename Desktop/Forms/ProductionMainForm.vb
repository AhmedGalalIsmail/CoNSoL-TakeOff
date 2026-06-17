Option Strict On

Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports Application
Imports Application.AI
Imports Desktop
Imports Desktop.Controls
Imports Domain.Entities
Imports Domain.Services
Imports Infrastructure.IO

Namespace Forms
	''' <summary>
	''' Clean runtime-built main form for the production-style desktop shell.
	''' </summary>
	Public Class ProductionMainForm
		Inherits Form

		Private ReadOnly _canvas As New CanvasControl()
		Private ReadOnly _propertiesPanel As New PropertiesPanel()
		Private ReadOnly _layerPanel As New LayerPanel()
		Private ReadOnly _layerManager As New LayerManager()
		Private ReadOnly _fileStore As New TakeOffFileStore()
		Private ReadOnly _aiIntake As New AiIntakeService()

		Private ReadOnly _toolbar As New ToolStrip()
		Private ReadOnly _status As New StatusStrip()
		Private ReadOnly _statusMessage As New ToolStripStatusLabel()
		Private ReadOnly _statusSelection As New ToolStripStatusLabel()
		Private ReadOnly _statusFile As New ToolStripStatusLabel()
		Private ReadOnly _statusMode As New ToolStripStatusLabel()

		Private ReadOnly _root As New TableLayoutPanel()
		Private ReadOnly _contentSplit As New SplitContainer()
		Private ReadOnly _rightSplit As New SplitContainer()
		Private ReadOnly _layerHost As New Panel()
		Private ReadOnly _propertiesHost As New Panel()

		Private _currentFile As String = ""
		Private _loaded As Boolean

		Public Sub New()
			AutoScaleMode = AutoScaleMode.Dpi
			StartPosition = FormStartPosition.CenterScreen
			Text = "CoNSoL-TakeOff"
			MinimumSize = New Size(1280, 800)
			BackColor = Color.FromArgb(246, 248, 251)

			SuspendLayout()
			BuildFrame()
			ResumeLayout(False)

			AddHandler Load, AddressOf OnLoadBuild
		End Sub

		Private Sub OnLoadBuild(sender As Object, e As EventArgs)
			If _loaded Then Return
			_loaded = True

			InitializeRuntimeUi()
			WireEvents()
			ResetWorkspace()
		End Sub

		Private Sub BuildFrame()
			_root.Dock = DockStyle.Fill
			_root.ColumnCount = 1
			_root.RowCount = 3
			_root.Padding = New Padding(10)
			_root.BackColor = BackColor
			_root.RowStyles.Add(New RowStyle(SizeType.Absolute, 44))
			_root.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
			_root.RowStyles.Add(New RowStyle(SizeType.Absolute, 24))

			_toolbar.Dock = DockStyle.Fill
			_toolbar.GripStyle = ToolStripGripStyle.Hidden
			_toolbar.RenderMode = ToolStripRenderMode.System
			_toolbar.BackColor = Color.White
			_toolbar.Padding = New Padding(4, 2, 4, 2)
			_toolbar.Stretch = True

			_contentSplit.Dock = DockStyle.Fill
			_contentSplit.Orientation = Orientation.Vertical
			_contentSplit.FixedPanel = FixedPanel.Panel2
			_contentSplit.SplitterDistance = 880
			_contentSplit.Panel1MinSize = 640
			_contentSplit.Panel2MinSize = 320

			_rightSplit.Dock = DockStyle.Fill
			_rightSplit.Orientation = Orientation.Horizontal
			_rightSplit.FixedPanel = FixedPanel.Panel2
			_rightSplit.SplitterDistance = 380
			_rightSplit.Panel1MinSize = 240
			_rightSplit.Panel2MinSize = 220

			_layerHost.Dock = DockStyle.Fill
			_layerHost.Padding = New Padding(0, 0, 0, 8)
			_layerHost.BackColor = Color.White
			_layerHost.BorderStyle = BorderStyle.FixedSingle

			_propertiesHost.Dock = DockStyle.Fill
			_propertiesHost.BackColor = Color.White
			_propertiesHost.BorderStyle = BorderStyle.FixedSingle

			_canvas.Dock = DockStyle.Fill
			_canvas.BackColor = Color.White

			_layerPanel.Dock = DockStyle.Fill
			_propertiesPanel.Dock = DockStyle.Fill

			_status.Dock = DockStyle.Fill
			_status.SizingGrip = False
			_statusMessage.Text = "Ready"
			_statusSelection.Text = "Selection: none"
			_statusFile.Text = "File: untitled"
			_statusMode.Text = "Mode: select"
			_status.Items.AddRange(New ToolStripItem() {
				_statusMessage,
				New ToolStripStatusLabel With {.Spring = True},
				_statusSelection,
				New ToolStripStatusLabel With {.BorderSides = ToolStripStatusLabelBorderSides.Left},
				_statusFile,
				New ToolStripStatusLabel With {.BorderSides = ToolStripStatusLabelBorderSides.Left},
				_statusMode
			})

			_layerHost.Controls.Add(_layerPanel)
			_propertiesHost.Controls.Add(_propertiesPanel)

			_rightSplit.Panel1.Controls.Add(_layerHost)
			_rightSplit.Panel2.Controls.Add(_propertiesHost)
			_contentSplit.Panel1.Controls.Add(_canvas)
			_contentSplit.Panel2.Controls.Add(_rightSplit)

			_root.Controls.Add(_toolbar, 0, 0)
			_root.Controls.Add(_contentSplit, 0, 1)
			_root.Controls.Add(_status, 0, 2)
			Controls.Add(_root)
		End Sub

		Private Sub InitializeRuntimeUi()
			LoggerInfo("Building production shell")

			_layerManager.Initialize()
			_layerManager.EnsureDefaultLayer()
			_layerPanel.Initialize(_layerManager)

			_toolbar.Items.Add(CreateButton("New", AddressOf NewLayout))
			_toolbar.Items.Add(CreateButton("Open", AddressOf OpenLayout))
			_toolbar.Items.Add(CreateButton("Save", AddressOf SaveLayout))
			_toolbar.Items.Add(CreateButton("Import", AddressOf ImportDrawing))
			_toolbar.Items.Add(CreateButton("Export", AddressOf ExportLayout))
			_toolbar.Items.Add(New ToolStripSeparator())
			_toolbar.Items.Add(CreateButton("Select", Sub() SetTool(ToolType.SelectTool)))
			_toolbar.Items.Add(CreateButton("Line", Sub() SetTool(ToolType.Line)))
			_toolbar.Items.Add(CreateButton("Rectangle", Sub() SetTool(ToolType.Rectangle)))
			_toolbar.Items.Add(CreateButton("Ellipse", Sub() SetTool(ToolType.Ellipse)))
			_toolbar.Items.Add(CreateButton("Polyline", Sub() SetTool(ToolType.Polyline)))
			_toolbar.Items.Add(CreateButton("Pan", Sub() SetTool(ToolType.Pan)))
			_toolbar.Items.Add(New ToolStripSeparator())
			_toolbar.Items.Add(CreateButton("Grid", Sub() ToggleGrid()))
			_toolbar.Items.Add(CreateButton("Zoom +", Sub() ZoomIn()))
			_toolbar.Items.Add(CreateButton("Zoom -", Sub() ZoomOut()))

			_canvas.SetTool(ToolType.SelectTool)
		End Sub

		Private Sub WireEvents()
			AddHandler _canvas.ElementSelected, AddressOf OnElementSelected
		End Sub

		Private Function CreateButton(text As String, onClick As EventHandler) As ToolStripButton
			Dim button As New ToolStripButton(text)
			button.DisplayStyle = ToolStripItemDisplayStyle.Text
			button.AutoToolTip = True
			button.Margin = New Padding(2, 0, 2, 0)
			AddHandler button.Click, onClick
			Return button
		End Function

		Private Sub SetTool(tool As ToolType)
			_canvas.SetTool(tool)
			_statusMode.Text = $"Mode: {tool}"
			_statusMessage.Text = $"Tool set to {tool}"
		End Sub

		Private Sub ToggleGrid()
			_canvas.ToggleGrid()
			_statusMessage.Text = "Grid toggled"
		End Sub

		Private Sub ZoomIn()
			_canvas.ZoomIn()
			_statusMessage.Text = "Zoomed in"
		End Sub

		Private Sub ZoomOut()
			_canvas.ZoomOut()
			_statusMessage.Text = "Zoomed out"
		End Sub

		Private Sub NewLayout(sender As Object, e As EventArgs)
			ResetWorkspace()
			_currentFile = ""
			_statusMessage.Text = "New workspace created"
		End Sub

		Private Sub OpenLayout(sender As Object, e As EventArgs)
			Using dlg As New OpenFileDialog()
				dlg.Filter = "Take-Off files (*.takeoff)|*.takeoff|All files (*.*)|*.*"
				dlg.Title = "Open project"
				If dlg.ShowDialog(Me) <> DialogResult.OK Then Return

				Dim layout = _fileStore.Load(dlg.FileName)
				_canvas.LoadFromLayout(layout)
				_currentFile = dlg.FileName
				_statusFile.Text = $"File: {Path.GetFileName(dlg.FileName)}"
				_statusMessage.Text = "Project opened"
			End Using
		End Sub

		Private Sub SaveLayout(sender As Object, e As EventArgs)
			Dim layout = _canvas.ToLayout()

			If String.IsNullOrWhiteSpace(_currentFile) Then
				Using dlg As New SaveFileDialog()
					dlg.Filter = "Take-Off files (*.takeoff)|*.takeoff|All files (*.*)|*.*"
					dlg.Title = "Save project"
					dlg.FileName = "project.takeoff"
					If dlg.ShowDialog(Me) <> DialogResult.OK Then Return
					_currentFile = dlg.FileName
				End Using
			End If

			_fileStore.Save(_currentFile, layout)
			_statusFile.Text = $"File: {Path.GetFileName(_currentFile)}"
			_statusMessage.Text = "Project saved"
		End Sub

		Private Sub ExportLayout(sender As Object, e As EventArgs)
			Dim layout = _canvas.ToLayout()
			Dim result As New TakeOffResult()

			For Each element In layout.Elements
				If Not String.IsNullOrWhiteSpace(element.Type) Then
					result.Add(element.Type, 1D)
				End If
			Next

			Using dlg As New SaveFileDialog()
				dlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
				dlg.Title = "Export take-off"
				dlg.FileName = "takeoff.csv"
				If dlg.ShowDialog(Me) <> DialogResult.OK Then Return

				ExcelExporter.Export(result, dlg.FileName)
				_statusMessage.Text = "Take-off exported"
			End Using
		End Sub

		Private Sub ImportDrawing(sender As Object, e As EventArgs)
			Using dlg As New OpenFileDialog()
				dlg.Filter = "Images|*.png;*.jpg;*.jpeg|All files (*.*)|*.*"
				dlg.Title = "Import drawing for AI intake"
				If dlg.ShowDialog(Me) <> DialogResult.OK Then Return

				Using sourceImage = Image.FromFile(dlg.FileName)
					_canvas.SetBackgroundImage(New Bitmap(sourceImage), 0.35F)
				End Using

				Dim result = _aiIntake.ProcessDrawing(dlg.FileName)
				Dim layout As New CanvasLayout()

				For Each element In result.DetectedElements
					layout.Elements.Add(element)
				Next

				If layout.Elements.Count > 0 Then
					_canvas.LoadFromLayout(layout)
					_statusSelection.Text = $"Selection: imported {layout.Elements.Count}"
					_statusMessage.Text = $"Imported {layout.Elements.Count} AI candidates"
				Else
					_statusMessage.Text = "Import completed, no candidates detected"
				End If
			End Using
		End Sub

		Private Sub ResetWorkspace()
			_canvas.Clear()
			_canvas.SetTool(ToolType.SelectTool)
			_propertiesPanel.SetElement(Nothing)
			_statusSelection.Text = "Selection: none"
			_statusFile.Text = "File: untitled"
			_statusMode.Text = "Mode: select"
		End Sub

		Private Sub OnElementSelected(el As CanvasElement)
			_propertiesPanel.SetElement(el)
			If el Is Nothing Then
				_statusSelection.Text = "Selection: none"
			Else
				_statusSelection.Text = $"Selection: {el.Type}"
			End If
		End Sub

		Private Sub LoggerInfo(message As String)
			If CompositionRoot.Logger IsNot Nothing Then
				CompositionRoot.Logger.Info(message)
			End If
		End Sub
	End Class
End Namespace
