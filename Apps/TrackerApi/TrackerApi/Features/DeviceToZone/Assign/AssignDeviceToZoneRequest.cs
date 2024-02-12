namespace TrackerApi.Features.DeviceToZone.Assign
{
    public record AssignDeviceToZoneRequest(string DeviceId, List<string> ZoneIds);
}
