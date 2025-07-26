using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class TiktokJCombatTurnBasedReport : JCombatTurnBasedReportBuilder
    {
        public TiktokJCombatTurnBasedReport(IJCombatSeatBasedQuery jcombatQuery) : base(jcombatQuery)
        {
        }

        protected override T CreateUnitData<T>(IJCombatUnit unit)
        {
            return new TiktokJCombatUnitData
            {
                Uid = unit.Uid,
                Seat = seatQuery.GetSeat(unit.Uid),
                SamuraiBusinessId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SamuraiBusinessId,
                SoldierBusinessId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SoldierBusinessId,
                Actions = GetActions(unit)
            } as T;
        }

        List<KeyValuePair<string, string>> GetActions(IJCombatUnit unit)
        {
            var result = new List<KeyValuePair<string, string>>();

            var actionInfos = (unit as IJCombatTurnBasedUnit).GetActionInfos();

            var tiktokActionInfos = actionInfos.OfType<TiktokJCombatActionInfo>().ToList();

            foreach(var actionInfo in tiktokActionInfos)
            {
                result.Add(new KeyValuePair<string, string>(actionInfo.Uid, actionInfo.ActionBusinessId));
            }

            return result;
        }
    }

    public class TiktokJCombatUnitData : IJCombatUnitData
    {
        public string? Uid { get; set; }
        public int Seat { get; set; }
        public string? SamuraiBusinessId { get; set; }
        public string? SoldierBusinessId { get; set; } // 可能需要在其他地方使用
        public List<KeyValuePair<string, string>>? Actions { get; set; } 
    }

    public class TiktokJCombatActionInfo : IJCombatAcionInfo
    {
        public string? Uid { get ; set ; }
        public string? ActionBusinessId { get; set; }
        public List<IJCombatTrigger>? Triggers { get; set; }
        public List<IJCombatExecutor>? Executors { get ; set ; }
    }
}


