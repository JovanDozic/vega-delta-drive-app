using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleBookingController(IVehicleBookingService vehicleBookingService) : ControllerBase
    {
        private readonly IVehicleBookingService _vehicleBookingService = vehicleBookingService;

        [HttpPost("sendRequest")]
        [Authorize]
        public async Task<Result<VehicleBookingResponseDto>> SendRequest([FromBody] VehicleBookingRequestDto request)
        {
            return await _vehicleBookingService.SendRequestAsync(request);
        }

        [HttpGet("getBooking/{id}")]
        [Authorize]
        public IActionResult GetBooking(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("This booking does not belong to logged user.");
            }

            try
            {
                var result = _vehicleBookingService.GetBooking(id, int.Parse(userId));
                if (result.IsFailed)
                {
                    return Unauthorized("This booking does not belong to logged user.");
                }
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
