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
        private IVehicleRepo _vehicleRepo;
        private IVehicleBookingRepo _vehicleBookingRepo;

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
                try
                {
                    if (_context is null) throw new Exception("Context is null in UnitOfWork.Dispose().");
                    // TODO: check if this works! If something does not update in the database, this could be it.
                    _context?.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error disposing context in UnitOfWork.");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
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
        public IVehicleRepo VehicleRepo()
        {
            return _vehicleRepo ??= new VehicleRepo(_context);
        }
        public IVehicleBookingRepo VehicleBookingRepo()
        {
            return _vehicleBookingRepo ??= new VehicleBookingRepo(_context);
        }

    }
}