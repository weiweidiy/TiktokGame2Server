using JFramework.Game;
using Newtonsoft.Json;
using TiktokGame2Server.Others;

namespace Tiktok
{

    public class FightDTO 
    {
        public required string LevelNodeBusinessId { get; set; }

        public TiktokJCombatTurnBasedReportData? ReportData { get; set; }

        public  LevelNodeDTO? LevelNodeDTO { get; set; }


    }
}