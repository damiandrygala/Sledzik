using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Infrastructure.Mongo.Documents
{
    public class ZoneDocument : IEntity
    {
        public string Id { get; set; }
        public Zone Zone { get; set; }
    }

    public class Zone
    {
        public string ZoneName { get; set; }
        public List<Coordinates> Coordinates { get; set; }
    }
}
