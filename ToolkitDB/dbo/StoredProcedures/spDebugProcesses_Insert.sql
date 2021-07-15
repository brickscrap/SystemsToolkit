CREATE PROCEDURE [dbo].[spDebugProcesses_Insert]
	@Name varchar(50),
	@Description varchar(1000),
	@PosProcessName varchar(50)
AS
BEGIN
	insert into dbo.DebugProcesses ([Name], [Description], PosProcessName)
	values (@Name, @Description, @PosProcessName)
END