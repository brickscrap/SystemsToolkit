CREATE PROCEDURE [dbo].[spDebugProcesses_GetAll]
AS
BEGIN
	SELECT [Id], [Name], [Description], PosProcessName 
	from dbo.DebugProcesses
END
