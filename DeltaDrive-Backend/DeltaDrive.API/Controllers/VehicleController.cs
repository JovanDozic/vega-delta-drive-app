using DeltaDrive.BL.Contracts;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController(IVehicleService vehicleService) : ControllerBase
    {
        private readonly IVehicleService _vehicleService = vehicleService;

        [HttpPost("getAvailable")]
        [Authorize]
        public PagedResult<VehicleSearchResponseDto> GetAvailableVehicles([FromBody] VehicleSearchRequestDto request)
        {
            return _vehicleService.GetAvailableVehicles(request);
        }
    }
}
