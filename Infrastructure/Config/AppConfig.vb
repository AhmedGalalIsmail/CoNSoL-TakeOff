
Option Strict On
Imports System.Text.Json
Imports System.IO
Imports Infrastructure.Logging

Namespace Config
	''' <summary>
	''' Application configuration class, responsible for loading and saving application settings from/to a JSON file. 
	''' Contains properties for application name, log directory, and data directory.
	''' </summary>
	Public Class AppConfig
#Region "Public Properties"
		''' <summary>
		''' Name of the application, used for logging and display purposes. Default is "CoNSoL-TakeOff".</summary>
		Public Property AppName As String = "CoNSoL-TakeOff"
		''' <summary>
		''' Directory where log files will be stored. Default is "logs".</summary>
		Public Property LogDir As String = "logs"
		''' <summary>
		''' Directory where application data files (e.g., materials, blocks) will be stored. Default is "data".</summary>
		Public Property DataDir As String = "data"
		''' <summary>
		''' Logger instance for logging messages during configuration loading and application runtime. This is not serialized to the config file and should be set at runtime.</summary>
		Public _logger As ILogger
#End Region

		''' <summary>
		''' Loads configuration from specified path. 
		''' If file does not exist, creates default config and saves it.
		''' </summary>
		''' <param name="path"></param>
		''' <param name="logger"></param>
		Public Function Load(path As String, logger As ILogger) As AppConfig
			Return Load(path, logger, _logger)
		End Function

		''' <summary>
		''' Loads configuration from specified path. 
		''' </summary>
		''' <param name="path"></param>
		''' <param name="logger"></param>
		''' <param name="_logger"></param>
		Public Shared Function Load(path As String, logger As ILogger, _logger As ILogger) As AppConfig
			_logger = logger
			If Not File.Exists(path) Then
				Dim def = New AppConfig()
				File.WriteAllText(path, JsonSerializer.Serialize(def, New JsonSerializerOptions With {.WriteIndented = True}))
				Return def
			End If
			Dim json = File.ReadAllText(path)
			Return JsonSerializer.Deserialize(Of AppConfig)(json)
		End Function

		''' <summary>
		''' Loads configuration from specified path.
		''' </summary>
		''' <param name="path"></param>
		Public Shared Function Load(path As String) As AppConfig
			If Not File.Exists(path) Then
				Dim def = New AppConfig()
				File.WriteAllText(path, JsonSerializer.Serialize(def, New JsonSerializerOptions With {.WriteIndented = True}))
				Return def
			End If
			Dim json = File.ReadAllText(path)
			Return JsonSerializer.Deserialize(Of AppConfig)(json)
		End Function
	End Class
End Namespace