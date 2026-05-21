Imports System.Windows.Forms
Imports Domain.Entities
Imports Domain.Services

Public Class LayerPanel
	Inherits UserControl

	' ? CONTROL NAMES (as you requested)
	Private lstLayers As New ListBox()
	Private btnAddLayer As New Button()
	Private btnDeleteLayer As New Button()

	Private _layerManager As LayerManager

	''' <summary>
	''' Initializes LayerPanel with LayerManager dependency.
	''' </summary>
	Public Sub Initialize(manager As LayerManager)
		_layerManager = manager
		RefreshLayers()
	End Sub

	Public Sub New()
		Me.Width = 200
		Me.Dock = DockStyle.Left
		SetupControls()
	End Sub

	Private Sub SetupControls()
		lstLayers.Dock = DockStyle.Top
		lstLayers.Height = 200

		btnAddLayer.Text = "Add Layer"
		btnDeleteLayer.Text = "Delete Layer"

		btnAddLayer.Dock = DockStyle.Top
		btnDeleteLayer.Dock = DockStyle.Top

		AddHandler btnAddLayer.Click, AddressOf AddLayer_Click
		AddHandler btnDeleteLayer.Click, AddressOf DeleteLayer_Click

		Me.Controls.Add(btnDeleteLayer)
		Me.Controls.Add(btnAddLayer)
		Me.Controls.Add(lstLayers)

	End Sub

	Private Sub RefreshLayers()
		lstLayers.Items.Clear()
		For Each layer In _layerManager.GetAll()
			lstLayers.Items.Add(layer.Name)
		Next
	End Sub

	Private Sub AddLayer_Click(sender As Object, e As EventArgs)
		Dim name = "Layer_" & (_layerManager.GetAll().Count + 1)
		_layerManager.AddLayer(name)
		RefreshLayers()
	End Sub

	Private Sub DeleteLayer_Click(sender As Object, e As EventArgs)

		If lstLayers.SelectedIndex < 0 Then Exit Sub

		Dim selectedLayer = _layerManager.GetAll()(lstLayers.SelectedIndex)

		_layerManager.RemoveLayer(selectedLayer.Id)
		RefreshLayers()

	End Sub

End Class
