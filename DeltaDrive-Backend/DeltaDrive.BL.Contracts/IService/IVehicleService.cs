using DeltaDrive.BL.Contracts.DTO;
using FluentResults;

namespace DeltaDrive.BL.Contracts.IService
{
    public interface IVehicleService
    {
        public Result<PagedResult<VehicleSearchResponseDto>> GetAvailableVehicles(VehicleSearchRequestDto request);
        public Task UpdateVehicle(VehicleDto vehicleDto);
        Task<Result> UpdateLocation(VehicleDto vehicleDto);
        public Task<VehicleDto> GetByIdAsync(int id);
    }
}
