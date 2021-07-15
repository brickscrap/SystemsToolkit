CREATE PROCEDURE [dbo].[spDebugProcesses_GetByName]
	@Name varchar(50)
AS
Begin
	SELECT *
	from dbo.DebugProcesses
	where [Name] = @Name
END