using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface IPosDebugData
    {
        Task<DebugProcessDbModel> AddDebugProcess(DebugProcessModel process);
        Task<List<DebugProcessDbModel>> GetAllDebugProcessesAsync();
    }
}