using MediatR;
using TrackerApi.Infrastructure.Mongo.Documents;
using TrackerApi.Infrastructure.Mongo;

namespace TrackerApi.Features.Zones.GetZones
{
    public class GetZonesRequestHandler : IRequestHandler<GetZonesRequest, GetZonesResponse>
    {
        private readonly IDataRepository _repository;

        public GetZonesRequestHandler(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetZonesResponse> Handle(GetZonesRequest request, CancellationToken cancellationToken)
        {
            if (request.AssignedToDeviceId is not null)
            {
                var assignedZones = await _repository.Get<DeviceToZoneDocument>(request.AssignedToDeviceId);
                if (assignedZones is null) return new GetZonesResponse(new List<Zone>());
                var zones = (await _repository.GetAll<ZoneDocument>()).Where(z => assignedZones.ZoneIDs.Contains(z.Id)).ToList();
                return new GetZonesResponse(zones.Select(z => new Zone(z.Id, z.Zone.ZoneName, z.Zone.Coordinates)).ToList());
            }
            else
            {
                var zones = (await _repository.GetAll<ZoneDocument>());
                return new GetZonesResponse(zones.Select(z => new Zone(z.Id, z.Zone.ZoneName, z.Zone.Coordinates)).ToList());
            }
        }
    }
}
