using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Device.Update
{
    public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand>
    {
        private readonly IDataRepository _repository;
        public UpdateDeviceCommandHandler(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var deviceDocument = await _repository.Get<DeviceDocument>(request.Id);
            ArgumentNullException.ThrowIfNull(nameof(deviceDocument));
            deviceDocument.Name = request.Name;
            await _repository.Update(deviceDocument);
            return Unit.Value;
        }
    }
}
