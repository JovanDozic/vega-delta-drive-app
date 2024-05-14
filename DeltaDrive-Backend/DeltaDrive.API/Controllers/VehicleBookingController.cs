using DeltaDrive.API.Hubs;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.DTO.Model;
using DeltaDrive.BL.Contracts.IService;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleBookingController(IVehicleBookingService vehicleBookingService, IHubContext<VehicleLocationHub> hubContext, IRideSimulationService rideSimulationService) : ControllerBase
    {
        private readonly IVehicleBookingService _vehicleBookingService = vehicleBookingService;
        private readonly IHubContext<VehicleLocationHub> _hubContext = hubContext;
        private readonly IRideSimulationService _rideSimulationService = rideSimulationService;

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
                return Unauthorized("Token authentication problem.");
            }

            try
            {
                var result = _vehicleBookingService.GetUsersBooking(id, int.Parse(userId));
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

        [HttpGet("startRideToStartLocation/{id}")]
        [Authorize]
        public IActionResult StartRideToStartLocation(int id)
        {
            _rideSimulationService.SimulateRideToStartLocation(id);
            return Ok();
        }

        [HttpGet("startRideToEndLocation/{id}")]
        [Authorize]
        public IActionResult StartRideToEndLocation(int id)
        {
            _rideSimulationService.SimulateRideToEndLocation(id);
            return Ok();
        }

        [HttpPatch("completeBooking")]
        [Authorize]
        public async Task<IActionResult> CompleteBookingAsync(VehicleBookingDto booking)
        {
            var result = await _vehicleBookingService.CompleteBooking(booking);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<VehicleBookingDto>>> GetHistoryAsync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("Token authentication problem.");
            }

            var bookings = await _vehicleBookingService.GetHistoryAsync(int.Parse(userId));
            return Ok(bookings);
        }

        [HttpPatch("rate")]
        [Authorize]
        public async Task<IActionResult> LeaveARating(VehicleBookingDto bookingDto)
        {
            await _vehicleBookingService.LeaveARating(bookingDto);
            return Ok();
        }
    }
}
