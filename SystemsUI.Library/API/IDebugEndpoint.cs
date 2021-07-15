using System.Collections.Generic;
using System.Threading.Tasks;
using SystemsUI.Library.Models;

namespace SystemsUI.Library.API
{
    public interface IDebugEndpoint
    {
        Task<List<DebugProcessModel>> GetAll();
    }
}