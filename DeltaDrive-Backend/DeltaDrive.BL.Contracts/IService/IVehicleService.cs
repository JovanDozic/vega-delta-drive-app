using DeltaDrive.BL.Contracts.DTO;

namespace DeltaDrive.BL.Contracts.IService
{
    public interface IVehicleService
    {
        public PagedResult<VehicleSearchResponseDto> GetAvailableVehicles(VehicleSearchRequestDto request);
        public Task UpdateVehicle(VehicleDto vehicleDto);
        public Task UpdateLocation(VehicleDto vehicleDto);
        public Task<VehicleDto> GetByIdAsync(int id);
    }
}
