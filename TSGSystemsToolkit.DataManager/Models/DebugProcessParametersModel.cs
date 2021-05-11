using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsgSystemsToolkit.DataManager.Models
{
    public class DebugProcessParametersModel : DebugProcessModel
    {
        public List<DebugParametersModel> Parameters { get; set; } = new();
    }
}
