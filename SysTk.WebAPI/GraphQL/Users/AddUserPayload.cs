using SysTk.WebApi.Data.Models.Auth;

namespace SysTk.WebAPI.GraphQL.Users
{
    public record AddUserPayload(string FirstName, string LastName, string Email);
}
