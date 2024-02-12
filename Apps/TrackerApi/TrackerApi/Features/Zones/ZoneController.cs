using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.Features.Zones.Create;
using TrackerApi.Features.Zones.Create.ModelUi;
using TrackerApi.Features.Zones.Delete;
using TrackerApi.Features.Zones.GetZones;

namespace TrackerApi.Features.Zones
{
    [ApiController]
    [Route("[controller]")]
    public class ZoneController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ZoneController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllZonesRequest());
            return Ok(response);
        }

        [HttpGet("spec")]
        public async Task<IActionResult> Get([FromQuery] GetZonesRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateZoneRequest request)
        {
            await _mediator.Send(new CreateZoneCommand(new ZoneCommand(request.Zone.ZoneName, request.Zone.Coordinates))); //temp
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteZoneCommand(id));
            return Ok();
        }
    }
}