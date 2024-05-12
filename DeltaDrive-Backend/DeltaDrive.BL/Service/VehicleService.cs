using AutoMapper;
using DeltaDrive.BL.Contracts;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using DeltaDrive.DA.Contracts;
using DeltaDrive.DA.Contracts.Model;
using NetTopologySuite.Geometries;

namespace DeltaDrive.BL.Service
{
    public class VehicleService(IUnitOfWork unitOfWork, IMapper mapper) : BaseService(unitOfWork, mapper), IVehicleService
    {
        public PagedResult<VehicleSearchResponseDto> GetAvailableVehicles(VehicleSearchRequestDto request)
        {
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
                vehicle.DistanceFromPassenger = CalculateDistance(endPoint, new Point(vehicle.Location.X, vehicle.Location.Y));

                vehicle.EstimatedPrice = vehicle.StartPrice + vehicle.PricePerKm * CalculateDistance(startPoint, endPoint);
            }

            return new()
            {
                Results = searchResponse,
                TotalCount = vehicles.Count,
                Page = 1,
                PageSize = vehicles.Count
            };
        }



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
    }
}
