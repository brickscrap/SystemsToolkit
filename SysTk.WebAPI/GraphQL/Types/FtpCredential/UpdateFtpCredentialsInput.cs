namespace SysTk.WebAPI.GraphQL.Types.FtpCredential
{
    public class UpdateFtpCredentialsInput
    {
        public string StationId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
