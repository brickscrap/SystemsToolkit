using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.Queries
{
    public record GetStationListQuery() : IRequest<List<StationModel>>;
}
