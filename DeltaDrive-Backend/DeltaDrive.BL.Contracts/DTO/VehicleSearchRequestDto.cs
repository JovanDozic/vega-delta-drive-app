﻿using DeltaDrive.BL.Contracts.DTO.Model;

namespace DeltaDrive.BL.Contracts.DTO
{
    public class VehicleSearchRequestDto
    {
        public LocationDto StartLocation { get; set; }
        public LocationDto EndLocation { get; set; }
    }
}
