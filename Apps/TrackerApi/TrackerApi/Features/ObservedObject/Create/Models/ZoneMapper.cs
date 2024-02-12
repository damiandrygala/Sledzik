using TrackerApi.Infrastructure.Mongo.Documents;
using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Features.ObservedObject.Create.Models
{
    public static class ZoneMapper
    {
        public static Proximity.Zone ToProximityZone(this Zone observedZone)
        {
            var zoneCoords = new List<Coordinates>();

            foreach (var item in observedZone.Coordinates)
            {
                zoneCoords.Add(new Coordinates(item.Latitude, item.Longitude));
            }

            return new Proximity.Zone(zoneCoords);
        }
    }
}
