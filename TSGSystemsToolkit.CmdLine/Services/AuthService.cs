using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.GraphQL;

namespace TSGSystemsToolkit.CmdLine.Services
{
    public class AuthService : IAuthService
    {
        private readonly SysTkApiClient _apiClient;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;

        public AuthService(SysTkApiClient apiClient, ILogger<AuthService> logger, IConfiguration config)
        {
            _apiClient = apiClient;
            _logger = logger;
            _config = config;
        }

        public async Task<bool> Authenticate()
        {
            return await Authenticate(0);
        }

        public async Task<bool> Authenticate(int counter)
        {
            if (counter == 3)
                return false;

            Console.WriteLine("Please enter your API password:");
            var result = await _apiClient.GetToken.ExecuteAsync(_config["EmailAddress"], GetHiddenConsoleInput());

            if (result.Errors.Count > 0)
            {
                var loginFailure = result.Errors.Where(x => x.Code == "LOGIN_FAILURE");

                if (loginFailure.Any())
                {
                    Console.WriteLine($"{loginFailure.FirstOrDefault().Message}");
                    await Authenticate(counter += 1);
                }
            }

            try
            {
                Extensions.UpdateAccessToken(result.Data.Login.AccessToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to update Appsettings.json: {Message}", ex.Message);
                return false;
            }
            
            return true;
        }

        private static string GetHiddenConsoleInput()
        {
            StringBuilder input = new();

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }

            return input.ToString();
        }
    }
}
