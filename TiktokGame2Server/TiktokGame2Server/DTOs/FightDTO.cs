using JFramework.Game;
using Newtonsoft.Json;

namespace Tiktok
{

    public class FightDTO 
    {
        public required string LevelNodeBusinessId { get; set; }

        public  JCombatTurnBasedReportData? ReportData { get; set; }

        public  LevelNodeDTO? LevelNodeDTO { get; set; }


    }
}