using FluentValidation;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebAPI.Validators
{
    public class AddFtpCredentialsValidator : AbstractValidator<FtpCredentials>
    {

        public AddFtpCredentialsValidator()
        {
            RuleFor(x => x.StationId).Length(5).WithMessage("Site ID must be exactly 5 characters.");
        }
    }
}
