using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Commands;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.Handlers
{
    public class InsertPosDebugProcessHandler : IRequestHandler<InsertPosDebugProcessCommand, DebugProcessModel>
    {
        private readonly IPosDebugData _debugData;

        public InsertPosDebugProcessHandler(IPosDebugData debugData)
        {
            _debugData = debugData;
        }

        public async Task<DebugProcessModel> Handle(InsertPosDebugProcessCommand request, CancellationToken cancellationToken)
        {
            return await _debugData.AddDebugProcess(request.Process);
        }
    }
}
