CREATE PROCEDURE [dbo].[spStations_GetAll]
AS
BEGIN
	select Id, KimoceId, [Name], Company, [Status], NumberOfPOS
	from dbo.Stations
END
