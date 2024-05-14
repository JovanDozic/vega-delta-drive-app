using DeltaDrive.BL.Contracts.DTO.Model;
using DeltaDrive.BL.Contracts.IService;
using GeoCoordinatePortable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeltaDrive.BL.Service
{
    public class RideSimulationService(IServiceScopeFactory serviceScopeFactory, IRideSimulationUpdater rideSimulationUpdater) : BackgroundService, IRideSimulationService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly IRideSimulationUpdater _rideSimulationUpdater = rideSimulationUpdater;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        public async Task SimulateRideToStartLocation(int bookingId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var vehicleBookingService = scope.ServiceProvider.GetRequiredService<IVehicleBookingService>();
            var booking = vehicleBookingService.GetBooking(bookingId).Value;
            var startPoint = new GeoCoordinate(booking.StartLocation.Latitude, booking.StartLocation.Longitude);

            await SimulateRide(bookingId, startPoint, VehicleBookingStatus.DrivingToStartLocation);
        }

        public async Task SimulateRideToEndLocation(int bookingId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var vehicleBookingService = scope.ServiceProvider.GetRequiredService<IVehicleBookingService>();
            var booking = vehicleBookingService.GetBooking(bookingId).Value;
            var endPoint = new GeoCoordinate(booking.EndLocation.Latitude, booking.EndLocation.Longitude);

            await SimulateRide(bookingId, endPoint, VehicleBookingStatus.DrivingToEndLocation, distance => booking.Vehicle.StartPrice + booking.Vehicle.PricePerKm * (distance / 1000));
        }

        private async Task SimulateRide(int bookingId, GeoCoordinate endPoint, VehicleBookingStatus status, Func<double, double>? updatePrice = null)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var vehicleBookingService = scope.ServiceProvider.GetRequiredService<IVehicleBookingService>();
            var booking = vehicleBookingService.GetBooking(bookingId).Value;
            var vehicle = booking.Vehicle;

            var currentPoint = new GeoCoordinate(vehicle.Location.Y, vehicle.Location.X);

            double speed = 60 * 1000 / 3600;
            double distancePerTick = speed * 5;
            double initialDistance = currentPoint.GetDistanceTo(endPoint);
            double currentPrice = 0;

            while (currentPoint.GetDistanceTo(endPoint) > distancePerTick)
            {
                double distanceToTravel = Math.Min(distancePerTick, currentPoint.GetDistanceTo(endPoint));
                double travelFraction = distanceToTravel / currentPoint.GetDistanceTo(endPoint);

                double deltaY = (endPoint.Latitude - currentPoint.Latitude) * travelFraction;
                double deltaX = (endPoint.Longitude - currentPoint.Longitude) * travelFraction;

                if (Math.Abs(deltaY) > Math.Abs(endPoint.Latitude - currentPoint.Latitude))
                {
                    deltaY = endPoint.Latitude - currentPoint.Latitude;
                }
                if (Math.Abs(deltaX) > Math.Abs(endPoint.Longitude - currentPoint.Longitude))
                {
                    deltaX = endPoint.Longitude - currentPoint.Longitude;
                }

                vehicle.Location.Y += deltaY;
                vehicle.Location.X += deltaX;

                currentPoint.Latitude = vehicle.Location.Y;
                currentPoint.Longitude = vehicle.Location.X;

                if (updatePrice != null)
                {
                    double currentDistance = initialDistance - currentPoint.GetDistanceTo(endPoint);
                    currentPrice = updatePrice(currentDistance);
                }

                await _rideSimulationUpdater.UpdateLocationAsync(booking.Id, status, vehicle.Location.X, vehicle.Location.Y, currentPrice);

                await Task.Delay(5000);
            }

            vehicle.Location.Y = endPoint.Latitude;
            vehicle.Location.X = endPoint.Longitude;

            var finalStatus = status == VehicleBookingStatus.DrivingToStartLocation ? VehicleBookingStatus.WaitingForPassenger : VehicleBookingStatus.Completed;
            await _rideSimulationUpdater.UpdateLocationAsync(booking.Id, finalStatus, vehicle.Location.X, vehicle.Location.Y, currentPrice);
        }
    }
}
