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
        Panel1.SuspendLayout()
        Panel3.SuspendLayout()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Panel3)
        Panel1.Controls.Add(Panel5)
        Panel1.Controls.Add(Panel4)
        Panel1.Controls.Add(Panel2)
        Panel1.Location = New Point(12, 12)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(887, 457)
        Panel1.TabIndex = 0
        ' 
        ' Panel2
        ' 
        Panel2.Dock = DockStyle.Top
        Panel2.Location = New Point(0, 0)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(887, 43)
        Panel2.TabIndex = 0
        ' 
        ' Panel4
        ' 
        Panel4.Dock = DockStyle.Bottom
        Panel4.Location = New Point(0, 408)
        Panel4.Name = "Panel4"
        Panel4.Size = New Size(887, 49)
        Panel4.TabIndex = 2
        ' 
        ' Panel5
        ' 
        Panel5.Dock = DockStyle.Right
        Panel5.Location = New Point(739, 43)
        Panel5.Name = "Panel5"
        Panel5.Size = New Size(148, 365)
        Panel5.TabIndex = 3
        ' 
        ' Panel3
        ' 
        Panel3.Controls.Add(Panel6)
        Panel3.Dock = DockStyle.Left
        Panel3.Location = New Point(0, 43)
        Panel3.Name = "Panel3"
        Panel3.Size = New Size(177, 365)
        Panel3.TabIndex = 4
        ' 
        ' Panel6
        ' 
        Panel6.Dock = DockStyle.Fill
        Panel6.Location = New Point(0, 0)
        Panel6.Name = "Panel6"
        Panel6.Size = New Size(177, 365)
        Panel6.TabIndex = 5
        ' 
        ' MainF
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(921, 481)
        Controls.Add(Panel1)
        Name = "MainF"
        Text = "MainF"
        Panel1.ResumeLayout(False)
        Panel3.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Panel4 As Panel
End Class
