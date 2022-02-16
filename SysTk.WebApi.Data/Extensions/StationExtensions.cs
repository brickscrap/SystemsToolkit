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
    public static class StationExtensions
    {
        public static bool Exists(this DbSet<Station> station, string ip = default, string id = default) =>
            station.Where(x => x.IP == ip || x.Id == id)
            .Any();

        public static List<FtpCredentials> GetChildren(this AppDbContext context, Station station) =>
            context.Entry(station)
                .Collection(x => x.FtpCredentials)
                .Query().ToList();

        public static Station GetStationWithCredentials(this DbSet<Station> stations, string stationId, string ftpUsername = default, int ftpId = default) =>
            stations.Where(x => x.Id == stationId)
                .Include(x => x.FtpCredentials)
                .Where(x => x.FtpCredentials
                        .Where(x => x.Username.ToUpper() == ftpUsername.ToUpper() || x.Id == ftpId)
                        .Any())
                .FirstOrDefault();
    }
}
