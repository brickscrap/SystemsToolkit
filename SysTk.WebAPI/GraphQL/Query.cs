using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;

namespace SysTk.WebAPI.GraphQL
{
    public class Query
    {
        [Authorize(Policy = Policies.IsVerified)]
        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Station> GetStation([ScopedService] AppDbContext context) => context.Stations;

        [Authorize(Policy = Policies.IsVerified)]
        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<DebugProcess> GetDebugProcesses([ScopedService] AppDbContext context) => context.DebugProcesses;

        [Authorize(Policy = Policies.IsAdmin)]
        [UseFiltering]
        public IQueryable<AppUser> GetUser([Service] UserManager<AppUser> userManager) => userManager.Users;
    }
}
