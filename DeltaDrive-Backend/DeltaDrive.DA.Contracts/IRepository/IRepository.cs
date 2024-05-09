using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeltaDrive.DA.Contracts.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<EntityEntry<TEntity>> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllPagedAsync(int page, int pageSize);
        Task<TEntity> GetByIdAsync(int id);
        TEntity UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<int> GetTotalCount();
    }
}
