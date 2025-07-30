namespace TiktokGame2Server.Others
{
    public interface IHpPoolService
    {
        Task<int> GetHpPoolAsync(int playerId);
        Task<bool> AddHpPoolAsync(int playerId, int amount);
        Task<bool> SubtractHpPoolAsync(int playerId, int amount);
    }
}
