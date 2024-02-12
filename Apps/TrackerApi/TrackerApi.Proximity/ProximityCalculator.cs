using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Proximity
{
    public static class ProximityCalculator
    {
        public static bool Calculate(float latitude, float longitude, Zone zone)
        {
            float currentLatitude, currentLongitude;
            float nextLatitude, nextLongitude;
            bool isInside = false;

            for (int i=0; i < zone.Points.Count; i++)
            {
                currentLatitude = zone.Points[i].Latitude;
                currentLongitude = zone.Points[i].Longitude;

                if (i + 1 != zone.Points.Count)
                {
                    nextLatitude = zone.Points[i + 1].Latitude;
                    nextLongitude = zone.Points[i + 1].Longitude;
                }
                else
                {
                    nextLatitude = zone.Points[0].Latitude;
                    nextLongitude = zone.Points[0].Longitude;
                }

                if ((currentLongitude > longitude) != (nextLongitude > longitude) && latitude < (nextLatitude - currentLatitude) * (longitude - currentLongitude) / (nextLongitude - currentLongitude) + currentLatitude)
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }
    }

    public record Zone(List<Coordinates> Points);
}