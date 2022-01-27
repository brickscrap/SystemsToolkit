using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysTk.WebApi.Data.Models.Auth
{
    public class Role : IdentityRole<Guid>
    {
        public Role(string name) : base(name) { }
    }
}
