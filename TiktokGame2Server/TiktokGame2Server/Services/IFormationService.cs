using Tiktok;
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
        Task<Formation> AddOrUpdateFormationSamuraiAsync(int formationType, int formationPoint, int samuraiId, int playerId);

        /// <summary>
        /// 删除一个点位的配置
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="formationPoint"></param>
        /// <returns></returns>
        Task<bool> DeleteFormationSamuraiAsync(int formationType, int formationPoint);


        Task<int> GetFormationPoint(int formationType, int samuraiId);
        Task DeleteFormationAsync(List<Formation> formationDataToDelete);
        Task UpdateFormationAsync(Formation existingFormation);

        Task<List<Formation>> UpdateFormationAsync(FormationType formationType, List<FormationDTO> newFormations, int playerId);
        Task<List<int>> GetFormationSamuraiIdsAsync(int playerId);
    }
}
