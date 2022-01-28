namespace SysTk.WebAPI.GraphQL.Errors
{
    public static class ErrorFactory
    {
        public static IError CreateLoginError()
        {
            return ErrorBuilder.New()
                .SetMessage($"Login failed. The username or password provided is not valid.")
                .SetCode("LOGIN_FAILURE")
                .Build();
        }
    }
}
