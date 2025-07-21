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
        /// 添加一个新的武士
        /// </summary>
        /// <param name="samuraiUid"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<Samurai> AddSamuraiAsync(string samuraiUid, int playerId);

        /// <summary>
        /// 删除指定的武士
        /// </summary>
        /// <param name="samuraiUid"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<bool> DeleteSamuraiAsync(string samuraiUid, int playerId);
    }
}
