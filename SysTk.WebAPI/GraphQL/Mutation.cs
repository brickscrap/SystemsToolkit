using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;
using SysTk.WebAPI.GraphQL.FtpCredential;
using SysTk.WebAPI.GraphQL.Stations;
using SysTk.WebAPI.GraphQL.Users;
using SysTk.WebAPI.GraphQL.Errors;

namespace SysTk.WebAPI.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        public async Task<AddStationPayload> AddStationAsync(AddStationInput input,
            [ScopedService] AppDbContext context)
        {
            // TODO: Use Automapper
            var station = new Station
            {
                Id = input.Id,
                Name = input.Name,
                IP = input.IP,
                Cluster = input.Cluster
            };

            context.Stations.Add(station);
            await context.SaveChangesAsync();

            return new AddStationPayload(station);
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        public async Task<AddFtpCredentialsPayload> AddFtpCredentialsAsync(AddFtpCredentialsInput input,
            [ScopedService] AppDbContext context)
        {
            var stationExists = context.Stations.Where(x => x.Id == input.StationId).Any();
            if (!stationExists)
                throw new QueryException($"Station with ID {input.StationId} does not exist.");

            // TODO: Use AutoMapper
            var creds = new FtpCredentials
            {
                Username = input.Username,
                Password = input.Password,
                StationId = input.StationId
            };

            context.FtpCredentials.Add(creds);
            await context.SaveChangesAsync();

            return new AddFtpCredentialsPayload(creds);
        }

        [UseDbContext(typeof (AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        public async Task<DeleteStationPayload> DeleteStationAsync(DeleteStationInput input,
            [ScopedService] AppDbContext context)
        {
            // TODO: Handle station not existing
            var station = context.Stations.Where(x => x.Id == input.StationId).FirstOrDefault();

            context.Stations.Attach(station);
            context.Stations.Remove(station);
            await context.SaveChangesAsync();

            return new DeleteStationPayload(station);
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        public async Task<DeleteFtpCredentialsPayload> DeleteFtpCredentials(DeleteFtpCredentialsInput input,
            [ScopedService] AppDbContext context)
        {
            // TODO: Handle credentials IP not existing
            var credentials = context.FtpCredentials.Where(x => x.Id == input.Id).FirstOrDefault();

            context.FtpCredentials.Attach(credentials);
            context.FtpCredentials.Remove(credentials);
            await context.SaveChangesAsync();

            return new DeleteFtpCredentialsPayload(credentials);
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModifyUsers)]
        public async Task<AddUserPayload> AddUser(AddUserInput input,
            [ScopedService] AppDbContext context,
            [Service] UserManager<AppUser> userManager)
        {
            var user = new AppUser()
            {
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.Email
            };

            var userCreateResult = await userManager.CreateAsync(user, input.Password);

            if (userCreateResult.Succeeded)
            {
                var output = new AddUserPayload(input.FirstName, input.LastName, input.Email);
                return output;
            }

            throw new QueryException($"User could not be added: {userCreateResult.Errors}");
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModifyUsers)]
        public async Task<AddUserToRolePayload> AddUserToRole(AddUserToRoleInput input,
            [Service] UserManager<AppUser> userManager,
            [ScopedService] AppDbContext context)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            var roleExists = context.Roles.Where(x => x.Name == input.RoleName).Any();

            if (user is not null && roleExists)
            {
                var result = await userManager.AddToRoleAsync(user, input.RoleName);

                if(result.Succeeded)
                {
                    return new AddUserToRolePayload(input.Email, input.RoleName);
                }
                else
                {
                    throw new QueryException($"The user {input.Email} could not be added to role: {input.RoleName}. Reason: {result.Errors}");
                }
            }
            else
            {
                if (!roleExists)
                    throw new QueryException($"The role: {input.RoleName} does not exist.");

                if (user is null)
                    throw new QueryException($"The user: {input.Email} could not be found.");
            }

            // TODO: Handle this better
            throw new QueryException("Unknown error.");
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<LoginPayload> Login(LoginInput input,
            [ScopedService] AppDbContext context,
            [Service] UserManager<AppUser> userManager,
            [Service] IConfiguration config)
        {
            if (await IsValidUsernameAndPassword(userManager, input.Username, input.Password))
            {
                return await GenerateToken(userManager, context, input.Username, config);
            }
            else
            {
                throw new QueryException(ErrorFactory.CreateLoginError());
            }
        }

        // TODO: Put this somewhere else
        private async Task<bool> IsValidUsernameAndPassword(UserManager<AppUser> userManager, string userName, string password)
        {
            var user = await userManager.FindByEmailAsync(userName);
            return await userManager.CheckPasswordAsync(user, password);
        }

        // TODO: Spin this off into a token service
        private async Task<LoginPayload> GenerateToken(UserManager<AppUser> userManager, AppDbContext context, string username, IConfiguration config)
        {
            var user = await userManager.FindByEmailAsync(username);
            var roles = from ur in context.UserRoles
                        join r in context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(8)).ToUnixTimeSeconds().ToString())
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.Name));

            //TODO: Replace secret key from config
            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKey"])),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            var output = new LoginPayload
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Username = username
            };

            return output;
        }
    }
}
