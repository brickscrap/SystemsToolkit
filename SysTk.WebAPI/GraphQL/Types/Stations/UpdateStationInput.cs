using System.ComponentModel.DataAnnotations;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Types.Stations
{
    public class UpdateStationInput
    {
        private string _id;

        [GraphQLType(typeof(NonNullType<IdType>))]
        public string Id
        {
            get { return _id.ToUpper(); }
            set { _id = value.ToUpper(); }
        }

        public string Name { get; set; }
        public string Ip { get; set; }
        public Cluster? Cluster { get; set; }
    }
}
