CREATE PROCEDURE [dbo].[spPos_Insert]
	@StationId char(5),
	@Number int,
	@Type varchar(10),
	@OperatingSystem nvarchar(100),
	@HardwareType nvarchar(100),
	@SoftwareVersion nvarchar(100),
	@PrimaryIP nvarchar(20),
	@ReceiptPrinter nvarchar(50),
	@CustomerDisplay nvarchar(50),
	@BarcodeScanner nvarchar(50),
	@LevelGauge nvarchar(50),
	@TouchScreenType nvarchar(50),
	@UPS nvarchar(50),
	@NumSerialPorts nvarchar(50)
AS
BEGIN
	insert into dbo.POSHardware (StationId, Number, [Type], OperatingSystem, HardwareType,
		SoftwareVersion, PrimaryIP, ReceiptPrinter,CustomerDisplay, BarcodeScanner, LevelGauge,
		TouchScreenType, UPS, NumSerialPorts)
	values(@StationId, @Number, @Type, @OperatingSystem, @HardwareType, @SoftwareVersion, @PrimaryIP,
		@ReceiptPrinter,@CustomerDisplay, @BarcodeScanner, @LevelGauge, @TouchScreenType, @UPS, @NumSerialPorts)
END