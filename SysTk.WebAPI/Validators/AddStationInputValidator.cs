using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebAPI.GraphQL.Types.Stations;

namespace SysTk.WebAPI.Validators
{
    public class AddStationInputValidator : AbstractValidator<AddStationInput>
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public AddStationInputValidator(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;

            RuleFor(x => x.Cluster).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Id).Length(5).WithMessage("Site ID must be exactly 5 characters.");
            RuleFor(x => x.Ip).Must(BeIPAddress).WithMessage("Invalid IP address provided.")
                .Must(IPNotExist).WithMessage("IP address already exists.");
        }

        private bool BeIPAddress(string ip) => IPAddress.TryParse(ip, out _);

        private bool IPNotExist(string ip)
        {
            using var context = _contextFactory.CreateDbContext();

            return !context.Stations.Where(x => x.IP == ip).Any();
        }
    }
}
