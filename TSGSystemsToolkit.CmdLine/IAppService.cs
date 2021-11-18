using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine
{
    internal interface IAppService
    {
        Task<int> Run(string[] args);
    }
}