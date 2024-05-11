using DeltaDrive.DA.Contracts.Shared;

#nullable disable

namespace DeltaDrive.DA.Contracts.Model
{
    public class Driver : Entity
    {
        public string Brand { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string StartPrice { get; set; }
        public string PricePerKm { get; set; }
        public bool? IsBooked { get; set; }
        public float Rating { get; set; }
    }
}
