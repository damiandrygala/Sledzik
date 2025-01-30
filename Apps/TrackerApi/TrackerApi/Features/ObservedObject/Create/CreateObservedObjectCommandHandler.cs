using MediatR;
using Microsoft.AspNetCore.SignalR;
using TrackerApi.Features.ObservedObject.Create;
using TrackerApi.Features.ObservedObject.Create.Models;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;
using TrackerApi.Infrastructure.Mongo.Models;
using TrackerApi.Proximity;
using TrackerApi.SignalR;

namespace TrackerApi.Features.GpsData.Create
{
    public class CreateObservedObjectCommandHandler : IRequestHandler<CreateObservedObjectCommand>
    {
        private readonly IDataRepository _repository;
        private readonly IHubContext<CoordinatesObservedObjectHub> _signalRContext;
        private readonly IPublisher _publisher;
        public CreateObservedObjectCommandHandler(IDataRepository repository, IHubContext<CoordinatesObservedObjectHub> singlarContext, IPublisher publisher)
        {
            _repository = repository;
            _signalRContext = singlarContext;
            //_publisher = publisher;
        }

        public async Task<Unit> Handle(CreateObservedObjectCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));

            var assignedZones = await _repository.Get<DeviceToZoneDocument>(request.DeviceId);
            if (assignedZones is null) return Unit.Value;
            var violatedZoneIds = new List<string>();

            if (assignedZones is not null)
            {
                var zones = (await _repository.GetAll<ZoneDocument>()).Where(d => assignedZones.ZoneIDs.Contains(d.Id)).ToList();
                foreach (var zone in zones)
                {
                    var isViolated = ProximityCalculator.Calculate(request.Latitude, request.Longitude, zone.Zone.ToProximityZone());
                    if (isViolated) violatedZoneIds.Add(zone.Id);
                }
            }

            var observedObject = new ObservedObjectDocument
                {
                Id = Guid.NewGuid().ToString(),
                DeviceId = request.DeviceId,
                RecordedAt = request.RecordedAt,
                Coordinates = new Coordinates(request.Latitude, request.Longitude),
                ViolatedZoneIds = violatedZoneIds
            };

            await _repository.Create(observedObject);
            await _signalRContext.Clients.All.SendAsync("CoordinatesObservedObjectReceived", observedObject);
            //await _publisher.Publish(observedObject.Id);

            return Unit.Value;
        }
    }
}
