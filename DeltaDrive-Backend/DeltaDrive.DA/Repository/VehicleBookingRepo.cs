using DeltaDrive.DA.Contexts;
using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.DA.Repository
{
    internal class VehicleBookingRepo(DbContext context) : Repository<VehicleBooking>(context), IVehicleBookingRepo
    {
        public DataContext Context
        {
            get { return _context as DataContext; }
        }
    }
}
