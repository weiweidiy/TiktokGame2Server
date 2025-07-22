using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class FormationService : IFormationService
    {
        private readonly MyDbContext _dbContext;
        public FormationService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Formation>?> GetFormationAsync(int formationType, int playerId)
        {
            // 查找指定玩家的阵型
            var formations = await _dbContext.Formations
                .Where(f => f.FormationType == formationType && f.PlayerId == playerId)
                .ToListAsync();
            return formations;


        }

        /// <summary>
        /// 根据阵型类型和武士ID获取阵型点位
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public async Task<int> GetFormationPoint(int formationType, int samuraiId)
        {
            // 查找指定阵型类型和武士ID的阵型点位
            var formation = await _dbContext.Formations
                .FirstOrDefaultAsync(f => f.FormationType == formationType && f.SamuraiId == samuraiId);
            if (formation != null)
            {
                return formation.FormationPoint;
            }
            return -1; // 如果没有找到，返回-1表示未设置点位
        }

        /// <summary>
        /// 添加一个阵型点位数据
        /// </summary>
        /// <param name="formationType"></param>
        /// <param name="formationPoint"></param>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public async Task<Formation> AddFormationAsync(int formationType, int formationPoint, int samuraiId, int playerId)
        {
            //先查询是否存在相同的阵型和位置
            var existingFormation = await _dbContext.Formations
                .FirstOrDefaultAsync(f => f.FormationType == formationType && f.FormationPoint == formationPoint && f.SamuraiId == samuraiId && f.PlayerId == playerId);

            //如果存在，则修改对应的samuraiId
            if (existingFormation != null)
            {
                existingFormation.SamuraiId = samuraiId;
                _dbContext.Formations.Update(existingFormation);
                await _dbContext.SaveChangesAsync();
                return existingFormation;
            }
            else
            //如果不存在，则添加新的阵型
            {
                var formation = new Formation
                {
                    FormationType = formationType,
                    FormationPoint = formationPoint,
                    SamuraiId = samuraiId,
                    PlayerId = playerId
                };
                _dbContext.Formations.Add(formation);
                await _dbContext.SaveChangesAsync();
                return formation;
            }
        }

        public async Task<bool> DeleteFormationAsync(int formationType, int formationPoint)
        {
            var formation = await _dbContext.Formations
                .FirstOrDefaultAsync(f => f.FormationType == formationType && f.FormationPoint == formationPoint);
            if (formation != null)
            {
                _dbContext.Formations.Remove(formation);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }
    }
}
