using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebAPI.GraphQL.Errors;
using SysTk.WebAPI.GraphQL.Types.Stations;

namespace SysTk.WebAPI.Validators
{
    public class AddStationInputValidator : AbstractValidator<AddStationInput>
    {
        public AddStationInputValidator()
        {
            RuleFor(x => x.Cluster).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Id).Length(5).WithMessage("Site ID must be exactly 5 characters.").WithErrorCode("InvalidId");
            RuleFor(x => x.Ip).Must(BeIPAddress).WithMessage("Invalid IP address provided.");
        }

        private bool BeIPAddress(string ip) => IPAddress.TryParse(ip, out _);
    }
}
