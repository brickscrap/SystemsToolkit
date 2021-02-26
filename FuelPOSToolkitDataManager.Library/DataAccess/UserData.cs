using FuelPOSToolkitDataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }
        public async Task<List<UserModel>> GetUserById(string id)
        {
            var p = new { Id = id };

            var output = await _db.LoadDataAsync<UserModel, dynamic>("dbo.spUserLookup", p);

            return output;
        }

        public async Task AddUser(UserModel user)
        {
            await _db.SaveDataAsync("dbo.spInsertUser", user);
        }
    }
}
