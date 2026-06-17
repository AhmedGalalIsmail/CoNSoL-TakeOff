Imports Domain.Entities

''' <summary>Represents the result of an AI intake process, containing detected text and scale information.</summary>
''' <returns>An AiIntakeResult object that encapsulates the detected text annotations, scale information, and any identified geometric elements from the architectural drawing. This result serves as the output of the AI intake process and can be used for further processing in the takeoff calculations and other downstream tasks in the construction workflow.</returns>
Public Class AiIntakeResult

#Region "Public Properties"
	''' <summary>A list of detected text strings from the drawing. In a real implementation, this would be the output of an OCR engine that has processed the drawing image to extract textual information such as annotations, dimensions, and scale indicators. This text can be analyzed further to identify specific details about the drawing, such as room sizes, wall thicknesses, and other relevant information for construction takeoff calculations.</summary>
	''' <returns>A list of detected text strings from the image. In a real implementation, this would be the output of an OCR engine.</returns>
	Public Property DetectedText As List(Of String)
	''' <summary>The detected scale information extracted from the text annotations in the drawing. This typically includes the scale factor (e.g., "1:100") that indicates the ratio of the drawing's dimensions to real-world dimensions. The scale information is crucial for accurate measurements and takeoff calculations, as it allows for the conversion of dimensions from the drawing to actual sizes in the construction process.</summary>
	Public Property DetectedScale As String
	''' <summary>
	''' A list of detected geometric elements from the drawing, represented as CanvasElement objects. These elements are identified based on the analysis of the extracted text and may include shapes such as rectangles, lines, and other geometric forms that are relevant to the construction takeoff process. Each CanvasElement contains geometry information that can be used for further processing in the takeoff calculations, such as determining areas, lengths, and other dimensions necessary for estimating quantities and costs in construction projects.
	''' </summary>
	Public Property DetectedElements As List(Of CanvasElement) ' ? NEW
#End Region

End Class