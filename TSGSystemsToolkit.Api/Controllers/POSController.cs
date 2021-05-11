using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using FuelPOS.StatDevParser;
using FuelPOS.StatDevParser.Models;
using MediatR;
using TsgSystemsToolkit.DataManager.Queries;

namespace TsgSystems.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class POSController : ControllerBase
    {
        private readonly IStatDevParser _statdevParser;
        private readonly IPosData _posData;
        private readonly IMediator _mediator;

        public POSController(IStatDevParser statdevParser, IPosData posData, IMediator mediator)
        {
            _statdevParser = statdevParser;
            _posData = posData;
            _mediator = mediator;
        }

        // GET api/<POSController>/5
        [HttpGet("{stationId}")]
        public async Task<List<POSModel>> GetPOSBySiteId(string stationId)
        {
            return await _mediator.Send(new GetPosBySiteIdQuery(stationId));
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
