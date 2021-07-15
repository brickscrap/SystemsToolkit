CREATE PROCEDURE [dbo].[spSerialDevices_GetByPosId]
	@Id int
AS
BEGIN
Set Nocount On
	select PortNumber, Device
	from dbo.POSSerialDevices
	where POSHardwareId = @Id;
End
