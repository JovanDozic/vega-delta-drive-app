using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.IRepository.Authentication;

namespace DeltaDrive.DA.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveAsync();

        // Repositories:
        public IUserRepo UserRepo();
        public ITokenGeneratorRepo TokenGeneratorRepo();
        public IDriverRepo DriverRepo();
    }
}
