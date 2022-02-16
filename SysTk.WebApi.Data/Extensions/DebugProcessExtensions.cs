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
    public static class DebugProcessExtensions
    {
        public static DebugProcess GetProcessWithParameters(this DbSet<DebugProcess> process, string processName, string parameterName) =>
            process.Where(x => x.Name.ToUpper() == processName.ToUpper())
                .Include(x => x.Parameters)
                .Where(x => x.Parameters.Where(x => x.Name.ToUpper() == parameterName.ToUpper()).Any())
                .FirstOrDefault();

        public static bool Exists(this DbSet<DebugProcess> process, int id) =>
            process.Where(x => x.Id == id)
                .Select(x => x.Id)
                .Any();

        public static bool Exists(this DbSet<DebugProcess> process, string processName) =>
            process.Where(x => x.Name.ToUpper() == processName.ToUpper())
                .Any();

        public static List<DebugParameter> GetChildren(this AppDbContext context, DebugProcess process) =>
            context.Entry(process)
                .Collection(x => x.Parameters)
                .Query().ToList();
    }
}
