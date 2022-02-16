using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebAPI.GraphQL.Errors;
using SysTk.WebAPI.GraphQL.Types.Stations;

namespace SysTk.WebAPI.GraphQL.Types
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            
        }
    }
}
