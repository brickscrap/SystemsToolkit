namespace SysTk.WebAPI.GraphQL.Types.Users
{
    public class AddUserInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
