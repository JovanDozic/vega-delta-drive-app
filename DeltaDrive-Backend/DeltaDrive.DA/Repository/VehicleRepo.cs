using DeltaDrive.DA.Contexts;
using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.DA.Repository
{
    public class VehicleRepo(DbContext context) : Repository<Vehicle>(context), IVehicleRepo
    {
        //private readonly GeometryFactory _geometryFactory;

        public DataContext Context
        {
            get { return _context as DataContext; }
        }

        public IList<Vehicle> GetAvailableVehicles()
        {
            return [.. _context.Set<Vehicle>()
                               .OrderBy(x => x.Id)
                               .Take(10)];
        }
    }
}
