using MediatR;
using TsgSystemsToolkit.DataManager.Models;

namespace TsgSystemsToolkit.DataManager.Queries
{
    public record GetStationByIdQuery(string Id) : IRequest<StationDbModel>;
}
