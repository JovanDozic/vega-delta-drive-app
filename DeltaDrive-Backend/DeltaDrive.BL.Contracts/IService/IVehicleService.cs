using DeltaDrive.BL.Contracts.DTO;

namespace DeltaDrive.BL.Contracts.IService
{
    public interface IVehicleService
    {
        public PagedResult<VehicleSearchResponseDto> GetAvailableVehicles(VehicleSearchRequestDto request);
    }
}
