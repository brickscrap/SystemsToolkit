using FuelPOS.StatDevParser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using TsgSystems.Api.Data;
using TsgSystems.Api.Services;
using TsgSystems.Api.Swagger;
using TsgSystemsToolkit.DataManager;
using TsgSystemsToolkit.DataManager.Dapper;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Extensions;

namespace TsgSystems.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(policy =>
            {
                policy.AddPolicy("OpenCorsPolicy", opt =>
                    opt.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews()
                .AddXmlDataContractSerializerFormatters();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                .AddJwtBearer("JwtBearer", jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("StkfDL6E8uxnz6VQebwcs4vGqYPORe08mVbaLIPyLQKcoFDQkd3zg61vCxBAMZI")),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Database services
            services.RegisterDependencies();

            services.AddMediatR(typeof(MediatREntryPoint).Assembly);

            // Business services
            services.AddTransient<IStatDevParser, StatDevParser>();

            services.AddSwaggerGen(setup =>
            {
                setup.OperationFilter<AuthorisationOperationFilter>();

                setup.SwaggerDoc(
                    "v0_1",
                    new OpenApiInfo
                    {
                        Title = "FuelPOS Toolkit API",
                        Version = "v0.1"
                    }
                    );
            });

            // gRPC
            services.AddGrpc();

            DapperMappings.Initialise();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseCors("OpenCorsPolicy");
            app.UseStaticFiles();

            app.UseRouting();

            // gRPC
            app.UseGrpcWeb();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v0_1/swagger.json", "FuelPOS Toolkit API v0.1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                // gRPC
                endpoints.MapGrpcService<GreeterService>().EnableGrpcWeb();
                endpoints.MapGrpcService<FpDebugService>().EnableGrpcWeb();
            });
        }
    }
}
