namespace DeltaDrive.BL.Contracts.DTO
{
    public class VehicleBookingResponseDto : VehicleBookingRequestDto
    {
        public bool IsAccepted { get; set; }
        public int? BookingId { get; set; }
    }
}
