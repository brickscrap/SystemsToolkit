namespace SysTk.WebAPI.GraphQL.Errors
{
    public class ValidationFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return error;
        }
    }
}
