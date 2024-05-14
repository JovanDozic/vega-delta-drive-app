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
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
                if (_context != null)
                {
                    try
                    {
                        _context?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error disposing context in UnitOfWork.");
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        _context = null;
                    }
                }
            }
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