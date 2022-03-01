namespace SysTk.WebAPI.GraphQL.Errors
{
    public class FtpCredentialsExistsError : Exception
    {
        public string Message { get; set; }

        public FtpCredentialsExistsError(string username, string stationId)
        {
            Message = $"Credentials with username {username} already exist for station {stationId}.";
        }
    }
}
