using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsgSystemsToolkit.DataManager.Models
{
    public class StationDbModel
    {
        public string Id { get; set; }
        public string KimoceId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public int NumberOfPOS { get; set; }
    }
}
