using MediatR;

namespace TrackerApi.Features.Zones.GetZones
{
    public record GetAllZonesRequest() : IRequest<GetAllZonesResponse>;
    public record GetAllZonesResponse(List<Zone> Zones);
}
