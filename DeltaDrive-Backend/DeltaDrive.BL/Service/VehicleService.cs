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
        public PagedResult<VehicleDto> GetAvailableVehicles(VehicleSearchRequestDto request)
        {
            // TODO: Do the math

            var startPoint = new Point(request.StartLocation.Longitude, request.StartLocation.Latitude) { SRID = 4326 };

            var vehicles = _unitOfWork.VehicleRepo().GetAllAsync().Result
                .OrderBy(v => v.Location.Distance(startPoint))
                //.Select(v => v.IsBooked == false || v.IsBooked == null)
                .Take(10)
                .ToList();

            PagedResult<VehicleDto> result = new()
            {
                Results = _mapper.Map<IList<Vehicle>, IList<VehicleDto>>(vehicles),
                TotalCount = vehicles.Count,
                Page = 1,
                PageSize = vehicles.Count
            };

            return result;
        }
    }
}
