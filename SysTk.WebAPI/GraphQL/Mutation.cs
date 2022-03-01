using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SysTk.WebApi.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;
using SysTk.WebAPI.GraphQL.Errors;
using SysTk.WebAPI.GraphQL.Types.DebugParameters;
using SysTk.WebAPI.GraphQL.Types.DebugProcesses;
using SysTk.WebAPI.GraphQL.Types.FtpCredential;
using SysTk.WebAPI.GraphQL.Types.Stations;
using SysTk.WebAPI.GraphQL.Types.Users;
using SysTk.WebAPI.Services;
using SysTk.WebApi.Data.Extensions;

namespace SysTk.WebAPI.GraphQL
{
    public class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        [UseMutationConvention]
        [Error(typeof(StationExistsError))]
        public async Task<Station> AddStationAsync(AddStationInput input,
            [ScopedService] AppDbContext context)
        {
            var existingStation = context.Stations
                .Where(x => x.IP == input.Ip || x.Id == input.Id)
                .Select(x => new { IP = x.IP, Id = x.Id })
                .FirstOrDefault();

            if (existingStation is not null)
                throw new StationExistsError(input.Id, input.Ip, existingStation.IP, existingStation.Id);
            
            var station = new Station
            {
                Id = input.Id.ToUpper(),
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
        [Error(typeof(StationNotExistsError))]
        public async Task<Station> UpdateStationAsync(UpdateStationInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.Stations.Exists(id: input.Id))
                throw new StationNotExistsError(input.Id);

            var station = context.Stations
                .FirstOrDefault(x => x.Id == input.Id);

            station.Name = input.Name ?? station.Name;
            station.IP = input.Ip ?? station.IP;
            station.Cluster = input.Cluster ?? station.Cluster;

            context.Stations.Update(station);
            await context.SaveChangesAsync();

            station.FtpCredentials = context.GetChildren(station);

            return station;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        [UseMutationConvention]
        [Error(typeof(StationNotExistsError))]
        public async Task<Station> DeleteStationAsync(DeleteStationInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.Stations.Exists(id: input.Id))
                throw new StationNotExistsError(input.Id);

            var station = context.Stations.Where(x => x.Id == input.Id).FirstOrDefault();

            station.FtpCredentials = context.GetChildren(station);

            context.Stations.Attach(station);
            context.Stations.Remove(station);
            await context.SaveChangesAsync();

            return station;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        [UseMutationConvention]
        [Error(typeof(StationNotExistsError))]
        [Error(typeof(FtpCredentialsExistsError))]
        public async Task<FtpCredentials> AddFtpCredentialsAsync(AddFtpCredentialsInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.Stations.Exists(id: input.StationId))
                throw new StationNotExistsError(input.StationId);

            if(context.FtpCredentials.Exists(input.Username, input.StationId))
                throw new FtpCredentialsExistsError(input.Username, input.StationId);

            var creds = new FtpCredentials
            {
                Username = input.Username,
                Password = input.Password,
                StationId = input.StationId
            };

            context.FtpCredentials.Add(creds);
            await context.SaveChangesAsync();

            creds.Station = context.GetParent(creds);

            return creds;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModify)]
        [UseMutationConvention]
        [Error(typeof(StationNotExistsError))]
        [Error(typeof(FtpCredentialsNotExistError))]
        public async Task<FtpCredentials> UpdateFtpCredentialsAsync(UpdateFtpCredentialsInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.Stations.Exists(id: input.StationId))
                throw new StationNotExistsError(input.StationId);

            var statCreds = context.Stations.GetStationWithCredentials(input.StationId, input.Username);

            if (statCreds is null)
                throw new FtpCredentialsNotExistError(input.Username, input.StationId);

            var credentials = statCreds.FtpCredentials.FirstOrDefault();

            credentials.Username = input.Username ?? credentials.Username;
            credentials.Password = input.Password ?? credentials.Password;

            context.FtpCredentials.Update(credentials);

            await context.SaveChangesAsync();

            credentials.Station = statCreds;

            return credentials;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        [UseMutationConvention]
        [Error(typeof(StationNotExistsError))]
        [Error(typeof(FtpCredentialsNotExistError))]
        public async Task<FtpCredentials> DeleteFtpCredentialsAsync(DeleteFtpCredentialsInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.Stations.Exists(id: input.StationId))
                throw new StationNotExistsError(input.StationId);

            var statCreds = context.Stations.GetStationWithCredentials(input.StationId, input.Username, input.Id);

            if (statCreds is null)
                throw new FtpCredentialsNotExistError(input.Username, input.StationId);

            var credentials = statCreds.FtpCredentials.First();

            credentials.Station = statCreds;

            context.FtpCredentials.Attach(credentials);
            context.FtpCredentials.Remove(credentials);
            await context.SaveChangesAsync();

            return credentials;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        [UseMutationConvention]
        [Error(typeof(DebugProcessExistsError))]
        public async Task<DebugProcess> AddDebugProcessAsync(AddDebugProcessInput input,
            [ScopedService] AppDbContext context)
        {
            if (context.DebugProcesses.Exists(input.Name))
                throw new DebugProcessExistsError(input.Name);

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
        [Authorize(Policy = Policies.CanModify)]
        [UseMutationConvention]
        [Error(typeof(DebugProcessNotExistsError))]
        public async Task<DebugProcess> UpdateDebugProcessAsync(UpdateDebugProcessInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.DebugProcesses.Exists(input.Name))
                throw new DebugProcessNotExistsError(input.Name);

            var process = context.DebugProcesses
                .First(x => x.Name.ToUpper() == input.Name.ToUpper());

            process.Name = input.Name ?? process.Name;
            process.Description = input.Description ?? process.Description;

            context.DebugProcesses.Update(process);

            await context.SaveChangesAsync();

            process.Parameters = context.GetChildren(process);

            return process;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        [UseMutationConvention]
        [Error(typeof(DebugProcessNotExistsError))]
        public async Task<DebugProcess> DeleteDebugProcessAsync(DeleteDebugProcessInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.DebugProcesses.Exists(input.Id))
                throw new DebugProcessNotExistsError(input.Id);

            var process = context.DebugProcesses
                .First(x => x.Id == input.Id);

            process.Parameters = context.GetChildren(process);

            context.DebugProcesses.Attach(process);
            context.DebugProcesses.Remove(process);

            await context.SaveChangesAsync();

            return process;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanAdd)]
        [UseMutationConvention]
        [Error(typeof(DebugParamExistsError))]
        public async Task<DebugParameter> AddDebugParameterAsync(AddDebugParameterInput input,
            [ScopedService] AppDbContext context)
        {
            if (context.DebugParameters.Exists(input.DebugProcessId, input.Name))
                throw new DebugParamExistsError(input.Name, input.DebugProcessId);

            var param = new DebugParameter
            {
                Name = input.Name,
                Description = input.Description,
                DebugProcessId = input.DebugProcessId
            };

            context.DebugParameters.Add(param);
            await context.SaveChangesAsync();

            param = context.DebugParameters
                .FirstOrDefault(x => x.Name == input.Name);

            param.Process = context.GetParent(param);

            return param;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModify)]
        [UseMutationConvention]
        [Error(typeof(DebugProcessNotExistsError))]
        [Error(typeof(DebugParameterNotExistsError))]
        public async Task<DebugParameter> UpdateDebugParameterAsync(UpdateDebugParameterInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.DebugProcesses.Exists(input.DebugProcessName))
                throw new DebugProcessNotExistsError(input.DebugProcessName);

            var process = context.DebugProcesses.GetProcessWithParameters(input.DebugProcessName, input.Name);

            if (process is null)
                throw new DebugParameterNotExistsError(input.Name, input.DebugProcessName);

            var param = process.Parameters.First();

            param.Process = process;

            param.Name = input.Name ?? param.Name;
            param.Description = input.Description ?? param.Description;

            context.DebugParameters.Update(param);

            await context.SaveChangesAsync();

            return param;
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanDelete)]
        [UseMutationConvention]
        [Error(typeof(DebugProcessNotExistsError))]
        [Error(typeof(DebugParameterNotExistsError))]
        public async Task<DebugParameter> DeleteDebugParameterAsync(DeleteDebugParameterInput input,
            [ScopedService] AppDbContext context)
        {
            if (!context.DebugProcesses.Exists(input.DebugProcessName))
                throw new DebugProcessNotExistsError(input.Name);

            var process = context.DebugProcesses.GetProcessWithParameters(input.DebugProcessName, input.Name);

            if (process is null)
                throw new DebugParameterNotExistsError(input.Name, input.DebugProcessName);

            var param = process.Parameters.First();

            var paramProcess = context.GetParent(param);

            param.Process = paramProcess;

            context.DebugParameters.Attach(param);
            context.DebugParameters.Remove(param);

            await context.SaveChangesAsync();

            return param;
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
                return user;

            throw new QueryException($"User could not be added: {userCreateResult.Errors}");
        }

        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.CanModifyUsers)]
        [UseMutationConvention]
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
                    return new AddUserToRolePayload(input.Email, input.Role.ToString());
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
            if (!await tokenService.IsValidUsernameAndPassword(input.Username, input.Password))
            {
                var error = ErrorBuilder.New()
                    .SetMessage("Login failed. The username or password provided is invalid.")
                    .SetCode("LOGIN_FAILURE")
                    .ClearExtensions()
                    .Build();

                throw new QueryException(error);
            }

            var output = await tokenService.GenerateToken(input.Username);
            return new LoginPayload(output.AccessToken, output.Username);
        }
    }
}
