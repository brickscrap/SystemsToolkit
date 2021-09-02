using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.DataAccess;

namespace TsgSystemsToolkit.DataManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IStationData, StationData>();
            services.AddScoped<IPosData, PosData>();
            services.AddScoped<IPosDebugData, PosDebugData>();

            return services;
        }
    }
}
