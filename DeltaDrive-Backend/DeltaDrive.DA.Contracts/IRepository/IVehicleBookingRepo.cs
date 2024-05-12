using DeltaDrive.DA.Contracts.Model;

namespace DeltaDrive.DA.Contracts.IRepository
{
    public interface IVehicleBookingRepo : IRepository<VehicleBooking>
    {
        public VehicleBooking GetById(int id);
    }
}
