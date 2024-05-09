using DeltaDrive.DA.Contracts.Model;

namespace DeltaDrive.DA.Contracts.IRepository.Authentication
{
    public interface ITokenGeneratorRepo
    {
        public string GenerateAccessToken(User user);
    }
}
