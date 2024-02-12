using MediatR;

namespace TrackerApi.Features.Device.Update
{
    public record UpdateDeviceCommand(string Id, string Name) : IRequest;
}