
Public Enum enBitmapOriginPosition
	TopLeft = 0
	Custom = 4
End Enum

Public Class cBackImageGraphics

#Region "Private Variables"
	Friend Origin As Point
	Private BitmapImage As Bitmap
	Private BitmapOrigin As enBitmapOriginPosition
	Private PixelWidth As Double = 100.0, PixelHeight As Double = 100.0
#End Region

#Region "Constractors"
	''' <summary>
	''' Initializes a new instance of the cBackImageGraphics class. This constructor is private and cannot be called directly. Use the public constructor with parameters to create an instance of the class.</summary>
	Private Sub New()
		Return
	End Sub

	''' <summary>Creates a new instance of the cBackImageGraphics class with the specified bitmap image, origin coordinates, origin position, and pixel dimensions.</summary>
	''' <param name="BitmapImg"></param><param name="OriginX"></param><param name="OriginY"></param>
	''' <param name="OriginPosition"></param><param name="Pixel_Width"></param><param name="Pixel_Height"></param>
	Public Sub New(ByVal BitmapImg As Bitmap, ByVal OriginX As Integer, ByVal OriginY As Integer, ByVal OriginPosition As enBitmapOriginPosition, ByVal Pixel_Width As Double, ByVal Pixel_Height As Double)
		BitmapImage = BitmapImg
		Origin.X = OriginX
		Origin.Y = OriginY
		BitmapOrigin = OriginPosition
		PixelWidth = Pixel_Width
		PixelHeight = Pixel_Height
		If PixelWidth < 10 Then
			PixelHeight = 10
		End If
		If PixelHeight < 10 Then
			PixelHeight = 10
		End If
	End Sub
#End Region

#Region "Public Functions"
	''' <summary>Draws the bitmap image onto the specified Graphics object at the defined origin and scaled by the pixel dimensions. If the bitmap image is not set, the method returns without performing any drawing operations.</summary>
	''' <param name="GR"></param>
	Public Sub Draw(ByVal GR As Graphics)
		Try
			If BitmapImage Is Nothing Then
				Return
			End If
			GR.DrawImage(BitmapImage, New Rectangle(Origin.X, Origin.Y, BitmapImage.Width * PixelWidth, BitmapImage.Height * PixelWidth), 0, 0, BitmapImage.Width, BitmapImage.Height, GraphicsUnit.Pixel)
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub
#End Region

#Region "Initialization and finalization"
	''' <summary>
	''' Disposes of the bitmap image and releases any resources used by the cBackImageGraphics instance. After calling this method, the BitmapImage property is set to Nothing, and any further attempts to use the instance may result in an error.</summary>
	Public Sub Dispose()
		Try
			Me.BitmapImage.Dispose()
			Me.BitmapImage = Nothing
		Catch ex As Exception
			Return
		End Try
	End Sub
#End Region

End Class
