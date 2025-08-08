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
        Task<Samurai> AddSamuraiAsync(string samuraiBusinessId, string soldierBusinessId, int playerId);

        /// <summary>
        /// 添加武将
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <param name="soldierBusinessId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<List<Samurai>> AddSamuraisAsync(List<string> samuraiBusinessId, List<string> soldierBusinessId, int playerId);
        /// <summary>
        /// 删除指定的武士
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<bool> DeleteSamuraiAsync(int samuraiId);

        /// <summary>
        /// 更新武将信息
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <param name="curHp"></param>
        /// <returns></returns>
        Task<Samurai> UpdateSamuraiHpAsync(int samuraiId, int curHp);

        /// <summary>
        /// 通过武士UID查询武士ID
        /// </summary>
        /// <param name="samuraiUid"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        //Task<int> QuerySamuraiId(string samuraiUid, int playerId);

        //Task<string> QuerySamuraiUid(int samuraiId, int playerId);
    }
}
