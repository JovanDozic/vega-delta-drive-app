using DeltaDrive.DA.Contracts.Shared;

#nullable disable
namespace DeltaDrive.DA.Contracts.Model
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }

        public void SecurePassword()
        {
            Password = BCrypt.Net.BCrypt.HashPassword(Password);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }
    }
}
