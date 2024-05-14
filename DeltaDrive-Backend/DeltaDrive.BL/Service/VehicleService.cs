using AutoMapper;
using DeltaDrive.BL.Contracts;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.DTO.Model;
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
                vehicle.DistanceFromPassenger = CalculationHelper.CalculateDistance(startPoint, new Point(vehicle.Location.X, vehicle.Location.Y));

                vehicle.EstimatedPrice = vehicle.StartPrice + vehicle.PricePerKm * CalculationHelper.CalculateDistance(startPoint, endPoint);
            }

            return Result.Ok(new PagedResult<VehicleSearchResponseDto>()
            {
                Results = searchResponse,
                TotalCount = vehicles.Count,
                Page = 1,
                PageSize = vehicles.Count
            });
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
