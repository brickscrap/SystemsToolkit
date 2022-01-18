using GraphQL.Server.Ui.Voyager;
using Microsoft.EntityFrameworkCore;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebAPI.GraphQL;
using SysTk.WebAPI.GraphQL.FtpCredential;
using SysTk.WebAPI.GraphQL.Stations;

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

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<StationType>()
    .AddType<FtpCredentialType>()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
});

app.UseGraphQLVoyager(new VoyagerOptions()
{
    GraphQLEndPoint = "/graphql"
}, "/graphql-voyager");

app.UseHttpsRedirection();

app.Run();
