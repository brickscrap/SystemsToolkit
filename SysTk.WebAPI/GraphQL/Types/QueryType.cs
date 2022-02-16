using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Types
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(x => x.GetStation(default!))
                .Authorize(Policies.IsVerified)
                .Argument("id", x => x.Type<StringType>())
                .Argument("cluster", x => x.Type<ClusterType>())
                .ResolveWith<Resolvers>(x => x.GetStation(default!, default!, default!))
                .UseDbContext<AppDbContext>()
                .UseProjection()
                .UseFiltering()
                .UseSorting();

            descriptor.Field(x => x.GetDebugProcesses(default!))
                .Authorize(Policies.IsVerified)
                .Argument("id", x => x.Type<IntType>())
                .Argument("name", x => x.Type<StringType>())
                .ResolveWith<Resolvers>(x => x.GetDebugProcess(default!, default!, default!))
                .UseDbContext<AppDbContext>()
                .UseProjection()
                .UseFiltering()
                .UseSorting();
        }

        private class Resolvers
        {
            public IQueryable<Station> GetStation([ScopedService] AppDbContext context, string id, Cluster? cluster)
            {
                if (cluster is null && id is null)
                    return context.Stations;


                return context.Stations.Where(x => x.Cluster == cluster || x.Id == id);
            }

            public IQueryable<DebugProcess> GetDebugProcess([ScopedService] AppDbContext context, int id, string name)
            {
                if (id == 0 && name is null)
                    return context.DebugProcesses;

                return context.DebugProcesses.Where(x => x.Name == name || x.Id == id);
            }
        }
    }
}
