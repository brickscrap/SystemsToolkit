using System.ComponentModel.DataAnnotations;

namespace SysTk.WebAPI.GraphQL.Types.Users
{
    public class AddUserToRoleInput
    {
        [Required]
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}
