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
        public IActionResult GetAvailableVehicles([FromBody] VehicleSearchRequestDto request)
        {
            var result = _vehicleService.GetAvailableVehicles(request);

            if (result.IsFailed)
            {
                return BadRequest(result.Value);
            }
            return Ok(result.Value);
        }

        [HttpPatch("updateLocation")]
        [Authorize]
        public async Task<IActionResult> UpdateLocation([FromBody] VehicleDto vehicleDto)
        {
            var result = await _vehicleService.UpdateLocation(vehicleDto);
            if (result.IsSuccess)
            {
                return BadRequest(new { Error = result?.Errors.FirstOrDefault().Message });
            }
            return Ok();
        }
    }
}
