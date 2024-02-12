using MediatR;
using TrackerApi.Features.ObservedObject.Create.Services;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.ObservedObject.Create
{
    internal sealed class SaveObservedObjectDataEventHandler : INotificationHandler<ObservedObjectCreatedEvent>
    {
        private readonly IDataRepository _repository;
        private readonly IHistoricalViolationService _historicalViolationService;
        public SaveObservedObjectDataEventHandler(IDataRepository repository, IHistoricalViolationService historicalViolationService)
        {
            _repository = repository;
            _historicalViolationService = historicalViolationService;
        }
        public async Task Handle(ObservedObjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var observedObject = await _repository.Get<ObservedObjectDocument>(notification.ObservedObjectId);

            var finish = _historicalViolationService.Finish(observedObject);
            var update = _historicalViolationService.Update(observedObject);
            var create = _historicalViolationService.Create(observedObject);

            await Task.WhenAll(finish, update, create);
        }
    }
}
