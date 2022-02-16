namespace SysTk.WebAPI.GraphQL.Errors
{
    public class FtpCredentialsNotExistError : Exception
    {
        public FtpCredentialsNotExistError(string username, string siteId) : base($"FTP credentials for station {siteId}, username {username} do not exist.")
        {

        }
    }
}
