Imports System.Drawing.Drawing2D

Partial Public Class CanvasControl
	Public Class SelectionBoxElement

#Region "Private variables"
		Public TopLeftCorner As System.Drawing.Point = System.Drawing.Point.Empty
		Public BottomRightCorner As System.Drawing.Point = RECT.InvalidPoint
		Public KeepAspectRatio As Boolean = False
		Public LinkedPictureBox As CanvasControl
		Private Shared myBoxPenAreaSelection As New Pen(Color.FromArgb(200, Color.Black))
		Private Shared myBoxPenSingleClick As New Pen(Color.FromArgb(200, Color.Red))
		Private Shared myBoxBrush As New SolidBrush(Color.FromArgb(40, Color.CadetBlue))
#End Region

#Region "Property"
		Public ReadOnly Property IsInvalid() As Boolean
			Get
				Return BottomRightCorner = RECT.InvalidPoint OrElse TopLeftCorner = RECT.InvalidPoint
			End Get
		End Property
		''' <summary>Returns the size of the area to be selected in the case of single-click selection.
		''' </summary>
		Private ReadOnly Property PointSelectAreaSize() As Integer
			Get
				' I calculate the area to keep around the point, I keep 15 pixels in total
				Return LinkedPictureBox.GraphicInfo.ToLogicalDimension(15.0!)
			End Get
		End Property
		Private ReadOnly Property SingleClickRectangle() As RECT
			Get
				Dim halfAreaSize As Integer = PointSelectAreaSize / 2
				Dim r As New RECT(TopLeftCorner.X - halfAreaSize, TopLeftCorner.Y - halfAreaSize, TopLeftCorner.X + halfAreaSize, TopLeftCorner.Y + halfAreaSize)
				r.NormalizeRect()
				Return r
			End Get
		End Property
		Public ReadOnly Property IsCreatedFromSinglePoint() As Boolean
			Get
				' Check if the rectangle has both valid coordinates
				If IsInvalid Then
					Return False
				End If
				' Check if the coordinates are the same
				If (TopLeftCorner = BottomRightCorner) Then
					Return True
				End If
				'If the Then "single-click rectangle" contains the second point Of the box,
				' then the selection box was created by a single click.
				Return SingleClickRectangle.Contains(BottomRightCorner)
			End Get
		End Property
#End Region

#Region "Operators"
		Public Shared Widening Operator CType(ByVal box As SelectionBoxElement) As RECT
			If box.IsInvalid Then
				Return New RECT()
			End If
			If box.IsCreatedFromSinglePoint Then
				Return box.SingleClickRectangle
			Else
				Return box.RectFromPoints(box.TopLeftCorner, box.BottomRightCorner)
			End If
		End Operator
#End Region

#Region "Constructors"
		Public Sub New(ByVal picBox As CanvasControl)
			LinkedPictureBox = picBox
		End Sub

#End Region

#Region "Private Functions"
		Private Function RectFromPoints(ByVal FirstCorner As System.Drawing.Point, ByVal SecondCorner As System.Drawing.Point) As RECT
			Try
				If FirstCorner = RECT.InvalidPoint OrElse SecondCorner = RECT.InvalidPoint Then
					Return New RECT()
				End If

				If KeepAspectRatio Then
					Dim Sign As Integer
					If (Math.Abs((SecondCorner.X - FirstCorner.X) / LinkedPictureBox.Width)) > Math.Abs(((SecondCorner.Y - FirstCorner.Y) / LinkedPictureBox.Height)) Then
						If SecondCorner.Y > FirstCorner.Y Then Sign = 1 Else Sign = -1
						SecondCorner.Y = FirstCorner.Y + Math.Abs((SecondCorner.X - FirstCorner.X) * (LinkedPictureBox.Height / LinkedPictureBox.Width)) * Sign
					Else
						If SecondCorner.X > FirstCorner.X Then Sign = 1 Else Sign = -1
						SecondCorner.X = FirstCorner.X + Math.Abs((SecondCorner.Y - FirstCorner.Y) * (LinkedPictureBox.Width / LinkedPictureBox.Height)) * Sign
					End If
				End If

				Dim r As New RECT(FirstCorner.X, FirstCorner.Y, SecondCorner.X, SecondCorner.Y)
				r.NormalizeRect()

				Return r
			Catch e As Exception
				MsgBox(e.Message)
				Return New RECT()
			End Try
		End Function

#End Region

#Region "Public Functions"
		Public Sub Reset()
			TopLeftCorner = RECT.InvalidPoint
			BottomRightCorner = RECT.InvalidPoint
		End Sub
		Public Sub Draw(ByVal GR As Graphics, Optional ByVal usePhysicalCoords As Boolean = True)
			' Check se questo box e' valido
			If Me.IsInvalid Then
				Exit Sub
			End If

			' I find the rectangle to invalidate
			Dim r As RECT = Me

			' If necessary, convert to physical coordinates
			If usePhysicalCoords Then
				r = LinkedPictureBox.GraphicInfo.ToPhysicalRect(r)
			End If

			' Check if the resulting rectangle is valid
			If r.IsZeroSized Then
				Exit Sub
			End If

			' Draw the rectangle
			If Me.IsCreatedFromSinglePoint Then
				GR.DrawRectangle(myBoxPenSingleClick, r)
			Else
				GR.FillRectangle(myBoxBrush, r)
				GR.DrawRectangle(myBoxPenAreaSelection, r)
			End If
		End Sub
		Public Sub Invalidate()
			Dim r As RECT = Me
			' NOTE: Invalidate() must be passed physical coordinates, not logical coordinates.
			r = LinkedPictureBox.GraphicInfo.ToPhysicalRect(r)
			r.Inflate(1, 1)
			' Redraw the associated PictureBox
			LinkedPictureBox.Invalidate(r)
		End Sub
#End Region

	End Class
End Class
