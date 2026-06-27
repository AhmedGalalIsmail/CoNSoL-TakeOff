Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Drawing2D


Public Class ConversionInfo

#Region "Public Fields"

#End Region

#Region "Public Fields and Proprties"
	Public PhysicalWidth As Integer = 640
	Public PhysicalHeight As Integer = 480
	Private myScaleFactor As Single = 1.0F
	Public LogicalOrigin As System.Drawing.Point = RECT.InvalidPoint

	''' <summary>
	''' Gets or sets the scale factor used for converting between physical and logical coordinates. The scale factor is a positive single-precision floating-point value.</summary>	
	Public Property ScaleFactor() As Single
		Get
			If Single.IsNaN(myScaleFactor) OrElse Single.IsInfinity(myScaleFactor) Then
				myScaleFactor = 1
			End If
			Return myScaleFactor
		End Get
		Set(ByVal value As Single)
			myScaleFactor = Math.Abs(value)
		End Set
	End Property

	''' <summary>Returns the logical width of the image, in logical coordinates. The logical width is defined as the physical width divided by the scale factor.</summary>
	''' <returns>An integer representing the logical width of the image. If the scale factor is zero, it returns the physical width instead.</returns>
	Public Property LogicalWidth() As Integer
		Get
			Debug.Assert(ScaleFactor <> 0)
			If ScaleFactor = 0 Then
				ScaleFactor = 1
				Return PhysicalWidth
			Else
				Return PhysicalWidth / ScaleFactor
			End If
		End Get
		Set(ByVal Value As Integer)
			If Value <> 0 Then
				ScaleFactor = PhysicalWidth / Value
			End If
		End Set
	End Property

	''' <summary>Returns the logical height of the image, in logical coordinates. The logical height is defined as the physical height divided by the scale factor.</summary>
	''' <returns>An integer representing the logical height of the image. If the scale factor is zero, it returns the physical height instead.</returns>
	Public Property LogicalHeight() As Integer
		Get
			Debug.Assert(ScaleFactor <> 0)
			If ScaleFactor = 0 Then
				ScaleFactor = 1
				Return PhysicalHeight
			Else
				Return PhysicalHeight / ScaleFactor
			End If
		End Get
		Set(ByVal Value As Integer)
			If Value <> 0 Then
				ScaleFactor = PhysicalHeight / Value
			End If
		End Set
	End Property

	''' <summary>Returns the logical area of the image, in logical coordinates. The logical area is defined by the logical origin and the logical width and height.</summary>
	''' <returns>A RECT structure that defines the logical area of the image. If the logical area is not valid, returns a new RECT structure with all fields set to 0.</returns>
	Public Property LogicalArea() As RECT
		Get
			Dim isNotValidArea As Boolean = (LogicalOrigin = RECT.InvalidPoint) OrElse (LogicalWidth = Integer.MaxValue) OrElse (LogicalHeight = Integer.MaxValue)
			isNotValidArea = isNotValidArea OrElse (LogicalWidth = 0) OrElse (LogicalHeight = 0)
			If isNotValidArea Then
				Return New RECT()
			End If
			LogicalArea.left = LogicalOrigin.X
			LogicalArea.top = LogicalOrigin.Y
			LogicalArea.bottom = LogicalOrigin.Y + LogicalHeight
			LogicalArea.right = LogicalOrigin.X + LogicalWidth
			LogicalArea.NormalizeRect()
		End Get
		Set(ByVal value As RECT)
			If (LogicalArea = value) Then
				Exit Property
			End If
			LogicalOrigin = New Point(value.left, value.top)
			LogicalWidth = value.Width
			LogicalHeight = value.Height
		End Set
	End Property
#End Region

#Region "Operators"
	''' <summary>
	''' Compares two ConversionInfo objects for equality</summary>
	Public Shared Operator =(ByVal C1 As ConversionInfo, ByVal C2 As ConversionInfo) As Boolean
		Return (C1.PhysicalWidth = C2.PhysicalWidth) AndAlso (C1.PhysicalHeight = C2.PhysicalHeight) AndAlso (C1.ScaleFactor = C2.ScaleFactor) AndAlso (C1.LogicalOrigin = C2.LogicalOrigin)
	End Operator

	''' <summary>
	''' Compares two ConversionInfo objects for inequality</summary>
	Public Shared Operator <>(ByVal C1 As ConversionInfo, ByVal C2 As ConversionInfo) As Boolean
		Return Not (C1 = C2)
	End Operator
#End Region

#Region "Converting from physical coordinates to logical coordinates"
	''' <summary>
	''' Transforms an X coordinate from physical coordinates to logical coordinates</summary>
	Public Function ToLogicalCoordX(ByVal PhysicalCoordX As Single) As Single
		Try
			Return PhysicalCoordX / ScaleFactor + LogicalOrigin.X
		Catch ex As Exception
			MsgBox(ex.Message)
			Return 0
		End Try
	End Function

	''' <summary>
	''' Transforms a Y coordinate from physical coordinates to logical coordinates</summary>
	Public Function ToLogicalCoordY(ByVal PhysicalCoordY As Single) As Single
		Try
			Return PhysicalCoordY / ScaleFactor + LogicalOrigin.Y
		Catch ex As Exception
			MsgBox(ex.Message)
			Return 0
		End Try
	End Function

	''' <summary>
	''' Transforms a dimension from physical coordinates to logical coordinates<br></br>
	''' NOTE: A dimension is understood as "coordinate1-coordinate2", therefore <br></br>
	''' is invariant with respect to the position of the origin</summary>
	Public Function ToLogicalDimension(ByVal dimension As Single) As Single
		Try
			Return dimension / ScaleFactor
		Catch ex As Exception
			MsgBox(ex.Message)
			Return 0
		End Try
	End Function

	''' <summary>
	''' Transforms a point from physical coordinates to logical coordinates</summary>
	Public Function ToLogicalPoint(ByVal PhysicalPoint As System.Drawing.Point) As System.Drawing.Point
		Try
			Return New System.Drawing.Point(PhysicalPoint.X / ScaleFactor + LogicalOrigin.X, PhysicalPoint.Y / ScaleFactor + LogicalOrigin.Y)
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Function

	''' <summary>
	''' Transforms a point from physical coordinates to logical coordinates</summary>
	Public Function ToLogicalPoint(ByVal X As Integer, ByVal Y As Integer) As Point
		Try
			Return New Point(X / ScaleFactor + LogicalOrigin.X, Y / ScaleFactor + LogicalOrigin.Y)
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Function
#End Region

#Region "Converting from logical to physical coordinates"
	''' <summary>
	''' Transforms an X coordinate from logical coordinates to physical coordinates</summary>
	Public Function ToPhysicalCoordX(ByVal LogicalCoordX As Single) As Single
		Try
			Return (LogicalCoordX - LogicalOrigin.X) * ScaleFactor
		Catch ex As Exception
			MsgBox(ex.Message)
			Return 0
		End Try
	End Function

	''' <summary>
	''' Transforms a Y coordinate from logical coordinates to physical coordinates</summary>
	Public Function ToPhysicalCoordY(ByVal LogicalCoordY As Single) As Single
		Try
			Return (LogicalCoordY - LogicalOrigin.Y) * ScaleFactor
		Catch ex As Exception
			MsgBox(ex.Message)
			Return 0
		End Try
	End Function

	''' <summary>
	''' Transforms a dimension from logical coordinates to physical coordinates <br></br>
	''' NOTE: A dimension is understood as "coordinate 1 - coordinate 2", therefore<br></br>
	''' it is invariant with respect to the position of the origin
	''' </summary>
	Public Function ToPhysicalDimension(ByVal dimension As Single) As Single
		Try
			Return dimension * ScaleFactor
		Catch ex As Exception
			MsgBox(ex.Message)
			Return 0
		End Try
	End Function

	''' <summary>
	''' Transforms a point from logical coordinates to physical coordinates</summary>
	Public Function ToPhysicalPoint(ByVal LogicalPoint As System.Drawing.Point) As System.Drawing.Point
		Try
			Return New System.Drawing.Point((LogicalPoint.X - LogicalOrigin.X) * ScaleFactor, (LogicalPoint.Y - LogicalOrigin.Y) * ScaleFactor)
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Function

	''' <summary>
	''' Transform a rectangle from logical coordinates to physical coordinates</summary>
	Public Function ToPhysicalRect(ByVal LogicalRect As RECT) As RECT
		Try
			Return New RECT(ToPhysicalCoordX(LogicalRect.left), ToPhysicalCoordY(LogicalRect.top), ToPhysicalCoordX(LogicalRect.right), ToPhysicalCoordY(LogicalRect.bottom))
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Function

#End Region

#Region "Conversion from Dot to Micron"
	''' <summary>Converts a value in dots (pixels) to microns, given the bitmap DPI</summary>
	''' <param name="BitmapDPI"></param>
	Public Shared Function DotToMicron(ByVal BitmapDPI As Integer) As Single
		Try
			Return 1 / ((BitmapDPI / 25.4) / 1000)
		Catch ex As Exception
			Return -1.0F
		End Try
	End Function
#End Region

#Region "Public Functions"
	''' <summary>
	''' Creates a copy of this ConversionInfo object</summary>
	Public Function Clone() As Object
		Dim retVal As New ConversionInfo
		retVal.CopyParamsFrom(Me)
		Return retVal
	End Function

	''' <summary>Copies the parameters of the given ConversionInfo object into this object</summary>
	''' <param name="info"></param>
	Public Overridable Sub CopyParamsFrom(ByVal info As ConversionInfo)
		Me.PhysicalWidth = info.PhysicalWidth
		Me.PhysicalHeight = info.PhysicalHeight
		Me.ScaleFactor = info.ScaleFactor
		Me.LogicalOrigin = info.LogicalOrigin
	End Sub
#End Region

End Class

