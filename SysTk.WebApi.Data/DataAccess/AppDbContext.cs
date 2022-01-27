using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysTk.WebApi.Data.Models;
using SysTk.WebApi.Data.Models.Auth;

namespace SysTk.WebApi.Data.DataAccess
{
    public class AppDbContext : IdentityDbContext<AppUser, Role, Guid>
    {
        public DbSet<FtpCredentials> FtpCredentials { get; set; }
        public DbSet<Station> Stations { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Station>()
                .HasMany(x => x.FtpCredentials)
                .WithOne(x => x.Station!)
                .HasForeignKey(x => x.StationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FtpCredentials>()
                .HasOne(x => x.Station)
                .WithMany(x => x.FtpCredentials)
                .HasForeignKey(x => x.StationId);
        }
    }
}
