using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Identity;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;
using SysTk.WebAPI.GraphQL.Errors;
using SysTk.WebAPI.GraphQL.Types.DebugProcesses;
using SysTk.WebAPI.GraphQL.Types.FtpCredential;
using SysTk.WebAPI.GraphQL.Types.Stations;
using SysTk.WebAPI.GraphQL.Types.Users;
using SysTk.WebAPI.Services;

namespace SysTk.WebAPI.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        [UseMutationConvention]
        public async Task<Station> AddStationAsync(AddStationInput input,
            [ScopedService] AppDbContext context)
        {
            // TODO: Use Automapper
            var station = new Station
            {
                Id = input.Id,
                Name = input.Name,
                IP = input.Ip,
                Cluster = input.Cluster
            };

            context.Stations.Add(station);
            await context.SaveChangesAsync();

            return station;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModify)]
        [UseMutationConvention]
        public async Task<Station> UpdateStationAsync(UpdateStationInput input,
            [ScopedService] AppDbContext context)
        {
            var station = context.Stations.FirstOrDefault(x => x.Id == input.Id);

            if (station is not null)
            {
                station.Name = input.Name ?? station.Name;
                station.IP = input.Ip ?? station.IP;
                station.Cluster = input.Cluster ?? station.Cluster;

                context.Stations.Update(station);
                await context.SaveChangesAsync();
            }
            else
            {
                // TODO: Throw appropriately
            }

            return station;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        [UseMutationConvention]
        public async Task<Station> DeleteStationAsync(DeleteStationInput input,
            [ScopedService] AppDbContext context)
        {
            // TODO: Handle station not existing
            var station = context.Stations.Where(x => x.Id == input.Id).FirstOrDefault();

            context.Stations.Attach(station);
            context.Stations.Remove(station);
            await context.SaveChangesAsync();

            return station;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        [UseMutationConvention]
        public async Task<FtpCredentials> AddFtpCredentialsAsync(AddFtpCredentialsInput input,
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

            return creds;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModify)]
        [UseMutationConvention]
        public async Task<FtpCredentials> UpdateFtpCredentialsAsync(UpdateFtpCredentialsInput input,
            [ScopedService] AppDbContext context)
        {
            var credentials = context.FtpCredentials.Where(x => x.StationId == input.StationId).FirstOrDefault(x => x.Username == input.Username);

            if (credentials is not null)
            {
                credentials.Username = input.Username ?? credentials.Username;
                credentials.Password = input.Password ?? credentials.Password;

                context.FtpCredentials.Update(credentials);

                await context.SaveChangesAsync();
            }
            else
            {
                // TODO: Throw appropriately
            }

            return credentials;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        [UseMutationConvention]
        public async Task<FtpCredentials> DeleteFtpCredentialsAsync(DeleteFtpCredentialsInput input,
            [ScopedService] AppDbContext context)
        {
            // TODO: Handle credentials IP not existing
            var credentials = context.FtpCredentials.Where(x => x.Id == input.Id ||
            (x.StationId == input.StationId && x.Username == input.Username))
                .FirstOrDefault();

            context.FtpCredentials.Attach(credentials);
            context.FtpCredentials.Remove(credentials);
            await context.SaveChangesAsync();

            return credentials;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        [UseMutationConvention]
        public async Task<DebugProcess> AddDebugProcessAsync(AddDebugProcessInput input,
            [ScopedService] AppDbContext context)
        {
            var processExists = context.DebugProcesses.Where(x => x.Name == input.Name).Any();
            if (processExists)
                throw new QueryException($"Process with name {input.Name} already exists");

            var proc = new DebugProcess
            {
                Name = input.Name,
                Description = input.Description
            };

            context.DebugProcesses.Add(proc);
            await context.SaveChangesAsync();

            return proc;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModifyUsers)]
        [UseMutationConvention]
        public async Task<AppUser> AddUser(AddUserInput input,
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
                return user;
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
            var roleExists = context.Roles.Where(x => x.Name == input.Role.ToString()).Any();

            if (user is not null && roleExists)
            {
                var result = await userManager.AddToRoleAsync(user, input.Role.ToString());

                if (result.Succeeded)
                {
                    return new AddUserToRolePayload(input.Email, input.Role.ToString());
                }
                else
                {
                    throw new QueryException($"The user {input.Email} could not be added to role: {input.Role}. Reason: {result.Errors}");
                }
            }
            else
            {
                if (!roleExists)
                    throw new QueryException($"The role: {input.Role} does not exist.");

                if (user is null)
                    throw new QueryException($"The user: {input.Email} could not be found.");
            }

            // TODO: Handle this better
            throw new QueryException("Unknown error.");
        }

        public async Task<LoginPayload> Login(LoginInput input,
            [Service] ITokenService tokenService)
        {
            if (await tokenService.IsValidUsernameAndPassword(input.Username, input.Password))
            {
                var output = await tokenService.GenerateToken(input.Username);
                return new LoginPayload { AccessToken = output.AccessToken, Username = output.Username };
            }
            else
            {
                throw new QueryException(ErrorFactory.CreateLoginError());
            }
        }
    }
}
