CREATE PROCEDURE [dbo].[spPos_GetIdByNumber]
	@StationId char(5),
	@POSNumber int
AS
BEGIN
	SELECT Id
	from dbo.POSHardware p
	where (p.StationId = @StationId and p.Number = @POSNumber)
END
