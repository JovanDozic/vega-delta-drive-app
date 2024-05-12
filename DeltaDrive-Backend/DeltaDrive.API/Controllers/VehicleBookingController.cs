using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.BL.Contracts.IService;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleBookingController(IVehicleBookingService vehicleBookingService) : ControllerBase
    {
        private readonly IVehicleBookingService _vehicleBookingService = vehicleBookingService;

        [HttpPost("sendRequest")]
        //[Authorize] // TODO: Uncomment
        public async Task<Result<VehicleBookingResponseDto>> SendRequest([FromBody] VehicleBookingRequestDto request)
        {
            return await _vehicleBookingService.SendRequestAsync(request);
        }

    }
}
