using System.ComponentModel.DataAnnotations;

namespace SysTk.WebAPI.GraphQL.Types.Users
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
