using DeltaDrive.DA.Contracts.IRepository.Authentication;
using DeltaDrive.DA.Contracts.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DeltaDrive.DA.Repository.Authentication
{
    public class TokenGeneratorRepo : ITokenGeneratorRepo
    {
        private readonly string _key = "long_secret_key_with_sufficient_length_here";
        private readonly string _issuer = "deltadrive.com";
        private readonly string _audience = "deltadrive.com";

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("id", user.Id.ToString()),
                new("email", user.Email),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                //new(ClaimTypes.Role, user.GetRoleName())
            };

            var jwt = CreateToken(claims, 60 * 24);

            return jwt;
        }

        private string CreateToken(IEnumerable<Claim> claims, double expirationTimeInMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddMinutes(expirationTimeInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
