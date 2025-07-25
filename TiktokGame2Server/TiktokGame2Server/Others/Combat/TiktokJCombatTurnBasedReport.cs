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
                SoldierBusinessId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SoldierBusinessId
            } as T;
        }
    }

    public class TiktokJCombatUnitData : IJCombatUnitData
    {
        public string Uid { get; set; }
        public int Seat { get; set; }
        public string SamuraiBusinessId { get; set; }
        public string SoldierBusinessId { get; set; } // 可能需要在其他地方使用
    }
}


