using FuelPOSToolkitDataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }
        public List<UserModel> GetUserById(string id)
        {
            var p = new { Id = id };

            var output = _db.LoadData<UserModel, dynamic>("dbo.spUserLookup", p);

            return output;
        }
    }
}
