using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TsgSystemsToolkit.DataManager.Commands;
using TsgSystemsToolkit.DataManager.DataAccess;
using TsgSystemsToolkit.DataManager.Models;
using TsgSystemsToolkit.DataManager.Queries;

namespace FuelPOSToolkitApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PosDebugController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPosDebugData _debug;

        public PosDebugController(IMediator mediator, IPosDebugData debug)
        {
            _mediator = mediator;
            _debug = debug;
        }

        [HttpGet]
        public async Task<List<DebugProcessModel>> GetAll()
        {
            return await _mediator.Send(new GetDebugProcessListQuery());
        }

        [HttpPost]
        public async Task<DebugProcessModel> Post([FromBody] DebugProcessModel process)
        {
            return await _mediator.Send(new InsertPosDebugProcessCommand(process));
        }

        [HttpGet]
        [Route("GetWithParams")]
        public async Task<List<DebugProcessParametersModel>> GetAllWithParams()
        {
            return await _debug.GetAllWithParameters();
        }
    }
}
