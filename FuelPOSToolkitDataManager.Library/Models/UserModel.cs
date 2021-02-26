using System;
using System.Collections.Generic;
using System.Text;

namespace FuelPOSToolkitDataManager.Library.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Roles { get; set; }
    }
}
