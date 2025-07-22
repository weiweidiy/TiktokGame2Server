using JFramework.Game;
using Newtonsoft.Json;

namespace Tiktok
{

    public class FightDTO 
    {
        public required string LevelNodeBusinessId { get; set; }

        public required JCombatTurnBasedReportData ReportData { get; set; }

        public required LevelNodeDTO LevelNodeDTO { get; set; }


    }
}