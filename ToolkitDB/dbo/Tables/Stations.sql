CREATE TABLE [dbo].[Stations]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PsId] CHAR(5) NOT NULL, 
    [KimoceId] VARCHAR(17) NOT NULL, 
    [Name] VARCHAR(100) NOT NULL, 
    [Company] VARCHAR(50) NOT NULL, 
    [Status] VARCHAR(50) NOT NULL 
)
