using DeltaDrive.DA.Contexts;
using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.DA.Repository
{
    public class UserRepo(DbContext context) : Repository<User>(context), IUserRepo
    {
        public DataContext Context
        {
            get { return _context as DataContext; }
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return _context.Set<User>()
                           .FirstOrDefaultAsync(u => u.Email == email)
                           ?? throw new Exception("User not found.");
        }
    }
}
