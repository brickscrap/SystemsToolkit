using Bogus;
using FluentValidation;
using FluentValidation.AspNetCore;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;
using SysTk.WebAPI;
using SysTk.WebAPI.GraphQL;
using SysTk.WebAPI.GraphQL.Errors;
using SysTk.WebAPI.GraphQL.FtpCredential;
using SysTk.WebAPI.GraphQL.Stations;
using SysTk.WebAPI.Services;
using SysTk.WebAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPooledDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
        sqlServerOptionsAction: options =>
        {
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

string jwtKey;

if (builder.Environment.IsProduction())
{
    try
    {
        jwtKey = Environment.GetEnvironmentVariable("JwtKey");
        builder.Configuration["JwtKey"] = jwtKey;
    }
    catch (ArgumentNullException)
    {
        Console.WriteLine("Could not find JwtKey environment variable.");
        throw;
    }
}
else
    jwtKey = builder.Configuration["JwtKey"];

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
    options.AddPolicy("CanDelete", policy =>
    {
        policy.RequireRole(new[] { "Admin", "Supervisor" });
    });

    options.AddPolicy("CanAdd", policy =>
    {
        policy.RequireRole(new[] { "Admin", "Supervisor", "Member" });
    });

    options.AddPolicy("CanModifyUsers", policy =>
    {
        policy.RequireRole("Admin");
    });

    options.AddPolicy("IsVerified", policy =>
    {
        policy.RequireRole(new[] { Roles.Admin, Roles.Supervisor, Roles.Member });
    });
});

builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddFluentValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddStationInputValidator>();

builder.Services.AddGraphQLServer()
    .AddFairyBread()
    .AddQueryType<QueryType>()
    .AddMutationType<Mutation>()
    .AddType<StationType>()
    .AddType<FtpCredentialType>()
    .AddType<AppUser>()
    .AddAuthorization()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddErrorFilter<ValidationFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

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
        Email = app.Configuration["PowerUser:Email"]
    };

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
        Random rnd = new Random();
        List<string> clusters = new List<string> { "ShellRBA", "ShellRFA", "Indep", "RontecUK", "Esso" };

        var ftpCredentials = new Faker<FtpCredentials>()
            .RuleFor(x => x.Password, x => x.Internet.Password())
            .RuleFor(x => x.Username, x => x.Internet.UserName())
            .RuleFor(x => x.LastModified, x => x.Date.Recent());

        var stationFaker = new Faker<Station>()
            .RuleFor(x => x.Name, x => x.Company.CompanyName())
            .RuleFor(x => x.IP, x => x.Internet.Ip())
            .RuleFor(x => x.FtpCredentials, x => ftpCredentials.Generate(rnd.Next(1, 2)))
            .RuleFor(x => x.Id, x => x.Random.AlphaNumeric(5))
            .RuleFor(x => x.Cluster, x => x.PickRandom(clusters));

        var station = stationFaker.Generate(600);

        db.AddRange(station);
        db.SaveChanges();
    }
}