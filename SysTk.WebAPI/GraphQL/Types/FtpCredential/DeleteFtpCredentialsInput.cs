using System.ComponentModel.DataAnnotations;

namespace SysTk.WebAPI.GraphQL.Types.FtpCredential
{
    public class DeleteFtpCredentialsInput
    {
        private string _stationId;

        [GraphQLType(typeof(NonNullType<IdType>))]
        public string StationId
        {
            get { return _stationId.ToUpper(); }
            set { _stationId = value.ToUpper(); }
        }

        public string Username { get; set; }
        public string Password { get; set; }
        [GraphQLType(typeof(IdType))]
        public int Id { get; set; }
    }
}
