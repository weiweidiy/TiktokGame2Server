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
        public async Task AddReward(int playerId, string rewardBusinessId)
        {
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
                    await currencyService.AddCurrency(playerId, (CurrencyType)currenciesTypes[i], rewardsCounts[i]);
                }
            }
        }
    }


}
