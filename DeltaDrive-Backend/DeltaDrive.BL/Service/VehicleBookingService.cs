using AutoMapper;
using DeltaDrive.BL.Contracts.IService;
using DeltaDrive.DA.Contracts;

namespace DeltaDrive.BL.Service
{
    public class VehicleBookingService(IUnitOfWork unitOfWork, IMapper mapper) : BaseService(unitOfWork, mapper), IVehicleBookingService
    {
    }
}
