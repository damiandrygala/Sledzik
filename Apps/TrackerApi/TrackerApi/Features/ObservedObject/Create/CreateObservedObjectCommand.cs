using MediatR;

namespace TrackerApi.Features.ObservedObject.Create
{
    public record CreateObservedObjectCommand(string DeviceId, DateTime RecordedAt, float Latitude, float Longitude) : IRequest;
}

