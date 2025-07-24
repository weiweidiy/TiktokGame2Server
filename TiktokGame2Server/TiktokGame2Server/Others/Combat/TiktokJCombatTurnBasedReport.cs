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
                SamuraiId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SamuraiId,
                SoldierId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SoldierId
            } as T;
        }
    }

    public class TiktokJCombatUnitData : IJCombatUnitData
    {
        public string Uid { get; set; }
        public int Seat { get; set; }
        public int SamuraiId { get; set; }
        public int SoldierId { get; set; } // 可能需要在其他地方使用
    }
}


