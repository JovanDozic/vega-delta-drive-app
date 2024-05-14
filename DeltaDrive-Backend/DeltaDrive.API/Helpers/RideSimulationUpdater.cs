using DeltaDrive.API.Hubs;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using Microsoft.AspNetCore.SignalR;

namespace DeltaDrive.API.Helpers
{
    public class RideSimulationUpdater(IHubContext<VehicleLocationHub> hubContext) : IRideSimulationUpdater
    {
        private readonly IHubContext<VehicleLocationHub> _hubContext = hubContext;

        public async Task UpdateLocationAsync(int bookingId, VehicleBookingStatus status, double latitude, double longitude, double currentPrice)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveLocation", bookingId, status, latitude, longitude, currentPrice);
        }
    }
}
