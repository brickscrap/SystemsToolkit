using Microsoft.AspNetCore.Identity;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models.Auth;

namespace SysTk.WebAPI.GraphQL.Types
{
    public class UserType : ObjectType<AppUser>
    {
        protected override void Configure(IObjectTypeDescriptor<AppUser> descriptor)
        {
            descriptor.Description("Represents an API user, only accessible by admin users.");
            descriptor.Authorize(Policies.IsAdmin);

            descriptor.BindFieldsExplicitly();

            descriptor.Field(x => x.Roles)
                .ResolveWith<Resolvers>(x => x.GetRoles(default!, default!))
                .UseDbContext<AppDbContext>()
                .Description("The list of roles which this user is in.");

            descriptor.Field(x => x.FirstName);
            descriptor.Field(x => x.LastName);
            descriptor.Field(x => x.Email)
                .Type<NonNullType<StringType>>();
        }

        private class Resolvers
        {
            public async Task<List<string>> GetRoles([Parent] AppUser user, [Service] UserManager<AppUser> userManager)
            {
                var roles = await userManager.GetRolesAsync(user) as List<string>;

                return roles;
            }
        }
    }
}
