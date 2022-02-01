using HotChocolate.Resolvers;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebAPI.GraphQL.Stations;

namespace SysTk.WebAPI.GraphQL
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(x => x.GetStation(default!))
                .Authorize(Policies.IsVerified)
                .Argument("id", x => x.Type<StringType>())
                .Argument("cluster", x => x.Type<StringType>())
                .ResolveWith<Resolvers>(x => x.GetStation(default!, default!, default!))
                .UseDbContext<AppDbContext>()
                .UseProjection()
                .UseFiltering()
                .UseSorting();
        }

        private class Resolvers
        {
            public IQueryable<Station> GetStation([ScopedService] AppDbContext context, string id, string cluster)
            {
                if (cluster is null && id is null)
                    return context.Stations;

                return context.Stations.Where(x => x.Cluster == cluster || x.Id == id);
            }
        }
    }
}
