using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class SamuraiService : ISamuraiService
    {
        private readonly MyDbContext _dbContext;
        private readonly TiktokConfigService tiktokConfigService;
        public SamuraiService(MyDbContext dbContext, TiktokConfigService tiktokConfigService)
        {
            _dbContext = dbContext;
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }

        /// <summary>
        /// 获取玩家的所有武士
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<Samurai>> GetAllSamuraiAsync(int playerId)
        {
            return await _dbContext.Samurais.Where(s => s.PlayerId == playerId).ToListAsync();
        }

        /// <summary>
        /// 获取指定武士
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public Task<Samurai?> GetSamuraiAsync(int samuraiId)
        {
            return _dbContext.Samurais.FirstOrDefaultAsync(s => s.Id == samuraiId);

        }

        /// <summary>
        /// 删除指定武士
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <returns></returns>
        public Task<bool> DeleteSamuraiAsync(int samuraiId)
        {
            var samurai = _dbContext.Samurais.Find(samuraiId);
            if (samurai == null)
                return Task.FromResult(false);
            _dbContext.Samurais.Remove(samurai);
            return _dbContext.SaveChangesAsync().ContinueWith(t => t.Result > 0);

        }

        /// <summary>
        /// 新添加一个武士
        /// </summary>
        /// <param name="samuraiUid"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Samurai> AddSamuraiAsync(string samuraiUid, string soldierUid, int playerId)
        {
            if(!CheckUid(samuraiUid))
                throw new ArgumentException($"武士 {samuraiUid} 配置数据不存在或无效。");

            var samurai = new Samurai
            {
                BusinessId = samuraiUid,
                PlayerId = playerId,
                SoldierUid = soldierUid,
                CurHp = tiktokConfigService.FormulaMaxHp(1),//默认1级
            };
            _dbContext.Samurais.Add(samurai);
            await _dbContext.SaveChangesAsync();
            return samurai;
        }

        /// <summary>
        /// 更新武士的血量
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <param name="curHp"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Samurai> UpdateSamuraiCurHp(int samuraiId, int curHp)
        {
            var samurai = await _dbContext.Samurais.FindAsync(samuraiId);
            if (samurai == null)
                throw new ArgumentException($"武士 {samuraiId} 不存在。");
            samurai.CurHp = curHp;
            _dbContext.Samurais.Update(samurai);
            await _dbContext.SaveChangesAsync();
            return samurai;
        }

        /// <summary>
        /// 增加武士的经验值
        /// </summary>
        /// <param name="samuraiId"></param>
        /// <param name="experience"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Samurai> AddSamuraiExperience(int samuraiId, int experience)
        {
            var samurai = await _dbContext.Samurais.FindAsync(samuraiId);
            if (samurai == null)
                throw new ArgumentException($"武士 {samuraiId} 不存在。");
            samurai.Experience += experience;
            //重新计算等级
            var level = tiktokConfigService.FormulaLevel(samurai.Experience);
            samurai.Level = level;

            _dbContext.Samurais.Update(samurai);
            await _dbContext.SaveChangesAsync();
            return samurai;
        }


        bool CheckUid(string samuraiBusinessId)
        {
            // 检查 samuraiBusinessId 是否符合预期格式
            // 这里可以根据实际情况进行验证
            return !string.IsNullOrEmpty(samuraiBusinessId)  && tiktokConfigService.IsValidSamurai(samuraiBusinessId);
        }

        public Task<Samurai> UpdateSamuraiHpAsync(int samuraiId, int curHp)
        {
            var samurai = _dbContext.Samurais.Find(samuraiId);
            if (samurai == null)
                throw new ArgumentException($"武士 {samuraiId} 不存在。");
            samurai.CurHp = curHp;
            _dbContext.Samurais.Update(samurai);
            return _dbContext.SaveChangesAsync().ContinueWith(t => samurai);

        }
    }
}
