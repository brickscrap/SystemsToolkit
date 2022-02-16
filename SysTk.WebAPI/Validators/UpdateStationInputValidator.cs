using FluentValidation;
using SysTk.WebAPI.GraphQL.Types.Stations;

namespace SysTk.WebAPI.Validators
{
    public class UpdateStationInputValidator : AbstractValidator<UpdateStationInput>
    {
        public UpdateStationInputValidator()
        {
            RuleFor(x => x.Ip).IsValidIP().Unless(x => string.IsNullOrEmpty(x.Ip));
        }
    }
}
