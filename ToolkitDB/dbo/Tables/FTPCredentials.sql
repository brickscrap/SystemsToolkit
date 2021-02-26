CREATE TABLE [dbo].[FTPCredentials]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StationId] CHAR(5) NOT NULL, 
    [CisIp] VARCHAR(15) NOT NULL, 
    [User] VARCHAR(50) NOT NULL, 
    [Password] VARCHAR(50) NOT NULL, 
    [Port] INT NOT NULL DEFAULT 21, 
    [ActiveMode] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_FTPCredentials_Stations] FOREIGN KEY ([StationId]) REFERENCES [Stations]([Id])
)
