using DeltaDrive.DA.Contracts.Shared;
using NetTopologySuite.Geometries;
#nullable disable

namespace DeltaDrive.DA.Contracts.Model
{
    public class Vehicle : Entity
    {
        public string Brand { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Point Location { get; set; }
        public double StartPrice { get; set; }
        public double PricePerKm { get; set; }
        public bool? IsBooked { get; set; }
        public float? Rating { get; set; } // TODO: Calculate average rating from bookings
    }
}
