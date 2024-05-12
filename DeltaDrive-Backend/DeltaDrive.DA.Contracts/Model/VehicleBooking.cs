using DeltaDrive.DA.Contracts.Shared;

namespace DeltaDrive.DA.Contracts.Model
{
    public class VehicleBooking : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public VehicleBookingStatus Status { get; set; }
        public DateTime BookingDate { get; set; }
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }
        public float Price { get; set; }
        public int Rating { get; set; }
    }
}
