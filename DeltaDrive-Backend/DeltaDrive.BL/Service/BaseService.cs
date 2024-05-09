using AutoMapper;
using DeltaDrive.DA.Contracts;

namespace DeltaDrive.BL.Service
{
    public abstract class BaseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly IMapper _mapper = mapper;
    }
}
