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
        public async Task<Samurai> AddSamuraiAsync(string samuraiUid, int playerId)
        {
            var samurai = new Samurai
            {
                Uid = samuraiUid,
                PlayerId = playerId,
            };
            _dbContext.Samurais.Add(samurai);
            await _dbContext.SaveChangesAsync();
            return samurai;
        }
        public async Task<bool> DeleteSamuraiAsync(string samuraiUid, int playerId)
        {
            var samurai = await _dbContext.Samurais.FirstOrDefaultAsync(s => s.Uid == samuraiUid && s.PlayerId == playerId);
            if (samurai != null)
            {
                _dbContext.Samurais.Remove(samurai);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
