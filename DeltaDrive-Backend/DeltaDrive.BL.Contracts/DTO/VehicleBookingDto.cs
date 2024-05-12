namespace DeltaDrive.BL.Contracts.DTO
{
    public class VehicleBookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }
        public int VehicleId { get; set; }
        public VehicleDto Vehicle { get; set; }
        public DateTime BookingDate { get; set; }
        public LocationDto StartLocation { get; set; }
        public LocationDto EndLocation { get; set; }
        public float Price { get; set; }
        public int Rating { get; set; }
    }
}
