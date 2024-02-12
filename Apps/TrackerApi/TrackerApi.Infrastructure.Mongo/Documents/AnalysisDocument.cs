using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Infrastructure.Mongo.Documents
{
    public class AnalysisDocument : IEntity
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public DateTime StartViolationTs { get; set; }
        public DateTime EndViolationTs { get; set; }
        public List<TimeCoordsMap> RecordedPositionMap { get; set; }
        public string ViolatedZoneId { get; set; }
    }
}
