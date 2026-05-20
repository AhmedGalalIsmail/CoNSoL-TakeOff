'Filename: Desktop/CompositionRoot.vb
Option Strict On
Imports Infrastructure.Logging
Imports Infrastructure.Config
Imports Infrastructure.Crypto


Public Class CompositionRoot
    Public Shared Logger As ILogger
    Public Shared Config As AppConfig
    Public Shared Crypto As CryptoService

    Public Shared Sub Bootstrap()
        Config = AppConfig.Load("appsettings.json")
        Logger = New FileLogger("CoNSoL", Config.LogDir)
        Crypto = New CryptoService()
        Logger.Info("Bootstrap complete")
    End Sub
End Class