﻿using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class HpPoolService : IHpPoolService
    {
        private readonly MyDbContext _dbContext;
        public HpPoolService(MyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<int> GetHpPoolCurHpAsync(int playerId)
        {
            //从hppool中获取玩家的生命池
            var hpPool = await _dbContext.HpPools
                .FirstOrDefaultAsync(hp => hp.PlayerId == playerId);

            if (hpPool == null)
                {
                //如果没有找到，返回0
                return 0;
            }
            return hpPool.Hp;
        }

        public async Task<bool> AddHpPoolAsync(int playerId, int amount)
        {
            var hpPool = await _dbContext.HpPools
                .FirstOrDefaultAsync(hp => hp.PlayerId == playerId);
            if (hpPool == null)
            {
                hpPool = new HpPool { PlayerId = playerId, Hp = amount };
                _dbContext.HpPools.Add(hpPool);
            }
            else
            {
                hpPool.Hp += amount;
            }
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SubtractHpPoolAsync(int playerId, int amount)
        {
            var hpPool = await _dbContext.HpPools
                .FirstOrDefaultAsync(hp => hp.PlayerId == playerId);
            if (hpPool == null)
            {
                //如果没有找到，返回false
                return false;
            }
            hpPool.Hp -= amount;
            if (hpPool.Hp < 0)
            {
                hpPool.Hp = 0; // 确保生命池不会小于0
            }
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<HpPool> GetHpPoolAsync(int playerId)
        {
            return await _dbContext.HpPools
                .FirstOrDefaultAsync(hp => hp.PlayerId == playerId);

        }
    }
}
