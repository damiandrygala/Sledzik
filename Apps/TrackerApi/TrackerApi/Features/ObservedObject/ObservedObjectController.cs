using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.Features.ObservedObject.Create;
using TrackerApi.Features.ObservedObject.Create.Models;

namespace TrackerApi.Features.GpsData
{
    [ApiController]
    [Route("[controller]")]
    public class ObservedObjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ObservedObjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateObservedObjectRequest request)
        {
            await _mediator.Send(new CreateObservedObjectCommand(request.DeviceId, request.RecordedAt, request.Latitude, request.Longitude));
            return Ok();
        }
    }
}