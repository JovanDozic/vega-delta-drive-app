using DeltaDrive.BL.Contracts.DTO;

namespace DeltaDrive.BL.Contracts.IService
{
    public interface IRideSimulationUpdater
    {
        Task UpdateLocationAsync(int bookingId, VehicleBookingStatus status, double latitude, double longitude);
    }
}
