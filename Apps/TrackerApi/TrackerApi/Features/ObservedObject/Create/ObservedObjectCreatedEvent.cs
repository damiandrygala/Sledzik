using MediatR;

namespace TrackerApi.Features.ObservedObject.Create
{
    public record ObservedObjectCreatedEvent(string ObservedObjectId) : INotification;
}
