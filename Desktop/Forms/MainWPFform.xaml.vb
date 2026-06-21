Option Strict On

Imports System.Collections.ObjectModel
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media

Partial Public Class MainWPFform
	Inherits Window

	Private ReadOnly _layers As New ObservableCollection(Of LayerRow)()
	Private _activeTool As String = "Select"
	Private _gridEnabled As Boolean = True
	Private _snapEnabled As Boolean = True
	Private _currentZoom As Integer = 100

	Public Sub New()
		InitializeComponent()
		SeedLayers()
		LayerGrid.ItemsSource = _layers
		UpdateStatus("Ready")
		UpdateToolStatus("Select")
	End Sub

	Private Sub SeedLayers()
		_layers.Clear()
		_layers.Add(New LayerRow("Walls", 4, True, False, True, "#555555"))
		_layers.Add(New LayerRow("Doors", 1, True, False, True, "#8B6914"))
		_layers.Add(New LayerRow("Slabs", 2, True, True, False, "#4e8cff"))
		_layers.Add(New LayerRow("Columns", 4, True, False, True, "#888888"))
	End Sub

	Private Sub OnOpenCrudClick(sender As Object, e As RoutedEventArgs)
		Dim dlg As New MaterialCrudFormWPF()
		dlg.Owner = Me
		dlg.ShowDialog()
		UpdateStatus("Materials & Blocks opened")
	End Sub

	Private Sub OnToolClick(sender As Object, e As RoutedEventArgs)
		Dim tagText As String = Nothing
		If TypeOf sender Is Button Then
			tagText = TryCast(CType(sender, Button).Tag, String)
		ElseIf TypeOf sender Is MenuItem Then
			tagText = TryCast(CType(sender, MenuItem).Tag, String)
		End If

		If String.IsNullOrWhiteSpace(tagText) Then Return
		_activeTool = tagText
		UpdateToolStatus(tagText)
	End Sub

	Private Sub OnGridClick(sender As Object, e As RoutedEventArgs)
		_gridEnabled = Not _gridEnabled
		UpdateStatus($"Grid {(If(_gridEnabled, "on", "off"))}")
	End Sub

	Private Sub OnZoomInClick(sender As Object, e As RoutedEventArgs)
		_currentZoom = Math.Min(200, _currentZoom + 10)
		StatusZoomText.Text = $"Zoom: {_currentZoom}%"
	End Sub

	Private Sub OnZoomOutClick(sender As Object, e As RoutedEventArgs)
		_currentZoom = Math.Max(50, _currentZoom - 10)
		StatusZoomText.Text = $"Zoom: {_currentZoom}%"
	End Sub

	Private Sub OnNewClick(sender As Object, e As RoutedEventArgs)
		SeedLayers()
		LayerGrid.Items.Refresh()
		UpdateStatus("New workspace created")
	End Sub

	Private Sub OnOpenClick(sender As Object, e As RoutedEventArgs)
		UpdateStatus("Open is not wired yet")
	End Sub

	Private Sub OnSaveClick(sender As Object, e As RoutedEventArgs)
		UpdateStatus("Save is not wired yet")
	End Sub

	Private Sub OnImportClick(sender As Object, e As RoutedEventArgs)
		UpdateStatus("Import placeholder")
	End Sub

	Private Sub OnExportClick(sender As Object, e As RoutedEventArgs)
		UpdateStatus("Export placeholder")
	End Sub

	Private Sub OnMinimizeClick(sender As Object, e As RoutedEventArgs)
		WindowState = WindowState.Minimized
	End Sub

	Private Sub OnMaximizeClick(sender As Object, e As RoutedEventArgs)
		If WindowState = WindowState.Maximized Then
			WindowState = WindowState.Normal
		Else
			WindowState = WindowState.Maximized
		End If
	End Sub

	Private Sub OnCloseClick(sender As Object, e As RoutedEventArgs)
		Close()
	End Sub

	Private Sub UpdateStatus(message As String)
		StatusMessageText.Text = message
		StatusSelectionText.Text = "Selection: none"
		StatusLayerText.Text = $"Layer: {If(_layers.Count > 0, _layers(0).Name, "none")}"
		StatusToolText.Text = $"Tool: {_activeTool}"
		StatusCoordsText.Text = "Cursor: --, --"
		StatusGridText.Text = $"Grid: {(If(_gridEnabled, "On", "Off"))} | Snap: {(If(_snapEnabled, "On", "Off"))}"
		StatusZoomText.Text = $"Zoom: {_currentZoom}%"
	End Sub

	Private Sub UpdateToolStatus(toolName As String)
		StatusToolText.Text = $"Tool: {toolName}"
		StatusMessageText.Text = $"Tool set to {toolName}"
	End Sub

	Private NotInheritable Class LayerRow
		Public Sub New(name As String, objectCount As Integer, visible As Boolean, locked As Boolean, printable As Boolean, colorTag As String)
			Me.Name = name
			Me.ObjectCount = objectCount
			Me.Visible = visible
			Me.Locked = locked
			Me.Printable = printable
			Me.ColorTag = colorTag
		End Sub

		Public Property Name As String
		Public Property ObjectCount As Integer
		Public Property Visible As Boolean
		Public Property Locked As Boolean
		Public Property Printable As Boolean
		Public Property ColorTag As String
	End Class
End Class
