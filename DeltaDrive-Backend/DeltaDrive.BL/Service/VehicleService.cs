using AutoMapper;
using DeltaDrive.BL.Contracts;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using DeltaDrive.DA.Contracts;
using DeltaDrive.DA.Contracts.Model;
using FluentResults;
using NetTopologySuite.Geometries;

namespace DeltaDrive.BL.Service
{
    public class VehicleService(IUnitOfWork unitOfWork, IMapper mapper) : BaseService(unitOfWork, mapper), IVehicleService
    {
        public Result<PagedResult<VehicleSearchResponseDto>> GetAvailableVehicles(VehicleSearchRequestDto request)
        {
            // TODO: Refactor this code

            var startPoint = new Point(request.StartLocation.Longitude, request.StartLocation.Latitude)
            { SRID = 4326 };
            var endPoint = new Point(request.EndLocation.Longitude,
                request.EndLocation.Latitude)
            { SRID = 4326 };

            var vehicles = _unitOfWork.VehicleRepo().GetAllAsync().Result
                .Where(v => v.IsBooked is null || v.IsBooked == false)
                .OrderBy(v => v.Location.Distance(startPoint))
                .Take(10)
                .ToList();

            var searchResponse = _mapper.Map<IList<Vehicle>, IList<VehicleSearchResponseDto>>(vehicles);

            var geometryFactory = new GeometryFactory();

            foreach (var vehicle in searchResponse)
            {
                vehicle.DistanceFromPassenger = CalculateDistance(startPoint, new Point(vehicle.Location.X, vehicle.Location.Y));

                vehicle.EstimatedPrice = vehicle.StartPrice + vehicle.PricePerKm * CalculateDistance(startPoint, endPoint);
            }

            return Result.Ok(new PagedResult<VehicleSearchResponseDto>()
            {
                Results = searchResponse,
                TotalCount = vehicles.Count,
                Page = 1,
                PageSize = vehicles.Count
            });
        }


        // TODO: Move this in helper class
        public static double CalculateDistance(Point coord1, Point coord2)
        {
            var R = 6371; // Radius of the Earth in kilometers
            var dLat = ToRadians(coord2.Y - coord1.Y);
            var dLon = ToRadians(coord2.X - coord1.X);
            var lat1 = ToRadians(coord1.Y);
            var lat2 = ToRadians(coord2.X);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public async Task UpdateVehicle(VehicleDto vehicleDto)
        {
            try
            {
                var vehicle = _mapper.Map<VehicleDto, Vehicle>(vehicleDto);

                _unitOfWork.VehicleRepo().Update(vehicle);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        public async Task<VehicleDto> GetByIdAsync(int id)
        {
            var vehicle = await _unitOfWork.VehicleRepo().GetByIdAsync(id);

            return _mapper.Map<Vehicle, VehicleDto>(vehicle);
        }

        public async Task<Result> UpdateLocation(VehicleDto vehicleDto)
        {
            try
            {
                var vehicle = _mapper.Map<VehicleDto, Vehicle>(vehicleDto);

                _unitOfWork.VehicleRepo().Update(vehicle);

                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return Result.Fail("Failed to update vehicle location.");
            }

            return Result.Ok();
        }
    }
}
