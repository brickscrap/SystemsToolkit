using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    internal interface IHandler<T> where T : OptionsBase
    {
        Task<int> RunHandlerAndReturnExitCode(T options, CancellationToken ct = default(CancellationToken));
    }
}
