CREATE PROCEDURE [dbo].[spStations_GetByKimoceId]
	@KimoceId varchar(17)
AS
BEGIN
	SELECT Id, KimoceId, [Name], Company, [Status], NumberOfPOS
	from dbo.Stations s
	where s.KimoceId = @KimoceId
END;
