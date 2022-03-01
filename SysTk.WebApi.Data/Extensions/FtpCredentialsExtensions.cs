using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.WebApi.Data.DataAccess;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebApi.Data.Extensions
{
    public static class FtpCredentialsExtensions
    {
        public static Station GetParent(this AppDbContext context, FtpCredentials credentials) =>
            context.Entry(credentials)
                .Reference(x => x.Station)
                .Query().First();

        public static bool Exists(this DbSet<FtpCredentials> credentials, string username, string stationId) =>
            credentials.Where(x => x.StationId == stationId && x.Username == username)
            .Any();
    }
}
