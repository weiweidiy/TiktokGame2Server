using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IPlayerService
    {
        Task<Player?> GetPlayerAsync(string playerId);
        Task<Player> CreatePlayerAsync(string playerId, string name, int accountId);
    }
}
