
namespace TiktokGame2Server.Others
{
    public abstract class AchievementBase : IAchievement
    {
        float[] args;
        public AchievementBase(float[] args)
        {
            this.args = args;
        }

        public abstract bool IsCompleted(string playerUid, TiktokJCombatTurnBasedReportData reportData);
    }
}
