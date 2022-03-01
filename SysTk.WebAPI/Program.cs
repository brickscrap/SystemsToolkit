using Bogus;
using FairyBread;
using FluentValidation;
using FluentValidation.AspNetCore;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;
using SysTk.WebAPI;
using SysTk.WebAPI.GraphQL.Errors;
using SysTk.WebAPI.GraphQL.Types;
using SysTk.WebAPI.Services;
using SysTk.WebAPI.Validators;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
};

var builder = WebApplication.CreateBuilder(options);

if (builder.Environment.IsProduction())
{
    builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.File("C:\\logs\\log.txt"));
    
}

if (WindowsServiceHelpers.IsWindowsService())
{
    builder.Services.AddSingleton<IHostLifetime, WindowsServiceLifetime>();
    builder.Logging.AddEventLog(settings =>
    {
        if (string.IsNullOrEmpty(settings.SourceName))
        {
            settings.SourceName = builder.Environment.ApplicationName;
        }
    });
}

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile(builder.Configuration["certPath"], optional: true, reloadOnChange: true);

    var certificateSettings = builder.Configuration.GetSection("certificateSettings");
    string certFileName = certificateSettings.GetValue<string>("filename");
    string certPassword = certificateSettings.GetValue<string>("password");

    var certificate = new X509Certificate2(certFileName, certPassword);

    builder.WebHost.UseKestrel(options =>
    {
        options.AddServerHeader = false;
        options.Listen(IPAddress.Loopback, 14592, listenOptions =>
        {
            listenOptions.UseHttps(certificate);
        });
    });

    builder.Services.AddAntiforgery(options =>
    {
        options.Cookie.Name = "_af";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.HeaderName = "X-XSRF-TOKEN";
    });
}

builder.Host.UseWindowsService();

builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sqlServerOptionsAction: options =>
        {
            options.MigrationsAssembly("SysTk.WebApi.Data");
            options.EnableRetryOnFailure();
        });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

string jwtKey = builder.Configuration["JwtKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
    .AddJwtBearer("JwtBearer", jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.CanDelete, policy =>
    {
        policy.RequireRole(new[] { Roles.Admin, Roles.Supervisor });
    });

    options.AddPolicy(Policies.CanAdd, policy =>
    {
        policy.RequireRole(new[] { Roles.Admin, Roles.Supervisor, Roles.Member });
    });

    options.AddPolicy(Policies.CanModify, policy =>
    {
        policy.RequireRole(new[] { Roles.Admin, Roles.Supervisor });
    });

    options.AddPolicy(Policies.CanModifyUsers, policy =>
    {
        policy.RequireRole(Roles.Admin);
    });

    options.AddPolicy(Policies.IsVerified, policy =>
    {
        policy.RequireRole(new[] { Roles.Admin, Roles.Supervisor, Roles.Member });
    });

    options.AddPolicy(Policies.IsAdmin, policy =>
    {
        policy.RequireRole(Roles.Admin);
    });
});

builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddFluentValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddStationInputValidator>();
builder.Services.AddSingleton<IValidationErrorsHandler, CustomValidationErrorsHandler>();

builder.Services.AddGraphQLServer()
    .AddFairyBread()
    .AddMutationConventions(false)
    .AddQueryType<QueryType>()
    .AddMutationType<MutationType>()
    .AddType<StationType>()
    .AddType<UserType>()
    .AddType<FtpCredentialType>()
    .AddType<DebugProcessType>()
    .AddType<DebugParameterType>()
    .AddAuthorization()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddErrorFilter<ValidationFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await SeedTestData(app.Services);
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

await CreateRoles(app.Services);

app.UseGraphQLVoyager(new VoyagerOptions()
{
    GraphQLEndPoint = "/graphql"
}, "/graphql-voyager");

app.UseHttpsRedirection();

app.Run();


async Task CreateRoles(IServiceProvider services)
{
    using var svc = services.CreateScope();
    var roleManager = svc.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var userManager = svc.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    string[] roleNames = { "Admin", "Supervisor", "Member" };

    foreach (var role in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new Role(role));
    }

    var powerUser = new AppUser
    {
        UserName = app.Configuration["PowerUser:Username"],
        Email = app.Configuration["PowerUser:Username"]
    };

    powerUser.Email = app.Configuration["PowerUser:Username"];

    string userPassword = app.Configuration["PowerUser:Password"];

    var user = await userManager.FindByEmailAsync(app.Configuration["AdminUserEmail"]);

    if (user is null)
    {
        var createPowerUser = await userManager.CreateAsync(powerUser, userPassword);
        if (createPowerUser.Succeeded)
            await userManager.AddToRoleAsync(powerUser, "Admin");
    }
    else
    {
        var createAdmin = await userManager.AddToRoleAsync(user, "Admin");
    }
}

async Task SeedTestData(IServiceProvider services)
{
    using var svc = services.CreateScope();
    var db = svc.ServiceProvider.GetService<AppDbContext>();

    if (db.Stations.Count() < 10)
    {
        Random rnd = new();

        var ftpCredentials = new Faker<FtpCredentials>()
            .RuleFor(x => x.Password, x => x.Internet.Password())
            .RuleFor(x => x.Username, x => x.Internet.UserName());

        var stationFaker = new Faker<Station>()
            .RuleFor(x => x.Name, x => x.Company.CompanyName())
            .RuleFor(x => x.IP, x => x.Internet.Ip())
            .RuleFor(x => x.FtpCredentials, x => ftpCredentials.Generate(rnd.Next(1, 2)))
            .RuleFor(x => x.Id, x => x.Random.AlphaNumeric(5))
            .RuleFor(x => x.Cluster, x => x.PickRandom<Cluster>());

        var station = stationFaker.Generate(600);

        db.AddRange(station);
        await db.SaveChangesAsync();
    }
}