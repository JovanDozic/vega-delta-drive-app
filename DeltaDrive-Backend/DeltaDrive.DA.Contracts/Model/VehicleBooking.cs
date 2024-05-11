using DeltaDrive.DA.Contracts.Shared;

namespace DeltaDrive.DA.Contracts.Model
{
    public class VehicleBooking : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int DriverId { get; set; }
        public Vehicle Driver { get; set; }
        public DateTime BookingDate { get; set; }
        public bool? IsAccepted { get; set; } = null;
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }
        public float Price { get; set; }
        public float Distance { get; set; }

    }
}
