using DeltaDrive.DA.Contexts;
using DeltaDrive.DA.Contracts;
using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.IRepository.Authentication;
using DeltaDrive.DA.Repository;
using DeltaDrive.DA.Repository.Authentication;

namespace DeltaDrive.DA
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _context;

        // Repositories:
        private IUserRepo _userRepo;
        private ITokenGeneratorRepo _tokenGeneratorRepo;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            _context = null;
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UOW] Error saving changes to the database: {ex.Message}");
                throw;
            }
        }

        // Repositories:

        public IUserRepo UserRepo()
        {
            return _userRepo ??= new UserRepo(_context);
        }
        public ITokenGeneratorRepo TokenGeneratorRepo()
        {
            return _tokenGeneratorRepo ??= new TokenGeneratorRepo();
        }

    }
}