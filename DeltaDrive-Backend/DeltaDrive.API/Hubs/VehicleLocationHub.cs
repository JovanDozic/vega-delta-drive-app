using Microsoft.AspNetCore.SignalR;

namespace DeltaDrive.API.Hubs
{
    public class VehicleLocationHub : Hub
    {
        public async Task SendLocation(string vehicleId, double latitude, double longitude)
        {
            await Clients.All.SendAsync("ReceiveLocation", vehicleId, latitude, longitude);
        }
    }
}
