namespace TrackerApi.Infrastructure.Mongo.Documents
{
    public class DeviceDocument : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
