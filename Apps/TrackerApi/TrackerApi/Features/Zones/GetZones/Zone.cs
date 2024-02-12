using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Features.Zones.GetZones
{
    public record Zone(string Id, string ZoneName, List<Coordinates> Coordinates);
}
