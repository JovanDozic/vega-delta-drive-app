namespace DeltaDrive.BL.Contracts.DTO.Model
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PointDto Location { get; set; }
        public double StartPrice { get; set; }
        public double PricePerKm { get; set; }
        public bool? IsBooked { get; set; }
        public float? Rating { get; set; }
    }
}