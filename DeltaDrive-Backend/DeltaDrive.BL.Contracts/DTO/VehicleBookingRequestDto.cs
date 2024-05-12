namespace DeltaDrive.BL.Contracts.DTO
{
    public class VehicleBookingRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //public UserDto User { get; set; }
        public int VehicleId { get; set; }
        //public VehicleDto Driver { get; set; }
        public LocationDto StartLocation { get; set; }
        public LocationDto EndLocation { get; set; }
    }
}
