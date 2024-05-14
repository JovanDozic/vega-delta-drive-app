using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.IRepository.Authentication;

namespace DeltaDrive.DA.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveAsync();

        public IUserRepo UserRepo();
        public ITokenGeneratorRepo TokenGeneratorRepo();
        public IVehicleRepo VehicleRepo();
        public IVehicleBookingRepo VehicleBookingRepo();
    }
}
