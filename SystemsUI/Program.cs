using Blazored.LocalStorage;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcGreeter;
using GrpcServer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
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
            builder.Services.AddTransient<IDebugEndpoint, DebugEndpoint>();

            // gRPC
            builder.Services.AddSingleton(services =>
            {
                var cfg = services.GetRequiredService<IConfiguration>();
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var grpcEndpoint = new Uri(cfg["grpcEndpoint"]);
                var channel = GrpcChannel.ForAddress(grpcEndpoint, new GrpcChannelOptions { HttpClient = httpClient });

                return new Greeter.GreeterClient(channel);
            });

            builder.Services.AddSingleton(services =>
            {
                var cfg = services.GetRequiredService<IConfiguration>();
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var grpcEndpoint = new Uri(cfg["grpcEndpoint"]);
                var channel = GrpcChannel.ForAddress(grpcEndpoint, new GrpcChannelOptions { HttpClient = httpClient });

                return new FpDebug.FpDebugClient(channel);
            });

            builder.Services.AddScoped(sp =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();

                return new HttpClient { BaseAddress = new Uri(cfg["apiLocation"]) };
            });

            await builder.Build().RunAsync();
        }
    }
}
