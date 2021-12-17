using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Handlers
{
    public abstract class AbstractHandler<T> : IHandler<T> where T : OptionsBase
    {
        public AbstractHandler()
        {

        }
        public abstract int RunHandlerAndReturnExitCode(T options);
    }
}
