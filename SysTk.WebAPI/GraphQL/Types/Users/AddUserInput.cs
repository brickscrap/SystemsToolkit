using System.ComponentModel.DataAnnotations;

namespace SysTk.WebAPI.GraphQL.Types.Users
{
    public class AddUserInput
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
