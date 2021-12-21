using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.Utils
{
    internal interface ISpreadsheet<IEnumerable>
    {
        public SLDocument Create(IEnumerable data);
    }
}
