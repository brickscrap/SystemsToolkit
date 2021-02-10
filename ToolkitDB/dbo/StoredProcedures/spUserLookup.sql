CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
begin
	set nocount on;

	select Id, EmailAddress, CreatedDate, Roles
	from [dbo].[User]
	where Id = @Id
end
