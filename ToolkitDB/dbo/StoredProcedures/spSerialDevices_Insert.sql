CREATE PROCEDURE [dbo].[spSerialDevices_Insert]
	@POSHardwareId int,
	@Device nvarchar(50),
	@PortNumber nvarchar(2)
AS
BEGIN
	insert into dbo.POSSerialDevices (POSHardwareId, Device, PortNumber)
	values (@POSHardwareId, @Device, @PortNumber)
END
