using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public interface ILevelNodeCombatService
    {
        Task<JCombatTurnBasedReportData<TiktokJCombatUnitData>> GetReport(int playerId, string levelNodeBusinessId);
    }
}
