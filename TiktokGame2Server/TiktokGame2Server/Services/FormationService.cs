using Microsoft.EntityFrameworkCore;
using Tiktok;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class FormationService : IFormationService
    {
        private readonly MyDbContext _dbContext;
        ISamuraiService samuraiService;
        public FormationService(MyDbContext dbContext, ISamuraiService samuraiService)
        {
            _dbContext = dbContext;
            this.samuraiService = samuraiService;
        }
        public async Task<List<Formation>?> GetFormationAsync(int formationType, int playerId)
        {
            // 查找指定玩家的阵型
            var formations = await _dbContext.Formations
                .Where(f => f.FormationType == formationType && f.PlayerId == playerId)
                .Include(f => f.Samurai)
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
        public async Task<Formation> AddOrUpdateFormationSamuraiAsync(int formationType, int formationPoint, int samuraiId, int playerId)
        {
            //先查询是否存在相同的阵型和位置
            var existingFormation = await _dbContext.Formations
                .FirstOrDefaultAsync(f => f.FormationType == formationType && f.FormationPoint == formationPoint /*&& f.SamuraiId == samuraiId*/ && f.PlayerId == playerId);

            //如果存在，则修改对应的samuraiId, 替换武将
            if (existingFormation != null)
            {
                existingFormation.SamuraiId = samuraiId;
                _dbContext.Formations.Update(existingFormation);
                await _dbContext.SaveChangesAsync();
                return existingFormation;
            }
            else
            //如果不存在，则添加新的上阵武将
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

        public async Task<bool> DeleteFormationSamuraiAsync(int formationType, int formationPoint)
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

        public async Task DeleteFormationAsync(List<Formation> formationDataToDelete)
        {
            if (formationDataToDelete == null || formationDataToDelete.Count == 0)
            {
                return; // 如果没有要删除的数据，直接返回
            }
            // 批量删除
            _dbContext.Formations.RemoveRange(formationDataToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateFormationAsync(Formation existingFormation)
        {
            if (existingFormation == null)
            {
                throw new ArgumentNullException(nameof(existingFormation), "Existing formation cannot be null");
            }
            // 更新阵型数据
            _dbContext.Formations.Update(existingFormation);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 更新一个阵型的数据
        /// </summary>
        /// <param name="newFormations"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<Formation>> UpdateFormationAsync(FormationType formationType, List<FormationDTO> newFormations, int playerId)
        {
            //to do:检查formationDTO中的samuraiId是否有重复，数据库是否拥有



            // 如果没有要更新的数据，返回当前数据库中的数据
            if (newFormations == null || newFormations.Count == 0)
            {
                return await _dbContext.Formations
                    .Where(f => f.FormationType == (int)formationType && f.PlayerId == playerId)
                    .ToListAsync();
            }


            // 首先删除所有旧的指定类型的阵型数据
            var existingFormations = await _dbContext.Formations
                .Where(f => f.FormationType == (int)formationType && f.PlayerId == playerId)
                .ToListAsync();
            //从数据库中删除旧的阵型数据existingFormations
            if (existingFormations.Count > 0)
            {
                _dbContext.Formations.RemoveRange(existingFormations);
            }


            // 遍历新的阵型数据，进行添加 
            var addedFormations = new List<Formation>();
            foreach (var formationDTO in newFormations)
            {
                //var samuraiId = await samuraiService.QuerySamuraiId(formationDTO.SamuraiId, playerId);
                //从数据库中查询该玩家是否有该武将
                var samurai = await samuraiService.GetSamuraiAsync(formationDTO.SamuraiId);
                if (samurai == null)
                    continue;

                var newFormation = new Formation
                {
                    FormationType = formationDTO.FormationType,
                    FormationPoint = formationDTO.FormationPoint,
                    SamuraiId = samurai.Id, 
                    PlayerId = playerId
                };
                _dbContext.Formations.Add(newFormation);
                addedFormations.Add(newFormation);
            }

            await _dbContext.SaveChangesAsync();
            return addedFormations;
        }
    }
}


