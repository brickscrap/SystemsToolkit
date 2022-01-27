using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebAPI.GraphQL.FtpCredential;

namespace SysTk.WebAPI.Validators
{
    public class AddFtpCredentialsValidator : AbstractValidator<AddFtpCredentialsInput>
    {

        public AddFtpCredentialsValidator()
        {
            RuleFor(x => x.StationId).Length(5).WithMessage("Site ID must be exactly 5 characters.");
        }
    }
}
