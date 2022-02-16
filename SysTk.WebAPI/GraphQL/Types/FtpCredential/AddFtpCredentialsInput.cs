using System.ComponentModel.DataAnnotations;

namespace SysTk.WebAPI.GraphQL.Types.FtpCredential
{
    public class AddFtpCredentialsInput
    {
        private string _stationId;

        [GraphQLType(typeof(NonNullType<IdType>))]
        public string StationId
        {
            get { return _stationId.ToUpper(); }
            set { _stationId = value.ToUpper(); }
        }


        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
