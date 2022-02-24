using System;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace TSGSystemsToolkit.CmdLine.Services
{
    public interface IAuthService
    {
        Task<bool> Authenticate(Func<InvocationContext, Task<int>> callback, InvocationContext context, CancellationToken ct = default);
    }
}