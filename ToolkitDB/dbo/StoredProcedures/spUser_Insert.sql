CREATE PROCEDURE [dbo].[spUser_Insert]
	@Id nvarchar(450),
	@EmailAddress nvarchar(256),
	@CreatedDate datetime2(7),
	@Roles nvarchar(256)
AS
begin
	set nocount on;

	insert into dbo.[User](Id, EmailAddress, CreatedDate, Roles)
	values(@Id, @EmailAddress, @CreatedDate, @Roles)
end