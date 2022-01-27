using HotChocolate.AspNetCore.Authorization;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;

namespace SysTk.WebAPI.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Station> GetStation([ScopedService] AppDbContext context) => context.Stations;

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<FtpCredentials> GetFtpCredentials([ScopedService] AppDbContext context) => context.FtpCredentials;
    }
}
