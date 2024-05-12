using DeltaDrive.API.Hubs;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using FluentResults;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleBookingController(IVehicleBookingService vehicleBookingService, IHubContext<VehicleLocationHub> hubContext) : ControllerBase
    {
        private readonly IVehicleBookingService _vehicleBookingService = vehicleBookingService;
        private readonly IHubContext<VehicleLocationHub> _hubContext = hubContext;


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

        [HttpGet("startRide/{id}")]
        [Authorize]
        public IActionResult StartRide(int id)
        {
            Task.Run(() => SimulateRide(id, true));
            return Ok();
        }


        private async Task SimulateRide(int bookingId, bool toStartLocation)
        {
            var booking = _vehicleBookingService.GetBooking(bookingId).Value;
            var vehicle = booking.Vehicle;

            // Determine start and end points based on the phase of the journey
            var startPoint = toStartLocation
                ? new GeoCoordinate(vehicle.Location.Y, vehicle.Location.X)
                : new GeoCoordinate(booking.StartLocation.Latitude, booking.StartLocation.Longitude);

            var endPoint = toStartLocation
                ? new GeoCoordinate(booking.StartLocation.Latitude, booking.StartLocation.Longitude)
                : new GeoCoordinate(booking.EndLocation.Latitude, booking.EndLocation.Longitude);

            double speed = 60 * 1000 / 3600; // Convert speed to m/s
            double distancePerTick = speed * 5; // Distance travelled in 5 seconds

            while (startPoint.GetDistanceTo(endPoint) > distancePerTick)
            {
                vehicle.Location.Y = startPoint.Latitude;
                vehicle.Location.X = startPoint.Longitude;

                await _hubContext.Clients.All.SendAsync("ReceiveLocation", bookingId, 0, 0);

                await Task.Delay(5000);
            }

            //vehicle.LocationLatitude = endPoint.Latitude;
            //vehicle.LocationLongitude = endPoint.Longitude;
            //_dbContext.SaveChanges();

            await _hubContext.Clients.All.SendAsync("ReceiveLocation", bookingId, endPoint.Latitude, endPoint.Longitude);
        }




    }
}
