CREATE TABLE [dbo].[Stations]
(
	[Id] CHAR(5) NOT NULL PRIMARY KEY,
    [KimoceId] VARCHAR(17) NOT NULL, 
    [Name] VARCHAR(100) NOT NULL, 
    [Company] VARCHAR(50) NOT NULL, 
    [Status] VARCHAR(50) NOT NULL 
)
