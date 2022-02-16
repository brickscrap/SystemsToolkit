using System.ComponentModel.DataAnnotations;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Types.Stations
{
    public class AddStationInput
    {
        private string _id;

        [GraphQLType(typeof(NonNullType<IdType>))]
        public string Id
        {
            get { return _id.ToUpper(); }
            set { _id = value.ToUpper(); }
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Ip { get; set; }
        [Required]
        public Cluster Cluster { get; set; }
    }
}
