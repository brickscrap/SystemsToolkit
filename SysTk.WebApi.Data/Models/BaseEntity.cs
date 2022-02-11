using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.WebApi.Data.Models
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModified { get; set; }

        public BaseEntity()
        {
            LastModified = DateTime.Now;
            CreatedDate ??= LastModified;
        }
    }
}
