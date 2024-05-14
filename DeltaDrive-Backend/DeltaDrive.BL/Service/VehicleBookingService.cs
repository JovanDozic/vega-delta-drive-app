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
            var bookings = await _unitOfWork.VehicleBookingRepo().GetByUserId(request.UserId);
            if (bookings.Any(x => x.Status != DA.Contracts.Shared.VehicleBookingStatus.Completed))
            {
                return Result.Fail<VehicleBookingResponseDto>(FailureCode.UserAlreadyHasAnActiveBooking);
            }

            await Task.Delay(2000); // Used for simulating

            bool isAccepted = SimulateAcceptance();

            if (!isAccepted)
            {
                return Result.Ok(new VehicleBookingResponseDto()
                {
                    IsAccepted = false,
                    BookingId = -1,
                });
            }

            // TODO: Set Users location to start location

            var vehicle = await _unitOfWork.VehicleRepo().GetByIdAsync(request.VehicleId);
            vehicle.IsBooked = true;
            _unitOfWork.VehicleRepo().Update(vehicle);

            var booking = await _unitOfWork.VehicleBookingRepo().AddAsync(new VehicleBooking()
            {
                UserId = request.UserId,
                VehicleId = request.VehicleId,
                BookingDate = DateTime.Now,
                StartLocation = _mapper.Map<LocationDto, Location>(request.StartLocation),
                EndLocation = _mapper.Map<LocationDto, Location>(request.EndLocation),
                Price = 0,
                RatingId = null,
                Rating = null,
            });

            await _unitOfWork.SaveAsync();

            var response = new VehicleBookingResponseDto
            {
                IsAccepted = true,
                BookingId = booking.Entity.Id,
            };

            return Result.Ok(response);
        }

        public Result<VehicleBookingDto> GetUsersBooking(int id, int userId)
        {
            var booking = _unitOfWork.VehicleBookingRepo().GetById(id);

            if (booking == null)
            {
                return Result.Fail<VehicleBookingDto>(FailureCode.NotFound);
            }

            if (booking.UserId != userId)
            {
                return Result.Fail<VehicleBookingDto>(FailureCode.Forbidden);
            }

            var response = _mapper.Map<VehicleBooking, VehicleBookingDto>(booking);

            return Result.Ok(response);
        }

        public static bool SimulateAcceptance()
        {
            return true; // TODO: Uncomment this: new Random().Next(100) >= 25;
        }

        public Result<VehicleBookingDto> GetBooking(int id)
        {
            var booking = _unitOfWork.VehicleBookingRepo().GetById(id);

            if (booking == null)
            {
                return Result.Fail<VehicleBookingDto>(FailureCode.NotFound);
            }

            var response = _mapper.Map<VehicleBooking, VehicleBookingDto>(booking);

            return Result.Ok(response);
        }

        public async Task<VehicleBookingDto> UpdateBookingAsync(VehicleBookingDto booking)
        {
            var entity = _mapper.Map<VehicleDto, Vehicle>(booking.Vehicle);

            _unitOfWork.VehicleRepo().Update(entity);
            await _unitOfWork.SaveAsync();

            return booking;
        }

        // TODO: Why is this here??
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

        public async Task<Result<VehicleBookingDto>> CompleteBooking(VehicleBookingDto bookingDto)
        {
            var booking = _mapper.Map<VehicleBookingDto, VehicleBooking>(bookingDto);

            _unitOfWork.VehicleBookingRepo().Update(booking);
            await _unitOfWork.SaveAsync();

            booking.Vehicle.IsBooked = false;
            _unitOfWork.VehicleRepo().Update(booking.Vehicle);
            await _unitOfWork.SaveAsync();

            return Result.Ok(bookingDto);
        }

        public async Task<IEnumerable<VehicleBookingDto>> GetHistoryAsync(int userId)
        {
            var bookings = await _unitOfWork.VehicleBookingRepo().GetByUserId(userId);

            return _mapper.Map<IEnumerable<VehicleBooking>, IEnumerable<VehicleBookingDto>>(bookings);
        }

        public async Task LeaveARating(VehicleBookingDto bookingDto)
        {
            var booking = _mapper.Map<VehicleBookingDto, VehicleBooking>(bookingDto);

            _unitOfWork.VehicleBookingRepo().Update(booking);
            await _unitOfWork.SaveAsync();
        }
    }
}
