using JFramework;
using JFramework.Game;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Diagnostics.Eventing.Reader;
using TiktokGame2Server.Entities;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    /// <summary>
    /// 副本节点的战斗服务
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="formationService"></param>
    /// <param name="tiktokConfigService"></param>
    public partial class LevelNodeCombatService(MyDbContext dbContext
        , IFormationService formationService
        , TiktokConfigService tiktokConfigService) : ILevelNodeCombatService
    {
        private readonly MyDbContext _dbContext = dbContext;
        private readonly IFormationService formationService = formationService;
        private readonly TiktokConfigService tiktokConfigService = tiktokConfigService;

        ///// <summary>
        ///// 阵型查询列表
        ///// </summary>
        //List<JCombatFormationInfo> lstFormationQuery = null;
        public async Task<JCombatTurnBasedReportData<TiktokJCombatUnitData>> GetReport(int playerId, string levelNodeBusinessId)
        {
            //创建阵型查询列表（包含玩家阵型和NPC阵型）
            var playerFormations = await formationService.GetFormationAsync(tiktokConfigService.GetAtkFormationType(), playerId);
            if (playerFormations == null || playerFormations.Count == 0)
            {
                throw new Exception("玩家没有可用的阵型");
            }

            //构建双方阵型信息
            var playerFormationBuilder = new PlayerFormationBuilder(playerFormations, new PlayerUnitBuilder(new PlayerAttrBuilder(), new PlayerActionsBuilder()));
            var levelNodeFormationBuilder = new LevelNodeFormationBuilder(levelNodeBusinessId, new LevelNodeUnitBuilder(new LevelNodeAttrBuilder(), new LevelNodeActionsBuilder()),tiktokConfigService);
            var playerFormationInfos = playerFormationBuilder.Build();
            var levelodeFormationInfos = levelNodeFormationBuilder.Build();

            //创建阵型点位查询构建器
            var lst = new List<JCombatFormationInfo>();
            lst.AddRange(playerFormationInfos);
            lst.AddRange(levelodeFormationInfos);
            var formationBuilder = new JCombatSeatFuncBuilder(lst);

            JCombatTurnBased turnbasedCombat;
            JCombatTurnBasedFrameRecorder frameRecorder;
            JCombatSeatBasedQuery jcombatQuery;
            JCombatTeam team1;
            JCombatTeam team2;
            JCombatTurnBasedEventRecorder eventRecorder;
            JCombatTurnBasedActionSelector actionSelector;
            JCombatTurnBasedRunner combatRunner;
            IJCombatTurnBasedAttrNameQuery attrNameQuery;
            JCombatTurnBasedReportBuilder report;

            //创建帧记录器
            frameRecorder = new JCombatTurnBasedFrameRecorder(19); //从0开始，共20回合
            //查询工具，funcSeat 可以替换成 formationBuilder
            jcombatQuery = new JCombatSeatBasedQuery(formationBuilder, frameRecorder);
            eventRecorder = new JCombatTurnBasedEventRecorder(frameRecorder);
            attrNameQuery = new TiktokAttrNameQuery();
            report = new TiktokJCombatTurnBasedReport(jcombatQuery);

            //创建玩家unit对象
            var playerUnits = new List<IJCombatUnit>();
            foreach (var formationInfo in playerFormationInfos)
            {
                //队伍1
                var unit1 = new JCombatTurnBasedUnit(formationInfo.UnitInfo, attrNameQuery, eventRecorder);
                playerUnits.Add(unit1);
            }
            team1 = new JCombatTeam("team1", playerUnits);

            //创建levelnode unit对象
            var levelNodeUnits = new List<IJCombatUnit>();
            foreach (var formationInfo in levelodeFormationInfos)
            {
                //队伍2 从配置表获取武士点位
                var unit2 = new JCombatTurnBasedUnit(formationInfo.UnitInfo, attrNameQuery, eventRecorder);
                levelNodeUnits.Add(unit2);
            }
            team2 = new JCombatTeam("team2", levelNodeUnits);

            var lstTeams = new List<IJCombatTeam>();
            lstTeams.Add(team1);
            lstTeams.Add(team2);

            //设置双方队伍
            jcombatQuery.SetTeams(lstTeams);

            actionSelector = new JCombatTurnBasedActionSelector(jcombatQuery.GetUnits().OfType<IJCombatTurnBasedUnit>().ToList());

            var runables = new List<IRunable>();
            runables.Add(team1);
            runables.Add(team2);

            turnbasedCombat = new JCombatTurnBased(actionSelector, frameRecorder, jcombatQuery, runables);
            combatRunner = new JCombatTurnBasedRunner(turnbasedCombat, jcombatQuery, eventRecorder, report);

            await combatRunner.Run();

            var result = combatRunner.GetReport();

            return result.GetCombatReportData<TiktokJCombatUnitData>();
        }

    }

    


}


