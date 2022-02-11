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
        public DbSet<DebugProcess> DebugProcesses { get; set; }
        public DbSet<DebugParameter> DebugParameters { get; set; }

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

            modelBuilder.Entity<DebugProcess>()
                .HasMany(x => x.Parameters)
                .WithOne(x => x.Process!)
                .HasForeignKey(x => x.DebugProcessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DebugParameter>()
                .HasOne(x => x.Process)
                .WithMany(x => x.Parameters)
                .HasForeignKey(x => x.DebugProcessId);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity && x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .Cast<BaseEntity>())
            {
                entity.LastModified = DateTime.Now;
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) 
        {
            foreach (var entity in ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity && x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .Cast<BaseEntity>())
            {
                entity.LastModified = DateTime.Now;
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
