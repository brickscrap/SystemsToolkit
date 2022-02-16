using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Types
{
    public class StationType : ObjectType<Station>
    {
        protected override void Configure(IObjectTypeDescriptor<Station> descriptor)
        {
            descriptor.Description("Represents a FuelPOS station");

            descriptor.Authorize(Policies.IsVerified);

            descriptor.Field(x => x.Id)
                .Type<NonNullType<IdType>>()
                .Description("The station ID, equal to the Petrol Server ID")
                .IsProjected(true);

            descriptor.Field(x => x.Name)
                .Description("The name of the station");

            descriptor.Field(x => x.Cluster)
                .Description("The Petrol Server cluster the station belongs to");

            descriptor.Field(x => x.IP)
                .Description("The IP address of the FuelPOS CIS/INT of the station")
                .Name("ip");

            descriptor.Field(x => x.FtpCredentials)
                .Description("Any stored FTP credentials for the FuelPOS system at this station");
        }
    }
}
