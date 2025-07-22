using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface IFormationService
    {
        /// <summary>
        /// 获取指定玩家的阵型
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<List<Formation>?> GetFormationAsync(int formationType, int playerId);
        /// <summary>
        /// 更新指定玩家的阵型
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="formation"></param>
        /// <returns></returns>
        Task<Formation> AddFormationAsync(int formationType, int formationPoint, int samuraiId);

        /// <summary>
        /// 删除一个点位的配置
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="formationPoint"></param>
        /// <returns></returns>
        Task<bool> DeleteFormationAsync(int formationType, int formationPoint);
    }
}
