
Option Strict On
Imports System.IO
Imports System.Text

Namespace Logging
    Public Class FileLogger
        Implements ILogger

        Private ReadOnly _logDir As String
        Private ReadOnly _appName As String

        Public Sub New(appName As String, logDir As String)
            _appName = appName
            _logDir = logDir
            Directory.CreateDirectory(_logDir)
        End Sub

        Public Sub Info(message As String) Implements ILogger.Info
            Write("INFO", message)
        End Sub

        Public Sub Warn(message As String) Implements ILogger.Warn
            Write("WARN", message)
        End Sub

        Public Sub [Error](message As String, Optional ex As Exception = Nothing) Implements ILogger.Error
            Dim full = If(ex Is Nothing, message, message & Environment.NewLine & ex.ToString())
            Write("ERROR", full)
        End Sub

        Private Sub Write(level As String, text As String)
            Dim file = Path.Combine(_logDir, $"{_appName}_{DateTime.UtcNow:yyyyMMdd}.log")
            Dim line = $"{DateTime.UtcNow:O} [{level}] {text}"
            SyncLock GetType(FileLogger)
                System.IO.File.AppendAllText(file, line & Environment.NewLine, Encoding.UTF8)
            End SyncLock
        End Sub
    End Class
End Namespace