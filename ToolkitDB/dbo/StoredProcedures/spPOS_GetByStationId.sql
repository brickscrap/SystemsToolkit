CREATE PROCEDURE [dbo].[spPos_GetByStationId]
	@StationId char(5)
AS
BEGIN
	SET NOCOUNT ON

	select Id, Number, [Type], OperatingSystem, HardwareType, SoftwareVersion, PrimaryIP, ReceiptPrinter, 
		CustomerDisplay, BarcodeScanner, LevelGauge,  TouchScreenType,  UPS,  NumSerialPorts
	from dbo.POSHardware
	where StationId = @StationId;
END
