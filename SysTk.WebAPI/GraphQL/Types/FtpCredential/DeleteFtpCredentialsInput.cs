namespace SysTk.WebAPI.GraphQL.Types.FtpCredential
{
    public class DeleteFtpCredentialsInput
    {
        public string StationId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
    }
}
