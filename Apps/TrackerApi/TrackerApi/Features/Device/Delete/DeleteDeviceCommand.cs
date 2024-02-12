using MediatR;

namespace TrackerApi.Features.Device.Delete
{
    public record DeleteDeviceCommand(string Id) : IRequest;
}
