using DeltaDrive.DA.Contracts.Model;

namespace DeltaDrive.DA.Contracts.IRepository
{
    public interface IVehicleRepo : IRepository<Vehicle>
    {
        public IList<Vehicle> GetAvailableVehicles();
    }
}
