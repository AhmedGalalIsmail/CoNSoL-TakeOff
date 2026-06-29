Imports Desktop.CanvasControl

Partial Public Class CanvasControl
	Friend Class CrossCursor

		''' <summary>
		''' Cross cursor default size</summary>
		Public Shared ReadOnly DefaultSize As Size = New Size(20, 20)

#Region "Private variables"
		''' <summary>
		''' Control to draw the cursor on</summary>
		Private myPictureBox As CanvasControl

		''' <summary>
		''' Size of the crosshair</summary>
		Private mySize As Size = DefaultSize

		''' <summary>
		''' Flag indicating whether the cross should be drawn in "full screen" mode</summary>
		Private myFullPictureBoxCross As Boolean = False

		''' <summary>
		''' Color to draw the cross with</summary>
		Private myColor As Color = Drawing.Color.Black

		''' <summary>
		''' Position where the cross is drawn [logical coordinates]</summary>
		Private myCrossPosition As System.Drawing.Point = RECT.InvalidPoint

		''' <summary>
		''' Rectangle containing the four points corresponding to the last cross drawn</summary>
		Private myLastCrossTopPoint As System.Drawing.Point
		Private myLastCrossLeftPoint As System.Drawing.Point
		Private myLastCrossRightPoint As System.Drawing.Point
		Private myLastCrossBottomPoint As System.Drawing.Point

		''' <summary>
		''' Box in which the coordinates are drawn, only the part of the cursor that does not fall on it is drawn</summary>
		Private myCoordinatesBox As CoordinatesBox = Nothing
#End Region
#Region "Properties'"
		''' <summary>
		''' PictureBoxControl associated with this class instance</summary>
		Public Property PictureBoxControl() As CanvasControl
			Get
				Return myPictureBox
			End Get
			Private Set(ByVal value As CanvasControl)
				myPictureBox = value
			End Set
		End Property

		''' <summary>
		''' Crosshair cursor size</summary>
		Public Property Size() As Size
			Get
				Return mySize
			End Get
			Set(ByVal Value As Size)
				mySize = Value
			End Set
		End Property

		''' <summary>
		''' Crosshair color</summary>
		Public Property Color() As Color
			Get
				Return myColor
			End Get
			Set(ByVal Value As Color)
				myColor = Value
			End Set
		End Property
		''' <summary>
		''' Box in which the coordinates are drawn, only the part of the cursor that does not fall on it is drawn</summary>
		Friend Property CoordinatesBox() As CoordinatesBox
			Get
				Return myCoordinatesBox
			End Get
			Set(ByVal value As CoordinatesBox)
				myCoordinatesBox = value
			End Set
		End Property
#End Region
#Region "Constractors"
		''' <summary>
		''' Constructor given a control on which to draw the cursor</summary>
		Public Sub New(ByVal picPictureBox As CanvasControl)
			myPictureBox = picPictureBox
		End Sub
#End Region
#Region "Public Functions"
		''' <summary>
		''' Delete the position where the cross is drawn.</summary>
		Public Sub ResetCrossPosition()
			myCrossPosition = RECT.InvalidPoint
		End Sub

		''' <summary>
		''' Position where the cross is drawn [logical coordinates]</summary>
		Public Property CrossPosition() As System.Drawing.Point
			Get
				Return myCrossPosition
			End Get
			Set(ByVal value As System.Drawing.Point)
				myCrossPosition = value
			End Set
		End Property

		''' <summary>
		''' Draw the cross at the position given by CrossPosition</summary>
		Friend Sub DrawCross(ByVal GR As Graphics)
			DrawCross(GR, CrossPosition)
		End Sub

		''' <summary>
		''' Draw the cross at the specified position</summary>
		Friend Sub DrawCross(ByVal GR As Graphics, ByVal LogicalCoord As System.Drawing.Point)
			Try

				' Check if I have a valid associated control
				If myPictureBox Is Nothing Then
					Exit Sub
				End If

				' Check if the coordinate passed to me is valid
				If LogicalCoord = RECT.InvalidPoint Then
					Exit Sub
				End If

				' Position to draw the cross [physical coordinates of the pictureBox]
				Dim physicalCrossCoords As Point = PictureBoxControl.GraphicInfo.ToPhysicalPoint(LogicalCoord)

				' Minimum and maximum permitted values ??for the arms of the cross
				Dim minCrossValue As Point = Point.Empty
				Dim maxCrossValue As Point = New Point(myPictureBox.Width, myPictureBox.Height)

				' If I have a coordinate box,
				' I make sure not to draw on it
				If myCoordinatesBox IsNot Nothing Then
					' I check that the cross (not full screen) does not fall completely inside the coordinate box.
					' If it is completely inside, there is no need to draw the cross.
					If Not myFullPictureBoxCross AndAlso myCoordinatesBox.DrawingRect.Contains(physicalCrossCoords) Then
						Exit Sub
					End If

					If physicalCrossCoords.X > myCoordinatesBox.DrawingRect.X Then
						maxCrossValue.Y -= myCoordinatesBox.DrawingRect.Height
					End If
					If physicalCrossCoords.Y > myCoordinatesBox.DrawingRect.Y Then
						maxCrossValue.X -= myCoordinatesBox.DrawingRect.Width
					End If
				End If

				' Two values ??I often use
				Dim maxCrossValueX As Integer = maxCrossValue.X '- 2
				Dim maxCrossValueY As Integer = maxCrossValue.Y '- 2

				' I calculate the position of the new cross
				If myFullPictureBoxCross Then
					' Horizontal line
					myLastCrossLeftPoint.X = minCrossValue.X
					myLastCrossRightPoint.X = maxCrossValue.X
					myLastCrossLeftPoint.Y = physicalCrossCoords.Y
					myLastCrossRightPoint.Y = physicalCrossCoords.Y
					' Vertical line
					myLastCrossTopPoint.Y = minCrossValue.Y
					myLastCrossBottomPoint.Y = maxCrossValue.Y
					myLastCrossTopPoint.X = physicalCrossCoords.X
					myLastCrossBottomPoint.X = physicalCrossCoords.X
				Else
					' Horizontal line
					myLastCrossLeftPoint.X = physicalCrossCoords.X - mySize.Width / 2
					myLastCrossRightPoint.X = physicalCrossCoords.X + mySize.Width / 2
					myLastCrossLeftPoint.Y = physicalCrossCoords.Y
					myLastCrossRightPoint.Y = physicalCrossCoords.Y
					' Vertical line
					myLastCrossTopPoint.Y = physicalCrossCoords.Y - mySize.Height / 2
					myLastCrossBottomPoint.Y = physicalCrossCoords.Y + mySize.Height / 2
					myLastCrossTopPoint.X = physicalCrossCoords.X
					myLastCrossBottomPoint.X = physicalCrossCoords.X
				End If

				' Make sure the crosshair doesn't overflow the PictureBox.
				' This should also be done for full-screen crosshairs, because it could overflow.
				' If the PictureBox doesn't take up all the available space in the application,
				' I would draw over the other controls in the application.
				If myLastCrossRightPoint.X > maxCrossValueX Then myLastCrossRightPoint.X = maxCrossValueX
				If myLastCrossRightPoint.Y > maxCrossValueY Then myLastCrossRightPoint.Y = maxCrossValueY
				If myLastCrossRightPoint.Y < minCrossValue.Y Then myLastCrossRightPoint.Y = minCrossValue.Y
				If myLastCrossBottomPoint.Y > maxCrossValueY Then myLastCrossBottomPoint.Y = maxCrossValueY
				If myLastCrossBottomPoint.X > maxCrossValueX Then myLastCrossBottomPoint.X = maxCrossValueX
				If myLastCrossBottomPoint.X < minCrossValue.X Then myLastCrossBottomPoint.X = minCrossValue.X
				If myLastCrossLeftPoint.X < minCrossValue.X Then myLastCrossLeftPoint.X = minCrossValue.X
				If myLastCrossLeftPoint.Y > maxCrossValueY Then myLastCrossLeftPoint.Y = maxCrossValueY
				If myLastCrossLeftPoint.Y < minCrossValue.Y Then myLastCrossLeftPoint.Y = minCrossValue.Y
				If myLastCrossTopPoint.Y < minCrossValue.Y Then myLastCrossTopPoint.Y = minCrossValue.Y
				If myLastCrossTopPoint.X > maxCrossValueX Then myLastCrossTopPoint.X = maxCrossValueX
				If myLastCrossTopPoint.X < minCrossValue.X Then myLastCrossTopPoint.X = minCrossValue.X

				Using crossPen As New Pen(myColor)
					GR.DrawLine(crossPen, myLastCrossLeftPoint, myLastCrossRightPoint)
					GR.DrawLine(crossPen, myLastCrossTopPoint, myLastCrossBottomPoint)
				End Using

			Catch ex As Exception
				MsgBox(ex.Message) 'MsgBox(ex.Message)
			End Try
		End Sub
#End Region
	End Class

End Class
