using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface ITokenService
    {
        string GenerateToken(Account user);

        bool ValidateToken(string token);
    }
}
