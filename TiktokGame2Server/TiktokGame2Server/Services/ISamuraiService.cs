using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface ISamuraiService
    {
        /// <summary>
        /// 获取指定玩家的所有武士
        /// </summary>
        /// <param name="samuraiUid"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<List<Samurai>> GetAllSamuraiAsync(int playerId);

        /// <summary>
        /// 获取指定的武士信息
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        Task<Samurai?> GetSamuraiAsync(int samuraiId);

        /// <summary>
        /// 添加一个新的武士
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<Samurai> AddSamuraiAsync(string samuraiBusinessId, int playerId);

        /// <summary>
        /// 删除指定的武士
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<bool> DeleteSamuraiAsync(int samuraiId);
    }
}
