using DeltaDrive.DA.Contracts.Shared;
using Microsoft.AspNetCore.SignalR;

namespace DeltaDrive.API.Hubs
{
    public class VehicleLocationHub : Hub
    {
        public async Task SendLocation(string vehicleId, VehicleBookingStatus statusCode, double latitude, double longitude)
        {
            await Clients.All.SendAsync("ReceiveLocation", vehicleId, statusCode, latitude, longitude);
        }
    }
}
