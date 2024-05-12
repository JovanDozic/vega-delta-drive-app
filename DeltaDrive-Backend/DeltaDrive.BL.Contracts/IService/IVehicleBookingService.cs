using DeltaDrive.BL.Contracts.DTO;
using FluentResults;

namespace DeltaDrive.BL.Contracts.IService
{
    public interface IVehicleBookingService
    {
        public Task<Result<VehicleBookingResponseDto>> SendRequestAsync(VehicleBookingRequestDto request);
    }
}
