using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface IUserData
    {
        Task AddUser(UserModel user);
        Task<List<UserModel>> GetUserById(string id);
    }
}