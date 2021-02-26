using FuelPOSToolkitDataManager.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuelPOSToolkitDataManager.Library.DataAccess
{
    public interface IUserData
    {
        Task AddUser(UserModel user);
        Task<List<UserModel>> GetUserById(string id);
    }
}