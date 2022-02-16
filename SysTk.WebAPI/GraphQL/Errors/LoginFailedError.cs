namespace SysTk.WebAPI.GraphQL.Errors
{
    public class LoginFailedError : Exception
    {
        public LoginFailedError() : base("Login failed. The username or password provided is not valid.")
        {
        }
    }
}
