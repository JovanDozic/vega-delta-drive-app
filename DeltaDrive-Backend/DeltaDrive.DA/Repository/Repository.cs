using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeltaDrive.DA.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            return await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<TEntity>().Remove(entity);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllPagedAsync(int page, int pageSize)
        {
            return await _context.Set<TEntity>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("ID does not exist.");
        }

        public async Task<int> GetTotalCount()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public TEntity UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            return entity;
        }
    }
}
