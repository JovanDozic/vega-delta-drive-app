using DeltaDrive.DA.Contexts;
using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.DA.Repository
{
    public class DriverRepo(DbContext context) : Repository<Driver>(context), IDriverRepo
    {
        public DataContext Context
        {
            get { return _context as DataContext; }
        }
    }
}
