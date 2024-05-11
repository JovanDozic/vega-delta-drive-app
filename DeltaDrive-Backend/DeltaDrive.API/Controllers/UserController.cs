using DeltaDrive.BL.Contracts.DTO;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace DeltaDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("getInfo/{id}")]
        public Result<UserDto> GetInfo(int id)
        {
            return Result.Ok();
        }
    }
}
