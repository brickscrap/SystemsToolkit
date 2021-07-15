using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Commands;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using TsgSystemsToolkit.DataManager.Queries;

namespace TsgSystems.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationData _stationData;
        private readonly IMediator _mediator;

        public StationController(IStationData stationData, IMediator _mediator)
        {
            _stationData = stationData;
            this._mediator = _mediator;
        }
        // GET: api/<StationController>
        [HttpGet]
        public async Task<List<StationDbModel>> Get()
        {
            return await _mediator.Send(new GetStationListQuery());
        }

        // GET api/<StationController>/5
        [HttpGet("{id}")]
        public async Task<StationDbModel> Get(string id)
        {
            return await _mediator.Send(new GetStationByIdQuery(id));
        }

        // POST api/<StationController>
        [HttpPost]
        public async Task<StationDbModel> Post([FromBody] StationModel station)
        {
            return await _mediator.Send(new InsertStationCommand(station));
        }
    }
}
