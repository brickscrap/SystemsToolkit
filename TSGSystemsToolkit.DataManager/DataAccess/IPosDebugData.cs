using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.DataAccess
{
    public interface IPosDebugData
    {
        Task<DebugProcessModel> AddDebugProcess(DebugProcessModel process);
        Task<List<DebugProcessModel>> GetAllDebugProcessesAsync();
        Task<List<DebugProcessParametersModel>> GetAllWithParameters();
    }
}