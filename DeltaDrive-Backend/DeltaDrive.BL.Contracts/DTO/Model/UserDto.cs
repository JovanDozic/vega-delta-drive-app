#nullable disable

using DeltaDrive;

namespace DeltaDrive.BL.Contracts.DTO.Model
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
