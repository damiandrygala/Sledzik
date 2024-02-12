using Microsoft.AspNetCore.SignalR;

namespace TrackerApi.SignalR
{
    public class SignalRHub : Hub
    {
        public async Task<TResponse> Publish<TResponse>(TResponse response, CancellationToken cancellation)
        {
            return response;
        }
    }
}