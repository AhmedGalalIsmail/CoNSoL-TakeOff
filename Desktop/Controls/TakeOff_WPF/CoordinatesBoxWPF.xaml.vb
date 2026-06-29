
Partial Public Class CanvasControlWPF

	Friend Class CoordinatesBoxWPF
#Region "Public Fields and Proprties"
		Private myPictureBoxControlWPF As CanvasControlWPF
		Private myDrawingRect As Rectangle = Rectangle.Empty
		Private myLastCoordToDraw As Point = New Point(Integer.MaxValue, Integer.MaxValue)


		Public Property PictureBoxControlWPF() As CanvasControlWPF
			Get
				Return myPictureBoxControlWPF
			End Get
			Private Set(ByVal value As CanvasControlWPF)
				myPictureBoxControlWPF = value
			End Set
		End Property


		Public ReadOnly Property UnitOfMeasure() As MeasureSystem.enUniMis
			Get
				Return PictureBoxControlWPF.UnitOfMeasure
			End Get
		End Property


		Public ReadOnly Property DrawingRect() As Rectangle
			Get
				Return myDrawingRect
			End Get
		End Property


		Private ReadOnly Property UnitOfMeasureFactor() As Single
			Get
				Return MeasureSystem.CustomUnitToMicron(1, UnitOfMeasure)
			End Get
		End Property


		Private ReadOnly Property UnitOfMeasureString() As String
			Get
				Return MeasureSystem.UniMisDescription(UnitOfMeasure)
			End Get
		End Property
#End Region
#Region "Constractors"
		Public Sub New(pictureBox As CanvasControlWPF)
			myPictureBoxControlWPF = pictureBox
		End Sub
#End Region
#Region "Public Functions"
		Public Sub DrawCoordinateInfo(GR As Graphics, CoordToDraw As Point, Optional PixelCoordMode As Boolean = False)
			Try
				If myPictureBoxControlWPF Is Nothing Then
					Exit Sub
				End If
				If GR Is Nothing Then
					Exit Sub
				End If
				If CoordToDraw.X = Integer.MaxValue OrElse CoordToDraw.Y = Integer.MaxValue Then
					Exit Sub
				End If
				myLastCoordToDraw = CoordToDraw

				Static textFont As Font = New Drawing.Font("Arial narrow", 8)

				Static borderSize As Integer = Math.Ceiling(GR.MeasureString("_", textFont).Width / 2)
				Dim _umsf As Single = UnitOfMeasureFactor
				If PixelCoordMode Then
					_umsf = 1
				End If
				Dim xValue As Single = CoordToDraw.X / _umsf
				Dim yValue As Single = CoordToDraw.Y / _umsf
				Dim textToDraw As String
				If PixelCoordMode Then
					textToDraw = "X=" + xValue.ToString("0000.00") + ", Y=" + yValue.ToString("0000.00")
				Else
					If UnitOfMeasure <> MeasureSystem.enUniMis.micron Then
						textToDraw = "X=" + xValue.ToString("0000.00") + ", Y=" + yValue.ToString("0000.00") + UnitOfMeasureString
					Else
						' If the unit of measurement is macron, then no digits after the decimal point
						textToDraw = "X=" + xValue.ToString("0000") + ", Y=" + yValue.ToString("0000") + UnitOfMeasureString
					End If
				End If

				Dim textBox As SizeF = GR.MeasureString(textToDraw, textFont)

				' If the box changes size, disable the picturebox, so it always looks clean.
				' This is useful when:
				'	- you go from "X=100,Y=100" to "X=99,Y=99", which would leave a part of the previous box "uncovered."
				'	- you change the measurement unit at runtime, so the box is resized.
				Static oldTextBox As SizeF = textBox
				If oldTextBox <> textBox Then
					' Updating the previous dimensions of the box
					oldTextBox = textBox
				End If

				' Update the coordinates of the background rectangle
				' NOTE: Use ClientRectangle.Width instead of Width because this way I can account for any scrollbars.
				myDrawingRect.X = myPictureBoxControl.ClientRectangle.Width - textBox.Width - borderSize
				myDrawingRect.Y = myPictureBoxControl.ClientRectangle.Height - textBox.Height - borderSize
				myDrawingRect.Width = textBox.Width + borderSize
				myDrawingRect.Height = textBox.Height + borderSize

				' If the scrollbars are visible, it needs to make sure the bottom/right edge of the rectangle is also visible
				If myPictureBoxControlWPF.HScroll Then
					myDrawingRect.Height -= 1
				End If
				If myPictureBoxControlWPF.VScroll Then
					myDrawingRect.Width -= 1
				End If

				' Draw the background rectangle
				GR.FillRectangle(Brushes.White, myDrawingRect)
				GR.DrawRectangle(Pens.Black, myDrawingRect)

				' Draw the text string, adding borderSize/2 is used to center the text in the background rectangle
				GR.DrawString(textToDraw, textFont, Brushes.Black, CSng(myDrawingRect.X + (borderSize / 2)), CSng(myDrawingRect.Y + borderSize / 2))
			Catch ex As Exception
				MsgBox(ex.Message)
			End Try
		End Sub
#End Region
	End Class
End Class