using DeltaDrive.DA.Contracts.Shared;

namespace DeltaDrive.DA.Contracts.Model
{
    public class VehicleBookingRating : Entity
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
