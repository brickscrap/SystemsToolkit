using System.ComponentModel.DataAnnotations.Schema;

namespace TsgSystemsToolkit.DataManager.Models
{
    public class DebugParametersModel
    {
        [Column(name:"ParameterId")]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Parameter { get; set; }
    }
}
