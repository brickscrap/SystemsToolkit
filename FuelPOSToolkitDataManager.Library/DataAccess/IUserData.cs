using FuelPOSToolkitDataManager.Library.Models;
using System.Collections.Generic;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string id);
    }
}