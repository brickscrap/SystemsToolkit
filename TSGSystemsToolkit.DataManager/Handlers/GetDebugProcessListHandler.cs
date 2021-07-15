using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Constants;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using TsgSystemsToolkit.DataManager.Queries;

namespace TsgSystemsToolkit.DataManager.Handlers
{
    public class GetDebugProcessListHandler : IRequestHandler<GetDebugProcessListQuery, List<DebugProcessModel>>
    {
        private readonly IPosDebugData _debugData;

        public GetDebugProcessListHandler(IPosDebugData debugData)
        {
            _debugData = debugData;
        }

        public async Task<List<DebugProcessModel>> Handle(GetDebugProcessListQuery request, CancellationToken cancellationToken)
        {
            return await _debugData.GetAllDebugProcessesAsync();
        }
    }
}
