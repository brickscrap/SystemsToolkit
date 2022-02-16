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
    public static class DebugParameterExtensions
    {
        public static bool Exists(this DbSet<DebugParameter> parameter, int processId, string paramName) =>
            parameter.Where(x => x.DebugProcessId == processId && x.Name.ToUpper() == paramName.ToUpper())
                .Any();

        public static DebugProcess GetParent(this AppDbContext context, DebugParameter debugParameter) =>
            context.Entry(debugParameter)
                .Reference(x => x.Process)
                .Query().FirstOrDefault();
    }
}
