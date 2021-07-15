CREATE TABLE [dbo].[DebugProcesses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(1000) NULL, 
    [PosProcessName] VARCHAR(50) NOT NULL
)
