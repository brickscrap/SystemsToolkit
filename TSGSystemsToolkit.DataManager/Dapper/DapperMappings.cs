using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.Dapper
{
    public static class DapperMappings
    {
        public static void Initialise()
        {
            SqlMapper.SetTypeMap(
                typeof(DebugParametersModel),
                new ColumnAttributeTypeMapper<DebugParametersModel>());
        }
    }
}
