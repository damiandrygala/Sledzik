using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.Features.DeviceToZone.Assign;

namespace TrackerApi.Features.DeviceToZone
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceToZoneController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeviceToZoneController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssignDeviceToZoneRequest request)
        {
            await _mediator.Send(new AssignDeviceToZoneCommand(request.DeviceId, request.ZoneIds));
            return Ok();
        }
    }
}
