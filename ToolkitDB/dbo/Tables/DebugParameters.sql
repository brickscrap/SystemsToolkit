CREATE TABLE [dbo].[DebugParameters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProcessId] INT NOT NULL, 
    [Decription] VARCHAR(1000) NULL, 
    [Parameter] VARCHAR(5) NOT NULL
)
