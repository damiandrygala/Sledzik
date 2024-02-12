using MediatR;

namespace TrackerApi.Features.Device.GetDevices
{
    public record GetDevicesRequest (bool NamedOnly) : IRequest<GetDeviceResponse>;
    public record GetDeviceResponse (List<DeviceItem> DeviceItems);
    public record DeviceItem (string Id, string Name);
}
