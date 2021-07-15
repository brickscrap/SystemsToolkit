CREATE PROCEDURE [dbo].[spStations_Insert]
	@Id char(5),
	@KimoceId varchar(17),
	@Name varchar(100),
	@Company varchar(50),
	@Status varchar(50),
	@NumberOfPOS int
AS
BEGIN
	insert into dbo.Stations (Id, KimoceId, [Name], Company, [Status], NumberOfPOS)
	values (@Id, @KimoceId, @Name, @Company, @Status, @NumberOfPOS)
END;
