using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine.Services
{
    public interface IAuthService
    {
        Task<bool> Authenticate();
    }
}