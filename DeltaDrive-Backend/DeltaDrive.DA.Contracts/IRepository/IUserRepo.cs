using DeltaDrive.DA.Contracts.Model;

namespace DeltaDrive.DA.Contracts.IRepository
{
    public interface IUserRepo : IRepository<User>
    {
        public Task<User?> GetByEmailAsync(string email);
    }
}
