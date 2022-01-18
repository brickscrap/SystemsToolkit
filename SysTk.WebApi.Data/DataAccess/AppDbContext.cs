using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.WebApi.Data.Models;

namespace SysTk.WebApi.Data.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<FtpCredentials> FtpCredentials { get; set; }
        public DbSet<Station> Stations { get; set; }
    }
}
