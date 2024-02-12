using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Zones.Delete
{
    public class DeleteZoneCommandHandler : IRequestHandler<DeleteZoneCommand>
    {
        private readonly IDataRepository _repository;
        public DeleteZoneCommandHandler(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteZoneCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var zoneToDelete = await _repository.Get<ZoneDocument>(request.Id);
            if (zoneToDelete is null) return Unit.Value;
            await _repository.Delete(zoneToDelete);
            return Unit.Value;
        }
    }
}
