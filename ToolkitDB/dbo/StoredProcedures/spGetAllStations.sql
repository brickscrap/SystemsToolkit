CREATE PROCEDURE [dbo].[spGetAllStations]
AS
BEGIN
	select Id, KimoceId, [Name], Company, [Status], NumberOfPOS
	from dbo.Stations
END
