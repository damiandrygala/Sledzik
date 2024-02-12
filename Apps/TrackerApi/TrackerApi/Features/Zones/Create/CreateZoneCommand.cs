using MediatR;
using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Features.Zones.Create
{
    public record CreateZoneCommand(ZoneCommand Zone) : IRequest;
    public record ZoneCommand(string ZoneName, List<Coordinates> Coordinates);
}
