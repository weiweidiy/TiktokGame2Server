﻿using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class RewardService : IRewardService
    {

        private readonly TiktokConfigService tiktokConfigService;
        private readonly ICurrencyService currencyService;
        public RewardService(TiktokConfigService tiktokConfigService, ICurrencyService currencyService)
        {
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
            this.currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }
        public async Task<List<(ResourceType,string,int)>> AddReward(int playerId, string rewardBusinessId)
        {
            var result = new List<(ResourceType, string, int)>();

            //发放货币奖励
            var currenciesTypes = tiktokConfigService.GetRewardCurrenciesTypes(rewardBusinessId);
            if(currenciesTypes != null && currenciesTypes.Length > 0)
            {
                var rewardsCounts = tiktokConfigService.GetRewardCurrenciesCounts(rewardBusinessId);
                if(rewardsCounts == null || rewardsCounts.Length != currenciesTypes.Length)
                {
                    throw new Exception($"奖励配置错误，奖励ID：{rewardBusinessId}，奖励数量与类型不匹配");
                }
                //遍历奖励类型，分别添加
                for (int i = 0; i < currenciesTypes.Length; i++)
                {
                    var currencyReward = await currencyService.AddCurrency(playerId, currenciesTypes[i], rewardsCounts[i]);
                    result.Add((ResourceType.Currency, ((int)currenciesTypes[i]).ToString(), rewardsCounts[i]));
                }
            }

            //// 发放物品奖励
            //var itemBusinessIds = tiktokConfigService.GetRewardItemsBusinessIds(rewardBusinessId);
            //var itemCounts = tiktokConfigService.GetRewardItemsCounts(rewardBusinessId);
            //if (itemBusinessIds != null && itemBusinessIds.Length > 0)
            //{
            //    if (itemCounts == null || itemCounts.Length != itemBusinessIds.Length)
            //    {
            //        throw new Exception($"奖励配置错误，奖励ID：{rewardBusinessId}，物品数量与类型不匹配");
            //    }
            //    for (int i = 0; i < itemBusinessIds.Length; i++)
            //    {
            //        // 这里假设你有 AddItemToBag 方法，实际实现请根据你的 BagService 或相关逻辑调整
            //        // await bagService.AddItemToBag(playerId, itemBusinessIds[i], itemCounts[i]);
            //        result.Add((ResourceType.BagItem, itemBusinessIds[i], itemCounts[i]));
            //    }
            //}

            return result;


        }
    }


}
