namespace TiktokGame2Server.Others
{
    public interface IRewardService
    {
        Task AddReward(int playerId, string rewardBusinessId);
    }


}
