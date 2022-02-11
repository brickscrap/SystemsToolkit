using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Types
{
    public class DebugProcessType : ObjectType<DebugProcess>
    {
        protected override void Configure(IObjectTypeDescriptor<DebugProcess> descriptor)
        {
            descriptor.Description("Represents a FuelPOS process which is available for debugging");

            descriptor.Authorize(Policies.IsVerified);

            descriptor.Field(x => x.Id)
                .IsProjected(true);

            descriptor.Field(x => x.Name)
                .Description("The name of the process as seen in the FuelPOS debug menu");

            descriptor.Field(x => x.Description)
                .Description("A friendly description of the FuelPOS process, and what debugging it provides");

            descriptor.Field(x => x.Parameters)
                .Description("Available parameters for the process");
        }
    }
}
