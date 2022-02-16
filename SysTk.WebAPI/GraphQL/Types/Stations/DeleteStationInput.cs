using System.ComponentModel.DataAnnotations;

namespace SysTk.WebAPI.GraphQL.Types.Stations
{
    public class DeleteStationInput
    {
        private string _id;

        [GraphQLType(typeof(NonNullType<IdType>))]
        public string Id
        {
            get { return _id.ToUpper(); }
            set { _id = value.ToUpper(); }
        }

    }
}
