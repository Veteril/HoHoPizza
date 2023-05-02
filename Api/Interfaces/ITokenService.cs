using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(User user);
    }
}
