using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Device.GetDevices
{
    public class GetDeviceRequestHandler : IRequestHandler<GetDevicesRequest, GetDeviceResponse>
    {
        private readonly IDataRepository _repository;

        public GetDeviceRequestHandler(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetDeviceResponse> Handle(GetDevicesRequest request, CancellationToken cancellationToken)
        {
            var allDevices = await _repository.GetAll<DeviceDocument>();

            var devices = request.NamedOnly
                ? allDevices.Where(d => !string.IsNullOrEmpty(d.Name)).ToList()
                : allDevices.Where(d => string.IsNullOrEmpty(d.Name)).ToList();

            return new GetDeviceResponse(devices.Select(x => new DeviceItem(x.Id, x.Name)).ToList());
        }
    }
}
