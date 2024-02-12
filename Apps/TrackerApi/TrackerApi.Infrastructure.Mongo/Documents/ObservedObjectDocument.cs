using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Infrastructure.Mongo.Documents
{
    public class ObservedObjectDocument : IEntity
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime RecordedAt { get; set; }
        public Coordinates Coordinates { get; set; }
        public List<string> ViolatedZoneIds { get; set; }
    }
}
