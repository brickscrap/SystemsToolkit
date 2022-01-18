using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.FtpCredential
{
    public class FtpCredentialType : ObjectType<FtpCredentials>
    {
        protected override void Configure(IObjectTypeDescriptor<FtpCredentials> descriptor)
        {
            descriptor.Description("Represents a set of FTP credentials for a FuelPOS system.");

            descriptor.Field(x => x.Station)
                .ResolveWith<Resolvers>(x => x.GetStation(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("This is the station on which the credentials are valid.");
        }

        private class Resolvers
        {
            public Station GetStation([Parent] FtpCredentials credentials, [ScopedService] AppDbContext context) =>
                context.Stations.FirstOrDefault(x => x.Id == credentials.StationId);
        }
    }
}
