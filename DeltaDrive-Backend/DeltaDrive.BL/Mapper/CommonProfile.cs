using AutoMapper;
using DeltaDrive.BL.Contracts.DTO;
using DeltaDrive.DA.Contracts.Model;

namespace DeltaDrive.BL.Mapper
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
