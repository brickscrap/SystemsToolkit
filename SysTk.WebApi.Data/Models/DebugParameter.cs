using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.WebApi.Data.Models
{
    public class DebugParameter : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        [Required]
        public string Name { get; set; }

        [MaxLength(300)]
        [Required]
        public string Description { get; set; }

        [Required]
        public int DebugProcessId { get; set; }

        public DebugProcess Process { get; set; }
    }
}
