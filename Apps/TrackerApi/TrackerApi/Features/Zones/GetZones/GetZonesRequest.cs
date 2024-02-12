using MediatR;

namespace TrackerApi.Features.Zones.GetZones
{
    public record GetZonesRequest(string AssignedToDeviceId) : IRequest<GetZonesResponse>;
    public record GetZonesResponse(List<Zone> Zones);
}
