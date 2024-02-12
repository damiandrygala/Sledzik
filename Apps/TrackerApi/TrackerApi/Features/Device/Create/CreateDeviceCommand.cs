using MediatR;

namespace TrackerApi.Features.Device.Create
{
    public record CreateDeviceCommand (string Id, string Name) : IRequest;
}
