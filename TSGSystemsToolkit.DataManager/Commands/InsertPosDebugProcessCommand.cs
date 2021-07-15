using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.Commands
{
    public record InsertPosDebugProcessCommand(DebugProcessModel Process) : IRequest<DebugProcessModel>;
}
