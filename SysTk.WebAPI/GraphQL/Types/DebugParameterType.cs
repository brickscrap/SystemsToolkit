using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.GraphQL.Types
{
    public class DebugParameterType : ObjectType<DebugParameter>
    {
        protected override void Configure(IObjectTypeDescriptor<DebugParameter> descriptor)
        {
            descriptor.Description("Represents a possible parameter of a debug process");

            descriptor.Authorize(Policies.IsVerified);

            descriptor.Field(x => x.Id)
                .IsProjected(true);

            descriptor.Field(x => x.Description)
                .Description("Description of the parameter");

            descriptor.Field(x => x.Name)
                .IsProjected(true)
                .Description("The name of the parameter as it would be entered into the FuelPOS debug menu");
        }
    }
}
