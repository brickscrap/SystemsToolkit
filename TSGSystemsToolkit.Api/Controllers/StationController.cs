using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using TsgSystemsToolkit.DataManager.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<List<StationModel>> Get()
        {
            return await _mediator.Send(new GetStationListQuery());
        }

        // GET api/<StationController>/5
        [HttpGet("{id}")]
        public async Task<StationModel> Get(string id)
        {
            return await _stationData.GetStationByID(id);
        }

        // POST api/<StationController>
        [HttpPost]
        public async Task Post([FromBody] StationModel station)
        {
            await _stationData.AddStation(station);
        }
    }
}
