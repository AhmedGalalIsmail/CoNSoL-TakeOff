
Option Strict On
Imports System.Text.Json
Imports System.IO

Namespace Config
    Public Class AppConfig
        Public Property AppName As String = "CoNSoL-TakeOff"
        Public Property LogDir As String = "logs"
        Public Property DataDir As String = "data"

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