using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Device.Create
{
    public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand>
    {
        private readonly IDataRepository _repository;
        public CreateDeviceCommandHandler(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(CreateDeviceCommand command, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(nameof(command));
            var devicesFromRepo = await _repository.Get<DeviceDocument>(command.Id);
            if (devicesFromRepo is not null) return Unit.Value;
            await _repository.Create(new DeviceDocument()
            {
                Id = command.Id,
                Name = command.Name,
            });
            return Unit.Value;
        }
    }
}
