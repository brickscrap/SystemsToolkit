using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SysTk.WebApi.Data.DataAccess;

namespace SysTk.WebAPI
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Database=SysTkWebApiDB;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
