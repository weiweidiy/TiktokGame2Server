using JFramework;
using JFramework.Game;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics.Eventing.Reader;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    /// <summary>
    /// 副本节点的战斗服务
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="formationService"></param>
    /// <param name="tiktokConfigService"></param>
    public class LevelNodeCombatService(MyDbContext dbContext
        , IFormationService formationService
        , TiktokConfigService tiktokConfigService) : ILevelNodeCombatService
    {
        private readonly MyDbContext _dbContext = dbContext;
        private readonly IFormationService formationService = formationService;
        private readonly TiktokConfigService tiktokConfigService = tiktokConfigService;

        /// <summary>
        /// 左队伍的阵型表
        /// </summary>
        List<Formation>? leftFormations = null;

        public async Task<JCombatTurnBasedReportData> GetReport(int playerId, string levelNodeBusinessId)
        {
            leftFormations = await formationService.GetFormationAsync(tiktokConfigService.GetAtkFormationType(), playerId);
            if (leftFormations == null || leftFormations.Count == 0)
            {
                throw new Exception("没有可用的阵型");
            }

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
            };//CreateFormationPointDelegate(tiktokConfigService.GetAtkFormationType()); // 阵型类型为1的武士点位
            Func<JCombatTurnBasedEvent, string> funcEvent = (e) => e.Uid;


            frameRecorder = new JCombatTurnBasedFrameRecorder(19); //从0开始，共20回合
            var attrFactory = new FakeAttrFacotry();
            var attrFactory2 = new FakeAttrFacotry2();

            jcombatQuery = new JCombatSeatBasedQuery(funcSeat, funcTeam, funcUnit, frameRecorder);

            eventRecorder = new TiktokEventRecorder(frameRecorder, funcEvent);

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


            //队伍2 从配置表获取武士点位
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

            combatRunner = new JCombatTurnBasedRunner(turnbasedCombat, jcombatQuery, eventRecorder, new TiktokJCombatResult());

            await combatRunner.Run();

            var result = combatRunner.GetReport();

            return result.GetCombatReportData();
        }




        /// <summary>
        /// 获取玩家武士在阵型中的坐标点位
        /// </summary>
        /// <returns></returns>
        Func<string, int> CreateFormationPointDelegate(int formationType)
        {
            // 从formation中获取武士的点位
            return (samuraiId) => // to do: 需要所有的战斗单位（包括NPC）
            {
                //从atkFormations中获取对应的阵型点位
                if (leftFormations == null || leftFormations.Count == 0)
                {
                    throw new Exception("没有可用的阵型");
                }
                var formation = leftFormations.FirstOrDefault(f => f.FormationType == formationType && f.SamuraiId == int.Parse(samuraiId));
                return formation?.FormationPoint ?? -1; // 如果没有找到对应的阵型点位，返回-1
            };
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

        public class TiktokEventRecorder : JCombatTurnBasedEventRecorder
        {
            public TiktokEventRecorder(IJCombatFrameRecorder frameRecorder, Func<JCombatTurnBasedEvent, string> keySelector) : base(frameRecorder, keySelector)
            {
            }
        }

        public class TiktokJCombatResult : IJCombatTurnBasedReport
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
