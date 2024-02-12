using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Zones.Create
{
    public class CreateZoneCommandHandler : IRequestHandler<CreateZoneCommand>
    {
        private readonly IDataRepository _repository;
        public CreateZoneCommandHandler(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(CreateZoneCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            await _repository.Create(new ZoneDocument()
            {
                Id = Guid.NewGuid().ToString(),
                Zone = new Zone
                {
                    ZoneName = request.Zone.ZoneName,
                    Coordinates = request.Zone.Coordinates,
                } 
            });
            
            return Unit.Value;
        }
    }
}
