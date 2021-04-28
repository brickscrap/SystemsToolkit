using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToolkitLibrary;
using ToolkitLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TsgSystems.Api.Controllers
{
    [Authorize]
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

        // GET api/<POSController>/5
        [HttpGet("{stationId}")]
        public async Task<List<POSModel>> GetPOSBySiteId(string stationId)
        {
            var output = await _posData.GetPOSByStationId(stationId);

            return output;
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
