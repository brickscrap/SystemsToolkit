using FuelPOSToolkitDataManager.Library.DataAccess;
using FuelPOSToolkitDataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelPOSToolkitApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IStationData _stationData;

        public StationController(IStationData stationData)
        {
            _stationData = stationData;
        }
        // GET: api/<StationController>
        [HttpGet]
        public async Task<List<StationModel>> Get()
        {
            var output = await _stationData.GetAllStations();

            return output;
        }

        // GET api/<StationController>/5
        [HttpGet("{id}")]
        public async Task<StationModel> Get(string id)
        {
            var output = await _stationData.GetStationByID(id);

            return output;
        }

        // POST api/<StationController>
        [HttpPost]
        public async Task Post([FromBody] StationModel station)
        {
            await _stationData.AddStation(station);
        }
    }
}
