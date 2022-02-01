using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Stations
{
    public class StationType : ObjectType<Station>
    {
        protected override void Configure(IObjectTypeDescriptor<Station> descriptor)
        {
            descriptor.Description("Represents a FuelPOS station");

            descriptor.Authorize(Policies.IsVerified);

            descriptor.Field(s => s.FtpCredentials)
                .ResolveWith<Resolvers>(x => x.GetFtpCredentials(default!, default!))
                .UseDbContext<AppDbContext>()
                .UseProjection()
                .Description("This is the list of available FTP credentials for this station.");
        } 

        private class Resolvers
        {
            public IQueryable<FtpCredentials> GetFtpCredentials([Parent] Station station, [ScopedService] AppDbContext context) =>
                context.FtpCredentials.Where(x => x.StationId == station.Id);
        }
    }
}
