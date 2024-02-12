using MediatR;

namespace TrackerApi.Features.Zones.Delete
{
    public record DeleteZoneCommand(string Id) : IRequest;
}
