using DeltaDrive.BL.Contracts.DTO;
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

        // TODO: Refactor this and the next method as they are mostly the same except current and end points.
        public async Task SimulateRideToStartLocation(int bookingId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var vehicleBookingService = scope.ServiceProvider.GetRequiredService<IVehicleBookingService>();
            //var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();
            var booking = vehicleBookingService.GetBooking(bookingId).Value;
            var vehicle = booking.Vehicle;

            var currentPoint = new GeoCoordinate(vehicle.Location.Y, vehicle.Location.X);
            var endPoint = new GeoCoordinate(booking.StartLocation.Latitude, booking.StartLocation.Longitude);

            double speed = 60 * 1000 / 3600;
            double distancePerTick = speed * 5;

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

                // Workaround: we will update vehicle location from the frontend only after every ride status phase (so after driving to start, and driving to end).
                // Why: In our case, RideSimulationService (injected as HostedService - a singleton) depends on IRideSimulationUpdater (injected as scoped service, as it should be -  it uses DbContext). This creates a problem because scoped service is disposed after the request ends, but singleton is not and it still tries to hold it's reference.
                // Even tho we already solved the problem of getting booking info with ServiceProvider, we can not update any entity as it's being tracked by another DbContext (the original one).
                //await vehicleService.UpdateVehicle(vehicle);

                await _rideSimulationUpdater.UpdateLocationAsync(booking.Id, VehicleBookingStatus.DrivingToStartLocation, vehicle.Location.X, vehicle.Location.Y, 0);

                await Task.Delay(1000); // TODO: Change to 5000
            }

            vehicle.Location.Y = booking.StartLocation.Latitude;
            vehicle.Location.X = booking.StartLocation.Longitude;

            await _rideSimulationUpdater.UpdateLocationAsync(booking.Id, VehicleBookingStatus.WaitingForPassenger, vehicle.Location.X, vehicle.Location.Y, 0);

        }

        public async Task SimulateRideToEndLocation(int bookingId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var vehicleBookingService = scope.ServiceProvider.GetRequiredService<IVehicleBookingService>();
            var booking = vehicleBookingService.GetBooking(bookingId).Value;
            var vehicle = booking.Vehicle;

            var currentPoint = new GeoCoordinate(vehicle.Location.Y, vehicle.Location.X);
            var endPoint = new GeoCoordinate(booking.EndLocation.Latitude, booking.EndLocation.Longitude);

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

                double currentDistance = initialDistance - currentPoint.GetDistanceTo(endPoint);
                currentPrice = vehicle.StartPrice + vehicle.PricePerKm * (currentDistance / 1000);

                // TODO: Fix this workaround
                // Workaround: we will update vehicle location from the frontend only after every ride status phase (so after driving to start, and driving to end).
                // Why: RideSimulationService is an background service injected as HostedService - basically a singleton, so it can not use same DbContext as the rest of the app.
                // Even tho we already solved the problem of getting booking info with ServiceProvider, we can not update any entity as it's being tracked by another DbContext (the original one).
                //await vehicleService.UpdateVehicle(vehicle);

                await _rideSimulationUpdater.UpdateLocationAsync(booking.Id, VehicleBookingStatus.DrivingToStartLocation, vehicle.Location.X, vehicle.Location.Y, currentPrice);

                await Task.Delay(1000); // TODO: Change to 5000
            }

            vehicle.Location.Y = booking.EndLocation.Latitude;
            vehicle.Location.X = booking.EndLocation.Longitude;

            await _rideSimulationUpdater.UpdateLocationAsync(booking.Id, VehicleBookingStatus.Completed, vehicle.Location.X, vehicle.Location.Y, currentPrice);
        }
    }
}
