CREATE TABLE [dbo].[POSPumps]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [POSHardwareId] INT NOT NULL, 
    [Number] INT NOT NULL, 
    [Protocol] NVARCHAR(20) NOT NULL
)
