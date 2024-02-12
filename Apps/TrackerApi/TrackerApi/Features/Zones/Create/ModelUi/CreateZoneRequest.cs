using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Features.Zones.Create.ModelUi
{
    public record CreateZoneRequest(ZoneRequest Zone);
    public record ZoneRequest(string ZoneName, List<Coordinates> Coordinates);
}