using AutoMapper;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.DA.Contracts.Model;
using NetTopologySuite.Geometries;

namespace DeltaDrive.BL.Mapper
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<VehicleDto, Vehicle>().ReverseMap();
            CreateMap<PointDto, Point>().ReverseMap();
            CreateMap<VehicleSearchResponseDto, Vehicle>().ReverseMap();
            CreateMap<LocationDto, DA.Contracts.Model.Location>().ReverseMap();
            CreateMap<VehicleBookingDto, VehicleBooking>().ReverseMap();
        }
    }
}
