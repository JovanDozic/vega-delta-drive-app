﻿using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.DTO.Model;
using FluentResults;

namespace DeltaDrive.BL.Contracts.IService
{
    public interface IVehicleBookingService
    {
        public Task<Result<VehicleBookingResponseDto>> SendRequestAsync(VehicleBookingRequestDto request);
        public Result<VehicleBookingDto> GetUsersBooking(int id, int userId);
        public Result<VehicleBookingDto> GetBooking(int id);
        public Task<Result<VehicleBookingDto>> CompleteBooking(VehicleBookingDto booking);
        public Task<IEnumerable<VehicleBookingDto>> GetHistoryAsync(int userId);
        public Task LeaveARating(VehicleBookingDto bookingDto);
    }
}
