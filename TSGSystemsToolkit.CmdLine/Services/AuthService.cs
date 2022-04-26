using Microsoft.Extensions.Configuration;
using TSGSystemsToolkit.CmdLine.GraphQL;

namespace TSGSystemsToolkit.CmdLine.Services;

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

    public async Task<bool> Authenticate(Func<InvocationContext, Task<int>> callback, InvocationContext context, CancellationToken ct = default)
    {
        AnsiConsole.MarkupLine("[bold red]Not authenticated.[/] Current token invalid or expired. Proceed with login process.");

        var authResult = await Authenticate(0, callback, context, ct);

        if (authResult)
        {
            AnsiConsole.MarkupLine("[bold green]Login success.[/]");
            await callback(context);
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Authentication failed.[/]");
            return false;
        }
    }

    public async Task<bool> Authenticate(int counter, Func<InvocationContext, Task<int>> callback, InvocationContext context, CancellationToken ct = default)
    {
        if (counter == 3)
        {
            return false;
        }

        Console.WriteLine("Please enter your API password:");
        var result = await _apiClient.GetToken.ExecuteAsync(_config["EmailAddress"], GetHiddenConsoleInput(), ct);

        if (result.Errors.Count > 0)
        {
            var loginFailure = result.Errors.Where(x => x.Code == "LOGIN_FAILURE");

            if (loginFailure.Any())
            {
                Console.WriteLine($"{loginFailure.FirstOrDefault().Message}");
                await Authenticate(counter += 1, callback, context, ct);
                return false;
            }
        }

        try
        {
            Extensions.UpdateAccessToken(result.Data.Login.AccessToken);
            _config["AccessToken"] = result.Data.Login.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to update Appsettings.json: {Message}", ex.Message);
            return false;
        }

        return true;
    }

    private static string GetHiddenConsoleInput() =>
        AnsiConsole.Prompt(
            new TextPrompt<string>("Enter your API password:")
            .PromptStyle("red")
            .Secret());

    public async Task<bool> AuthSimple()
    {
        AnsiConsole.MarkupLine("[bold red]Not authenticated.[/] Current token invalid or expired. Proceed with login process.");

        var authResult = await AuthSimple(0);

        if (authResult)
        {
            AnsiConsole.MarkupLine("[bold green]Login success.[/]");
            return true;
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Authentication failed.[/]");
            return false;
        }
    }

    public async Task<bool> AuthSimple(int counter)
    {
        if (counter == 3)
        {
            return false;
        }

        Console.WriteLine("Please enter your API password:");
        var result = await _apiClient.GetToken.ExecuteAsync(_config["EmailAddress"], GetHiddenConsoleInput());

        if (result.Errors.Count > 0)
        {
            var loginFailure = result.Errors.Where(x => x.Code == "LOGIN_FAILURE");

            if (loginFailure.Any())
            {
                Console.WriteLine($"{loginFailure.FirstOrDefault().Message}");
                await AuthSimple(counter += 1);
                return false;
            }
        }

        try
        {
            Extensions.UpdateAccessToken(result.Data.Login.AccessToken);
            _config["AccessToken"] = result.Data.Login.AccessToken;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unable to update Appsettings.json: {Message}", ex.Message);
            return false;
        }

        return true;
    }
}
