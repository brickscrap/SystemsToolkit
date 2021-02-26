CREATE TABLE [dbo].[POSHardware]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [StationId] CHAR(5) NOT NULL,
    [Number] INT NULL, 
    [Type] VARCHAR(10) NOT NULL, 
    [OperatingSystem] NVARCHAR(100) NOT NULL,
    [HardwareType] NVARCHAR(100) NOT NULL, 
    [SoftwareVersion] NVARCHAR(100) NOT NULL, 
    [PrimaryIP] NVARCHAR(20) NULL, 
    [ReceiptPrinter] NVARCHAR(50) NULL, 
    [CustomerDisplay] NVARCHAR(50) NULL, 
    [BarcodeScanner] NVARCHAR(50) NULL, 
    [LevelGauge] NVARCHAR(50) NULL, 
    [TouchScreenType] NVARCHAR(50) NULL, 
    [UPS] NVARCHAR(50) NULL, 
    [NumSerialPorts] INT NULL, 
    CONSTRAINT [FK_POSHardware_Stations] FOREIGN KEY ([StationId]) REFERENCES [Stations]([Id])
)
