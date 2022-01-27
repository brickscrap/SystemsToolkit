namespace SysTk.WebAPI.GraphQL.Errors
{
    public class ValidationFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Code == "FairyBread_ValidationError")
            {
                error = error.RemoveExtensions().RemoveCode();
            }

            return error;
        }
    }
}
