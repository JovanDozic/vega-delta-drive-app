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

        [HttpGet("startRideToStartLocation/{id}")]
        [Authorize]
        public IActionResult StartRideToStartLocation(int id)
        {
            var booking = _vehicleBookingService.GetBooking(id).Value;
            Task.Run(() => SimulateRideToStartLocation(booking));
            return Ok();
        }

        [HttpGet("startRideToEndLocation/{id}")]
        [Authorize]
        public IActionResult StartRideToEndLocation(int id)
        {
            var booking = _vehicleBookingService.GetBooking(id).Value;
            Task.Run(() => SimulateRideToEndLocation(booking));
            return Ok();
        }


        private async Task SimulateRideToStartLocation(VehicleBookingDto booking)
        {
            /* 
             * TODO: Write down how this method works, and how is location updated on the frontend.
             * 
             * TODO: Move this logic into a service, e.g. RideSimulationService.
             * 
             * TODO: Add actual calculations for the vehicle's location.
             * 
             * TODO: Make sure code is refactored and as clean as possible.
             */

            var vehicle = booking.Vehicle;

            var currentPoint = new GeoCoordinate(vehicle.Location.Y, vehicle.Location.X);
            var endPoint = new GeoCoordinate(booking.StartLocation.Latitude, booking.StartLocation.Longitude);

            //double speed = 60 * 1000 / 3600;
            //double distancePerTick = speed * 5;

            double thresholdDistance = 10.0;

            while (currentPoint.GetDistanceTo(endPoint) > thresholdDistance)
            {
                vehicle.Location.Y += 0.1;
                vehicle.Location.X += 0.1;

                currentPoint.Latitude = vehicle.Location.Y;
                currentPoint.Longitude = vehicle.Location.X;

                await _hubContext.Clients.All.SendAsync("ReceiveLocation", booking.Id, VehicleBookingStatus.DrivingToStartLocation, vehicle.Location.X, vehicle.Location.Y);

                await Task.Delay(1000); // TODO: Change to 5000
            }

            //vehicle.LocationLatitude = endPoint.Latitude;
            //vehicle.LocationLongitude = endPoint.Longitude;
            //_dbContext.SaveChanges();

            await _hubContext.Clients.All.SendAsync("ReceiveLocation", booking.Id, VehicleBookingStatus.WaitingForPassenger, endPoint.Latitude, endPoint.Longitude);
        }

        private async Task SimulateRideToEndLocation(VehicleBookingDto booking)
        {
            var vehicle = booking.Vehicle;

            var currentPoint = new GeoCoordinate(vehicle.Location.Y, vehicle.Location.X);
            var endPoint = new GeoCoordinate(booking.EndLocation.Latitude, booking.EndLocation.Longitude);

            double thresholdDistance = 10.0;

            while (currentPoint.GetDistanceTo(endPoint) > thresholdDistance)
            {
                vehicle.Location.Y += 0.1;
                vehicle.Location.X += 0.1;

                currentPoint.Latitude = vehicle.Location.Y;
                currentPoint.Longitude = vehicle.Location.X;

                await _hubContext.Clients.All.SendAsync("ReceiveLocation", booking.Id, VehicleBookingStatus.DrivingToEndLocation, vehicle.Location.X, vehicle.Location.Y);

                await Task.Delay(1000); // TODO: Change to 5000
            }

            await _hubContext.Clients.All.SendAsync("ReceiveLocation", booking.Id, VehicleBookingStatus.Completed, endPoint.Latitude, endPoint.Longitude);
        }
    }
}
