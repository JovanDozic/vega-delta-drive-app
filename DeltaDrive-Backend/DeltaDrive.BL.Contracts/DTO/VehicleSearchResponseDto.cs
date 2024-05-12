namespace DeltaDrive.BL.Contracts.DTO
{
    public class VehicleSearchResponseDto : VehicleDto
    {
        public double DistanceFromPassenger { get; set; }
        public double EstimatedPrice { get; set; }
    }
}
