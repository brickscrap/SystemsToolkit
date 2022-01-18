using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebAPI.GraphQL.Stations;

namespace SysTk.WebAPI.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddStationPayload> AddStationAsync(AddStationInput input,
            [ScopedService] AppDbContext context)
        {
            // TODO: Use Automapper
            var station = new Station
            {
                Id = input.Id,
                Name = input.Name,
                IP = input.IP,
                Cluster = input.Cluster
            };

            context.Stations.Add(station);
            await context.SaveChangesAsync();

            return new AddStationPayload(station);
        }
    }
}
