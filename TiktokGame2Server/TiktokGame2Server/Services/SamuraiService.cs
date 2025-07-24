using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class SamuraiService : ISamuraiService
    {
        private readonly MyDbContext _dbContext;
        public SamuraiService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Samurai>> GetAllSamuraiAsync(int playerId)
        {
            return await _dbContext.Samurais.Where(s => s.PlayerId == playerId).ToListAsync();
        }

        public Task<Samurai?> GetSamuraiAsync(int samuraiId)
        {
            return _dbContext.Samurais.FirstOrDefaultAsync(s => s.Id == samuraiId);

        }

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
                SoldierUid = soldierUid
            };
            _dbContext.Samurais.Add(samurai);
            await _dbContext.SaveChangesAsync();
            return samurai;
        }

        bool CheckUid(string samuraiUid)
        {
            // 检查 samuraiUid 是否符合预期格式
            // 这里可以根据实际情况进行验证
            return !string.IsNullOrEmpty(samuraiUid);
        }


    }
}
