namespace SysTk.WebAPI.GraphQL.Types.Users
{
    public class AddUserToRoleInput
    {
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}
