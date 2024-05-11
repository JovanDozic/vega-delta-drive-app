using DeltaDrive.DA.Contracts.Shared;
using System.Text.RegularExpressions;

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

        public bool IsPasswordSecure()
        {
            return Password.Length > 8;
        }

        public bool IsEmailValid()
        {
            Regex regex = new(@"^\S+@\S+\.\S+$");
            return regex.IsMatch(Email);
        }

        public bool Validate()
        {
            return !string.IsNullOrWhiteSpace(Email) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   Birthday != default &&
                   IsPasswordSecure() &&
                   IsEmailValid();
        }
    }
}
