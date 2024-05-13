namespace DeltaDrive.BL.Contracts.IService
{
    public interface IRideSimulationService
    {
        public Task SimulateRideToStartLocation(int bookingId);
        public Task SimulateRideToEndLocation(int bookingId);
    }
}
