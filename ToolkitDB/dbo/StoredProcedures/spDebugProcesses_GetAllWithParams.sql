CREATE PROCEDURE [dbo].[spDebugProcesses_GetAllWithParams]
AS
BEGIN
	SELECT d.Id, d.[Name], d.[Description], d.PosProcessName, p.Id as ParameterId, p.Parameter, p.[Description]
	from dbo.DebugProcesses d
	left join dbo.DebugParameters p
	on p.ProcessId = d.Id
END;
