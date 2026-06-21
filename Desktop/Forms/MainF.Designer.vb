<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainF
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Panel1 = New Panel()
        Panel2 = New Panel()
        Panel4 = New Panel()
        Panel5 = New Panel()
        Panel3 = New Panel()
        Panel6 = New Panel()
        CanvasControl = New CanvasControl()
        SplitContainer1 = New SplitContainer()
        LayerPanel1 = New LayerPanel()
        TreeView1 = New TreeView()
        Panel1.SuspendLayout()
        Panel3.SuspendLayout()
        Panel6.SuspendLayout()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(CanvasControl)
        Panel1.Controls.Add(Panel3)
        Panel1.Controls.Add(Panel5)
        Panel1.Controls.Add(Panel4)
        Panel1.Controls.Add(Panel2)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(0, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(680, 487)
        Panel1.TabIndex = 0
        ' 
        ' Panel2
        ' 
        Panel2.Dock = DockStyle.Top
        Panel2.Location = New Point(0, 0)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(680, 43)
        Panel2.TabIndex = 0
        ' 
        ' Panel4
        ' 
        Panel4.Dock = DockStyle.Bottom
        Panel4.Location = New Point(0, 438)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(680, 49)
        Panel4.TabIndex = 2
        ' 
        ' Panel5
        ' 
        Panel5.Dock = DockStyle.Right
        Panel5.Location = New Point(521, 43)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(159, 395)
        Panel5.TabIndex = 3
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(Panel6)
        Panel3.Dock = DockStyle.Left
        Panel3.Location = New Point(0, 43)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(177, 395)
        Panel3.TabIndex = 4
        ' 
        ' Panel6
        ' 
        Panel6.Controls.Add(SplitContainer1)
        Panel6.Dock = DockStyle.Fill
        Panel6.Location = New Point(0, 0)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(177, 395)
        Panel6.TabIndex = 5
        ' 
        ' CanvasControl
        ' 
        CanvasControl.BackColor = Color.White
        CanvasControl.BusinessJson = Nothing
        CanvasControl.Dock = DockStyle.Fill
        CanvasControl.Location = New Point(177, 43)
        CanvasControl.Name = "CanvasControl"
        CanvasControl.Size = New Size(344, 395)
        CanvasControl.TabIndex = 5
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 0)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(TreeView1)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(LayerPanel1)
        SplitContainer1.Size = New Size(177, 395)
        SplitContainer1.SplitterDistance = 137
        SplitContainer1.TabIndex = 0
        ' 
        ' LayerPanel1
        ' 
        LayerPanel1.Dock = DockStyle.Fill
        LayerPanel1.Location = New Point(0, 0)
        LayerPanel1.Name = "LayerPanel1"
        LayerPanel1.Size = New Size(177, 254)
        LayerPanel1.TabIndex = 0
        ' 
        ' TreeView1
        ' 
        TreeView1.Dock = DockStyle.Fill
        TreeView1.Location = New Point(0, 0)
        TreeView1.Name = "TreeView1"
        TreeView1.Size = New Size(177, 137)
        TreeView1.TabIndex = 0
        ' 
        ' MainF
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(680, 487)
        Controls.Add(Panel1)
        Name = "MainF"
        Text = "MainF"
        Panel1.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        Panel6.ResumeLayout(False)
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents CanvasControl As CanvasControl
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TreeView1 As TreeView
    Friend WithEvents LayerPanel1 As LayerPanel
End Class
