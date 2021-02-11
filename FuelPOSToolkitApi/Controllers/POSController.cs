using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using FuelPOSToolkitDataManager.Library.DataAccess;
using ToolkitLibrary;
using ToolkitLibrary.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FuelPOSToolkitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSController : ControllerBase
    {
        private readonly IStatdevParser _statdevParser;
        private readonly IPosData _posData;

        public POSController(IStatdevParser statdevParser, IPosData posData)
        {
            _statdevParser = statdevParser;
            _posData = posData;
        }

        // GET: api/<POSController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<POSController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<POSController>
        [HttpPost("{stationId}")]
        [Consumes("application/xml")]
        public async Task Post(string stationId, [FromBody] XElement statDevXML)
        {
            var xdoc = new XDocument(statDevXML);

            StatdevModel posInfo = new StatdevModel();
            posInfo = _statdevParser.Parse(xdoc);

            await _posData.AddPOSData(stationId, posInfo.POS);
        }
    }
}
