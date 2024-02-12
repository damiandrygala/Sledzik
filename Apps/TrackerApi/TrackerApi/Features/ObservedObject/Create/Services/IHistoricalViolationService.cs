using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.ObservedObject.Create.Services
{
    public interface IHistoricalViolationService
    {
        Task<Task> Create(ObservedObjectDocument observedObject);
        Task<Task> Update(ObservedObjectDocument observedObject);
        Task<Task> Finish(ObservedObjectDocument observedObject);
    }
}
