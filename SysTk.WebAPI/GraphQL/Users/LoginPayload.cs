using System.IdentityModel.Tokens.Jwt;

namespace SysTk.WebAPI.GraphQL.Users
{
    public class LoginPayload
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
    }
}
