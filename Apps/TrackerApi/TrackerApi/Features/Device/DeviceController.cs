using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.Features.Device.Create;
using TrackerApi.Features.Device.Create.ModelUi;
using TrackerApi.Features.Device.Delete;
using TrackerApi.Features.Device.GetDevices;
using TrackerApi.Features.Device.Update;

namespace TrackerApi.Features.Device
{
    [ApiController]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetDevicesRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request)
        {
            await _mediator.Send(new CreateDeviceCommand(request.Id, request.Name));
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDeviceRequest request)
        {
            await _mediator.Send(new UpdateDeviceCommand(request.Id, request.Name));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteDeviceCommand(id));
            return Ok();
        }
    }
}
