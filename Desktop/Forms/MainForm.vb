'Filename: Desktop/Forms/MainForm.vb
Option Strict On
Imports System.Security.Cryptography
'Imports System.Drawing
'Imports System.Windows.Forms
Imports Infrastructure.IO
Imports Domain.Entities
Imports Desktop.Controls
Imports Desktop.CompositionRoot

Namespace Forms
    Public Class MainForm
        Inherits Form

        Private ReadOnly _canvas As New CanvasControl With {.Dock = DockStyle.Fill}
        Private ReadOnly _left As New Panel With {.Dock = DockStyle.Left, .Width = 250}
        Private ReadOnly _status As New StatusStrip()
        Private ReadOnly _propertiesPanel As New PropertiesPanel()

        Private CurrentLayout As CanvasLayout

        Public Sub New()
            InitializeComponent()   ' ✅ MUST BE FIRST
            Me.Text = "CoNSoL-TakeOff"
            Me.Width = 1200
            Me.Height = 800
            'Me.Controls.Add(_propertiesPanel)

            ' Tools
            ' Select, Line, Rectangle, Ellipse, Polyline, Pan, Zoom In/Out, Toggle Grid

            ' Select
            Dim btnSelect As New Button With {.Text = "Select", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnSelect.Click, Sub() _canvas.SetTool(ToolType.SelectTool)

            ' Line
            Dim btnLine As New Button With {.Text = "Line", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnLine.Click, Sub() _canvas.SetTool(ToolType.Line)

            ' Rectangle
            Dim btnRect As New Button With {.Text = "Rectangle", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnRect.Click, Sub() _canvas.SetTool(ToolType.Rectangle)

            ' Ellipse
            Dim btnEllipse As New Button With {.Text = "Ellipse", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnEllipse.Click, Sub() _canvas.SetTool(ToolType.Ellipse)

            ' Polyline 
            Dim btnPolyline As New Button With {.Text = "Polyline", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnPolyline.Click, Sub() _canvas.SetTool(ToolType.Polyline)

            ' Pan
            Dim btnPan As New Button With {.Text = "Pan", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnPan.Click, Sub() _canvas.SetTool(ToolType.Pan)

            ' Zoom In/Out
            Dim btnZoomIn As New Button With {.Text = "Zoom +", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnZoomIn.Click, Sub() _canvas.ZoomIn()

            ' Zoom Out
            Dim btnZoomOut As New Button With {.Text = "Zoom -", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnZoomOut.Click, Sub() _canvas.ZoomOut()

            ' Toggle Grid
            Dim btnGrid As New Button With {.Text = "Toggle Grid", .Dock = DockStyle.Top, .Height = 34}
            AddHandler btnGrid.Click, Sub() _canvas.ToggleGrid()

            ' Layout management
            Dim btnNew As New Button With {.Text = "New Layout", .Dock = DockStyle.Top, .Height = 40}
            AddHandler btnNew.Click, Sub() NewLayout()

            ' Open Layout
            Dim btnOpen As New Button With {.Text = "Open Layout", .Dock = DockStyle.Top, .Height = 40}
            AddHandler btnOpen.Click, Sub() OpenLayout()

            ' Save Layout
            Dim btnSave As New Button With {.Text = "Save Layout", .Dock = DockStyle.Top, .Height = 40}
            AddHandler btnSave.Click, Sub() SaveLayout()

            _left.Controls.Add(btnSave)
            _left.Controls.Add(btnOpen)
            _left.Controls.Add(btnNew)
            _left.Controls.Add(btnGrid)
            _left.Controls.Add(btnZoomOut)
            _left.Controls.Add(btnZoomIn)
            _left.Controls.Add(btnPan)
            _left.Controls.Add(btnRect)
            _left.Controls.Add(btnLine)
            _left.Controls.Add(btnSelect)
            _left.Controls.Add(btnEllipse)
            _left.Controls.Add(btnPolyline)

            Me.Controls.Add(_canvas)
            Me.Controls.Add(_left)
            Me.Controls.Add(_status)

            ' Load per-project README files
            LoadReadmeFiles()
        End Sub

        Private Sub LoadReadmeFiles()
            Try
                Dim readmePaths = New String() {
                    "Desktop/README.md",
                    "Infrastructure/README.md",
                    "Application/README.md",
                    "Domain/README.md"
                }

                For Each path In readmePaths
                    Dim fullPath = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)
                    If IO.File.Exists(fullPath) Then
                        Dim content = IO.File.ReadAllText(fullPath)
                        ' You might want to show this content in a dedicated UI component, like a TextBox or a WebBrowser control
                        MessageBox.Show(content, $"README - {IO.Path.GetFileNameWithoutExtension(path)}", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show($"Error loading README files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub NewLayout()
            CurrentLayout = New CanvasLayout()
            Logger.Info("New layout created")
            _canvas.Clear()
        End Sub

        Private Sub OpenLayout()
            Using ofd As New OpenFileDialog With {.Filter = "TakeOff (*.takeoff)|*.takeoff|JSON (*.json)|*.json"}
                If ofd.ShowDialog() = DialogResult.OK Then
                    Dim store = New TakeOffFileStore(CompositionRoot.Crypto)
                    Dim encrypted = ofd.FileName.EndsWith(".takeoff", StringComparison.OrdinalIgnoreCase)
                    CurrentLayout = store.Load(ofd.FileName, encrypted:=encrypted)
                    _canvas.LoadFromLayout(CurrentLayout)
                    Logger.Info($"Loaded layout: {CurrentLayout.CanvasId}")
                End If
            End Using
        End Sub

        Private Sub InitializeComponent()
            SuspendLayout()
            ' 
            ' MainForm
            ' 
            ClientSize = New Size(606, 428)
            Name = "MainForm"
            ResumeLayout(False)

        End Sub

        Private Sub SaveLayout()
            Using sfd As New SaveFileDialog With {.Filter = "TakeOff (*.takeoff)|*.takeoff|JSON (*.json)|*.json"}
                If sfd.ShowDialog() = DialogResult.OK Then
                    Dim store = New TakeOffFileStore(CompositionRoot.Crypto)
                    Dim encrypted = sfd.FileName.EndsWith(".takeoff", StringComparison.OrdinalIgnoreCase)
                    Dim nonce = New Byte(11) {}
                    RandomNumberGenerator.Fill(nonce)
                    Dim layout = _canvas.ToLayout()
                    store.Save(sfd.FileName, layout, encrypt:=encrypted, nonce:=nonce)
                    Logger.Info("Layout saved")
                End If
            End Using
        End Sub
    End Class
End Namespace
