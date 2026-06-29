' 
Imports Desktop.CanvasControlWPF
Public Class cZoomButtonWPF
    Private WithEvents myPictureBox As CanvasControl 'PictureBoxControl
    Public Property LinkedPictureBox As CanvasControl 'PictureBoxControl
        Get
            Return myPictureBox
        End Get
        Set(value As CanvasControl)
            myPictureBox = value
            RefreshDisplayButtonState()
        End Set
    End Property

#Region "Events"
    Private Sub RefreshDisplayButtonState()
        Try
            If LinkedPictureBox IsNot Nothing Then
                Dim btViewRulers1 As ToolStripMenuItem = btViewRulers
                btViewRulers1.Checked = LinkedPictureBox.ShowRulers
                btViewScrollBars.Checked = LinkedPictureBox.ShowScrollbars
                btViewGrid.Checked = LinkedPictureBox.ShowGrid
                tbPixelSizeMic.Text = CStr(LinkedPictureBox.BackgroundImagePixelSize_Mic)
                btUmDmm.Checked = False
                btUmInch.Checked = False
                btUmMeters.Checked = False
                btUmMicron.Checked = False
                btUmMillimeters.Checked = False
                Select Case LinkedPictureBox.UnitOfMeasure
                    Case MeasureSystem.enUniMis.dmm
                        btUmDmm.Checked = True
                    Case MeasureSystem.enUniMis.inches
                        btUmInch.Checked = True
                    Case MeasureSystem.enUniMis.meters
                        btUmMeters.Checked = True
                    Case MeasureSystem.enUniMis.micron
                        btUmMicron.Checked = True
                    Case MeasureSystem.enUniMis.mm
                        btUmMillimeters.Checked = True
                End Select
                Select Case LinkedPictureBox.ClickAction
                    Case ToolType.MeasureDistance
                        btMeasure.Checked = True
                        btZoom.Checked = False
                    Case ToolType.Zoom
                        btMeasure.Checked = False
                        btZoom.Checked = True
                    Case Else
                        btMeasure.Checked = False
                        btZoom.Checked = True
                End Select
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btZoomFit_Click(sender As Object, e As EventArgs) Handles btZoomFit.Click
        Try
            If LinkedPictureBox IsNot Nothing Then
                LinkedPictureBox.ZoomToFit()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btShowGrid_Click(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btViewGrid.Click
        Try
            If LinkedPictureBox IsNot Nothing Then
                btViewGrid.Checked = Not (btViewGrid.Checked)
                LinkedPictureBox.ShowGrid = btViewGrid.Checked
                LinkedPictureBox.Redraw()
                RefreshDisplayButtonState()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btShowRuler_Click(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btViewRulers.Click
        Try
            If LinkedPictureBox IsNot Nothing Then
                btViewRulers.Checked = Not (btViewRulers.Checked)
                LinkedPictureBox.ShowRulers = btViewRulers.Checked
                LinkedPictureBox.Redraw()
                RefreshDisplayButtonState()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btShowScrollBars_Click(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btViewScrollBars.Click
        Try
            If LinkedPictureBox IsNot Nothing Then
                btViewScrollBars.Checked = Not (btViewScrollBars.Checked)
                LinkedPictureBox.ShowScrollbars = btViewScrollBars.Checked
                LinkedPictureBox.Redraw()
                RefreshDisplayButtonState()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btMeasure_Click(sender As Object, e As EventArgs) Handles btMeasure.Click
        Try
            If LinkedPictureBox IsNot Nothing Then
                LinkedPictureBox.ClickAction = ToolType.MeasureDistance
                RefreshDisplayButtonState()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btZoom_Click(sender As Object, e As EventArgs) Handles btZoom.Click
        Try
            If LinkedPictureBox IsNot Nothing Then
                LinkedPictureBox.ClickAction = ToolType.Zoom
                RefreshDisplayButtonState()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub btLoad_Click(sender As Object, e As EventArgs) Handles btLoad.Click
        Try
            Dim strFilter As String = ""
            Dim OpenImageDialog As New System.Windows.Forms.OpenFileDialog
            OpenImageDialog.Filter = "JPEG File Interchange Format (*.jpg;*.jpeg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tiff Format(*.tiff)|*.tiff|Graphics Interchange Format (*.gif)|*.gif"
            OpenImageDialog.ShowDialog()
            If OpenImageDialog.FileName.Length > 0 Then
                LinkedPictureBox.Image = System.Drawing.Image.FromFile(OpenImageDialog.FileName)
                LinkedPictureBox.ZoomToDefaultRect()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btUmMicron_Click(sender As Object, e As EventArgs) Handles btUmMicron.Click, btUmDmm.Click, btUmInch.Click, btUmMeters.Click, btUmMillimeters.Click
        Try
            If LinkedPictureBox Is Nothing Then Return

            If sender Is btUmDmm Then
                LinkedPictureBox.UnitOfMeasure = MeasureSystem.enUniMis.dmm
            ElseIf sender Is btUmInch Then
                LinkedPictureBox.UnitOfMeasure = MeasureSystem.enUniMis.inches
            ElseIf sender Is btUmMeters Then
                LinkedPictureBox.UnitOfMeasure = MeasureSystem.enUniMis.meters
            ElseIf sender Is btUmMicron Then
                LinkedPictureBox.UnitOfMeasure = MeasureSystem.enUniMis.micron
            ElseIf sender Is btUmMillimeters Then
                LinkedPictureBox.UnitOfMeasure = MeasureSystem.enUniMis.mm
            End If
            LinkedPictureBox.Redraw()
            RefreshDisplayButtonState()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents btMeasure As System.Windows.Forms.ToolStripButton
    Friend WithEvents btZoom As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btView As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents btViewRulers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btViewScrollBars As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btViewGrid As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btLoad As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btUm As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents btUmMicron As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btUmMillimeters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btUmInch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btUmMeters As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btUmDmm As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tbPixelSizeMic As System.Windows.Forms.ToolStripLabel
    Friend WithEvents btZoomFit As System.Windows.Forms.ToolStripButton
    Private Sub tbPixelSizeMic_Click(sender As Object, e As EventArgs) Handles tbPixelSizeMic.Click
        If LinkedPictureBox IsNot Nothing Then
            Dim newPixelWidth As Integer = LinkedPictureBox.BackgroundImagePixelSize_Mic
            Dim newString As String = InputBox("Insert new pixel width value (micron):", "Pixel width", CStr(newPixelWidth))
            If newString.Length > 0 Then
                newPixelWidth = CInt(newString)
                LinkedPictureBox.BackgroundImagePixelSize_Mic = newPixelWidth
                LinkedPictureBox.Redraw(True)
                RefreshDisplayButtonState()
            End If

        End If
    End Sub
End Class
