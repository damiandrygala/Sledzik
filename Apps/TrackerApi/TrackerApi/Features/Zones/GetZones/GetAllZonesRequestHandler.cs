using MediatR;
using TrackerApi.Infrastructure.Mongo.Documents;
using TrackerApi.Infrastructure.Mongo;

namespace TrackerApi.Features.Zones.GetZones
{
    public class GetAllZonesRequestHandler : IRequestHandler<GetAllZonesRequest, GetAllZonesResponse>
    {
        private readonly IDataRepository _repository;

        public GetAllZonesRequestHandler(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetAllZonesResponse> Handle(GetAllZonesRequest request, CancellationToken cancellationToken)
        {
            var zones = await _repository.GetAll<ZoneDocument>();
            return new GetAllZonesResponse(zones.Select(z => new Zone(z.Id, z.Zone.ZoneName, z.Zone.Coordinates)).ToList());
        }
    }
}
