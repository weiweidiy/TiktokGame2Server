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

        Task<bool> DeleteFormationAsync(int formationType, int formationPoint);
    }
}
