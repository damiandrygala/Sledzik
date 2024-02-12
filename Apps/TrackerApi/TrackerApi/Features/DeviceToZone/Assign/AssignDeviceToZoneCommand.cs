using MediatR;

namespace TrackerApi.Features.DeviceToZone.Assign
{
    public record AssignDeviceToZoneCommand(string DeviceId, List<string> ZoneIds) : IRequest;
}
