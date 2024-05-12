using DeltaDrive.BL.Contracts.DTO;
using FluentResults;

namespace DeltaDrive.BL.Contracts.IService
{
    public interface IVehicleBookingService
    {
        public Task<Result<VehicleBookingResponseDto>> SendRequestAsync(VehicleBookingRequestDto request);
        public Result<VehicleBookingDto> GetUsersBooking(int id, int userId);
        public Result<VehicleBookingDto> GetBooking(int id);
    }
}
