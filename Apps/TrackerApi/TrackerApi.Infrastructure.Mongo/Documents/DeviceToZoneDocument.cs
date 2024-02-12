namespace TrackerApi.Infrastructure.Mongo.Documents
{
    public class DeviceToZoneDocument : IEntity
    {
        public string Id { get; set; }
        public List<string> ZoneIDs { get; set; }
    }
}
