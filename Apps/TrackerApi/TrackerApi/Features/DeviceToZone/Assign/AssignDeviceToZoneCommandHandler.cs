using MediatR;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.DeviceToZone.Assign
{
    public class AssignDeviceToZoneCommandHandler : IRequestHandler<AssignDeviceToZoneCommand>
    {
        private readonly IDataRepository _repository;
        public AssignDeviceToZoneCommandHandler(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(AssignDeviceToZoneCommand request, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var currentAssignation = await _repository.Get<DeviceToZoneDocument>(request.DeviceId);
            if (currentAssignation is null)
            {
                await _repository.Create(new DeviceToZoneDocument()
                {
                    Id = request.DeviceId,
                    ZoneIDs = request.ZoneIds
                });
                return Unit.Value;
            }
            currentAssignation.ZoneIDs = request.ZoneIds;
            await _repository.Update(currentAssignation);
            return Unit.Value;
        }
    }
}
