using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Device.Delete
{
    public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand>
    {
        private readonly IDataRepository _repository;
        public DeleteDeviceCommandHandler(IDataRepository repository) 
        { 
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var repoDevice = await _repository.Get<DeviceDocument>(request.Id);
            if (repoDevice is null) return Unit.Value;

            await _repository.Delete(repoDevice);
            return Unit.Value;
        }
    }
}
