using JFramework;
using JFramework.Game;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics.Eventing.Reader;

namespace TiktokGame2Server.Others
{
    public class LevelNodeCombatService : ILevelNodeCombatService
    {

        public async Task<JCombatTurnBasedReportData> GetReport(int playerId, string levelNodeBusinessId)
        {
            JCombatTurnBased turnbasedCombat;
            JCombatTurnBasedFrameRecorder frameRecorder;
            JCombatSeatBasedQuery jcombatQuery;
            JCombatTeam team1;
            JCombatTeam team2;
            JCombatTurnBasedEventRecorder eventRecorder;
            JCombatTurnBasedActionSelector actionSelector;

            Func<IUnique, string> funcAttr = (attr) => attr.Uid;

            JCombatTurnBasedRunner combatRunner;

            Func<IJCombatUnit, string> funcUnit = (unit) => unit.Uid;

            Func<IJCombatTeam, string> funcTeam = (team) => team.Uid;
            Func<string, int> funcSeat = (unitUid) => {

                switch (unitUid)
                {
                    case "unit1":
                    case "unit2":
                        return 1;
                    case "unit3":
                        return 2;
                    default:
                        throw new Exception("没有定义座位 " + unitUid);
                }
            };
            Func<JCombatTurnBasedEvent, string> funcEvent = (e) => e.Uid;


            frameRecorder = new JCombatTurnBasedFrameRecorder(19); //从0开始，共20回合
            var attrFactory = new FakeAttrFacotry();
            var attrFactory2 = new FakeAttrFacotry2();

            jcombatQuery = new JCombatSeatBasedQuery(funcSeat, /*lstTeams,*/ funcTeam, funcUnit, frameRecorder);

            eventRecorder = new FakeEventRecorder(frameRecorder, funcEvent);

            //执行器
            var finder1 = new JCombatDefaultFinder(jcombatQuery);
            var executor1 = new JCombatExecutorDamage(jcombatQuery, finder1);
            var lstExecutor1 = new List<IJCombatExecutor>();
            lstExecutor1.Add(executor1);

            //队伍1
            var unit1 = new JCombatTurnBasedUnit("unit1", attrFactory.Create(), funcAttr, new FakeAttrNameQuery(), new List<IJCombatAction>() { new FakeJCombatAction(jcombatQuery, "action1", lstExecutor1) }, eventRecorder);
            var lst1 = new List<IJCombatCasterTargetableUnit>();
            lst1.Add(unit1);
            team1 = new JCombatTeam("team1", lst1, funcUnit);


            //队伍2
            var unit2 = new JCombatTurnBasedUnit("unit2", attrFactory2.Create(), funcAttr, new FakeAttrNameQuery(), new List<IJCombatAction>() { new FakeJCombatAction(jcombatQuery, "action2", null) }, eventRecorder);
            var lst2 = new List<IJCombatCasterTargetableUnit>();
            lst2.Add(unit2);
            team2 = new JCombatTeam("team2", lst2, funcUnit);

            var lstTeams = new List<IJCombatTeam>();
            lstTeams.Add(team1);
            lstTeams.Add(team2);


            jcombatQuery.AddRange(lstTeams);

            actionSelector = new JCombatTurnBasedActionSelector(jcombatQuery.GetUnits().OfType<IJCombatTurnBasedUnit>().ToList(), funcUnit);

            var runables = new List<IRunable>();
            runables.Add(team1);
            runables.Add(team2);

            turnbasedCombat = new JCombatTurnBased(actionSelector, frameRecorder, jcombatQuery, runables);

            combatRunner = new JCombatTurnBasedRunner(turnbasedCombat, jcombatQuery, eventRecorder, new FakeJCombatResult());

            await combatRunner.Run();

            var result = combatRunner.GetReport();

            return result.GetCombatReportData();
        }



        public class FakeAttrFacotry : IJCombatUnitAttrFactory
        {
            public List<IUnique> Create()
            {
                var result = new List<IUnique>();

                var hp = new GameAttributeInt("Hp", 100, 100);
                var speed = new GameAttributeInt("Speed", 50, 50);
                result.Add(hp);
                result.Add(speed);

                return result;
            }
        }

        public class FakeAttrFacotry2 : IJCombatUnitAttrFactory
        {
            public List<IUnique> Create()
            {
                var result = new List<IUnique>();

                var hp = new GameAttributeInt("Hp", 200, 200);
                var speed = new GameAttributeInt("Speed", 40, 60);
                result.Add(hp);
                result.Add(speed);

                return result;
            }
        }

        public class FakeAttrNameQuery : IJCombatTurnBasedAttrNameQuery
        {
            public string GetActionPointName()
            {
                return "Speed";
            }

            public string GetHpAttrName()
            {
                return "Hp";
            }
        }

        public class FakeEventRecorder : JCombatTurnBasedEventRecorder
        {
            public FakeEventRecorder(IJCombatFrameRecorder frameRecorder, Func<JCombatTurnBasedEvent, string> keySelector) : base(frameRecorder, keySelector)
            {
            }
        }

        public class FakeJCombatResult : IJCombatTurnBasedReport
        {
            JCombatTurnBasedReportData data = new JCombatTurnBasedReportData();
            public JCombatTurnBasedReportData GetCombatReportData()
            {
                return data;
            }

            public void SetCombatEvents(List<JCombatTurnBasedEvent> events)
            {
                data.events = events;
            }

            public void SetCombatWinner(IJCombatTeam team)
            {
                data.winnerTeamUid = team.Uid;
            }
        }

        public class FakeJCombatAction : JCombatActionBase
        {
            public FakeJCombatAction(IJCombatQuery query, string uid, List<IJCombatExecutor> executors) : base(query, uid, null, executors)
            {
            }
        }
    }
}
