using DeltaDrive.DA.Contexts;
using DeltaDrive.DA.Contracts.IRepository;
using DeltaDrive.DA.Contracts.Model;
using Microsoft.EntityFrameworkCore;

namespace DeltaDrive.DA.Repository
{
    public class VehicleBookingRepo(DbContext context) : Repository<VehicleBooking>(context), IVehicleBookingRepo
    {
        public DataContext Context
        {
            get { return _context as DataContext; }
        }

        public VehicleBooking? GetById(int id)
        {
            return _context.Set<VehicleBooking>()
                           .Include(booking => booking.User)
                           .Include(booking => booking.Vehicle)
                           .FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<VehicleBooking>> GetByUserId(int userId)
        {
            return _context.Set<VehicleBooking>()
                           .Include(booking => booking.User)
                           .Include(booking => booking.Vehicle)
                           .Where(x => x.UserId == userId)
                           .ToList();
        }
    }
}
