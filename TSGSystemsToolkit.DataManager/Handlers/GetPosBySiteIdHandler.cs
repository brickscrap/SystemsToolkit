using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using TsgSystemsToolkit.DataManager.Queries;

namespace TsgSystemsToolkit.DataManager.Handlers
{
    public class GetPosBySiteIdHandler : IRequestHandler<GetPosBySiteIdQuery, List<POSModel>>
    {
        private readonly IPosData _posData;

        public GetPosBySiteIdHandler(IPosData posData)
        {
            _posData = posData;
        }

        public async Task<List<POSModel>> Handle(GetPosBySiteIdQuery request, CancellationToken cancellationToken)
        {
            return await _posData.GetPOSByStationId(request.SiteId);
        }
    }
}
