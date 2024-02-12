using Microsoft.AspNetCore.SignalR;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.SignalR
{

    public class CoordinatesObservedObjectHub : Hub
    {
        public async Task CoordinatesObservedObjectReceived
        (ObservedObjectDocument observedObject)
        {
            await Clients.All.SendAsync("CoordinatesObservedObjectReceived", observedObject);
        }
    }
}
