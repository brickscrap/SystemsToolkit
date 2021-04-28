using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SystemsUI.Authentication;
using SystemsUI.Library.API;

namespace SystemsUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // Authentication/authorisation
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddBlazoredLocalStorage();
            
            builder.Services.AddTransient<IPosEndpoint, PosEndpoint>();
            builder.Services.AddTransient<IStationEndpoint, StationEndpoint>();

            builder.Services.AddScoped(sp =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();

                return new HttpClient { BaseAddress = new Uri(cfg["apiLocation"]) };
            });

            await builder.Build().RunAsync();
        }
    }
}
