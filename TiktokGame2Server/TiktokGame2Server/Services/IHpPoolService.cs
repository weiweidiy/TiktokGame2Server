using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IHpPoolService
    {
        Task<HpPool> GetHpPoolAsync(int playerId);
        Task<int> GetHpPoolCurHpAsync(int playerId);
        Task<bool> AddHpPoolAsync(int playerId, int amount);
        Task<bool> SubtractHpPoolAsync(int playerId, int amount);
    }
}
