using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public interface ILevelNodeCombatService
    {
        Task<JCombatTurnBasedReportData> GetReport(int playerId, string levelNodeBusinessId);
    }
}
