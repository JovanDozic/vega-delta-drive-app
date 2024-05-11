namespace DeltaDrive.BL.Contracts.DTO
{
    public class VehicleBookingRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int DriverId { get; set; }
        public VehicleDto Driver { get; set; }
        public DateTime BookingDate { get; set; }
        public bool? IsAccepted { get; set; } = null;
        public LocationDto StartLocation { get; set; }
        public LocationDto EndLocation { get; set; }
        public float Price { get; set; }
        public float Distance { get; set; }
    }
}
