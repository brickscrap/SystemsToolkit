CREATE PROCEDURE [dbo].[spStations_GetById]
	@Id char(5)
AS
BEGIN
	SELECT Id, KimoceId, [Name], Company, [Status], NumberOfPOS
	from dbo.Stations s
	where s.Id = @Id
END;
