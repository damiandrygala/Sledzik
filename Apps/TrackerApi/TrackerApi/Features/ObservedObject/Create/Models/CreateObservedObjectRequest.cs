namespace TrackerApi.Features.ObservedObject.Create.Models
{
    public record CreateObservedObjectRequest(string DeviceId, DateTime RecordedAt, float Latitude, float Longitude);
}