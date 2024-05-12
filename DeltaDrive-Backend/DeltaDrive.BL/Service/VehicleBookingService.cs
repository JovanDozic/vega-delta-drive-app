using AutoMapper;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using DeltaDrive.DA.Contracts;
using DeltaDrive.DA.Contracts.Model;
using FluentResults;

namespace DeltaDrive.BL.Service
{
    public class VehicleBookingService(IUnitOfWork unitOfWork, IMapper mapper) : BaseService(unitOfWork, mapper), IVehicleBookingService
    {
        public async Task<Result<VehicleBookingResponseDto>> SendRequestAsync(VehicleBookingRequestDto request)
        {
            // TODO: Simulate acceptance chances

            await Task.Delay(2000); // Used for simulating

            // TODO: Set vehicle as unavailable

            var booking = await _unitOfWork.VehicleBookingRepo().AddAsync(new VehicleBooking()
            {
                UserId = request.UserId,
                VehicleId = request.VehicleId,
                BookingDate = DateTime.Now,
                IsAccepted = true,
                StartLocation = _mapper.Map<LocationDto, Location>(request.StartLocation),
                EndLocation = _mapper.Map<LocationDto, Location>(request.EndLocation),
                Price = 0,
                Rating = 0,
            });

            await _unitOfWork.SaveAsync();

            var response = new VehicleBookingResponseDto
            {
                IsAccepted = true,
                BookingId = booking.Entity.Id,
            };

            return Result.Ok(response);
        }
    }
}
