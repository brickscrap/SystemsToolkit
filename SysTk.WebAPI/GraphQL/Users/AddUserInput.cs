namespace SysTk.WebAPI.GraphQL.Users
{
    public record AddUserInput(string Email, string FirstName, string LastName, string Password);
}
