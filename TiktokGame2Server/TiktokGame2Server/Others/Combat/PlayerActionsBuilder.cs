using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class PlayerActionsBuilder : IJCombatActionBuilder
    {
        int playerId;
        int samuraiId;
        IJCombatTurnBasedEventRecorder recorder;
        public PlayerActionsBuilder(int playerId, int samuraiId, IJCombatTurnBasedEventRecorder recorder)
        {
            this.playerId = playerId;
            this.samuraiId = samuraiId;
            this.recorder = recorder;
        }

        public List<IJCombatAction> Create()
        {
            var result = new List<IJCombatAction>();

            var actionBusinessIds = GetActionsBusiness(playerId, samuraiId);

            foreach(var actionBusinessId in actionBusinessIds)
            {
                var finder1 = new JCombatDefaultFinder();
                var formular = new TiktokNormalFormula();
                var executor1 = new JCombatExecutorDamage(finder1, formular);
                var lstExecutor1 = new List<IJCombatExecutor>();
                lstExecutor1.Add(executor1);

                var actionInfo = new TiktokJCombatActionInfo();
                actionInfo.Uid = Guid.NewGuid().ToString();
                actionInfo.ActionBusinessId = actionBusinessId;
                actionInfo.Executors = lstExecutor1;

                var action1 = new JCombatActionBase(actionInfo, recorder);
                result.Add(action1);
            }

            return result;
        }

        private List<string> GetActionsBusiness(int playerId, int samuraiId)
        {
            var result = new List<string>();
            // 这里可以根据playerId和samuraiId获取对应的技能或动作
            // 例如，可以从数据库或配置文件中获取
            // 这里简单示例添加一个动作
            result.Add("ActionPlayer");
            return result;

        }
    }
}



//public class FormationInfo
//{
//    public int FormationPoint { get; set; }

//    public required JCombatUnitInfo UnitInfo { get; set; }
//}

///// <summary>
///// 获取玩家武士在阵型中的坐标点位
///// </summary>
///// <returns></returns>
//Func<string, int> CreateFormationPointDelegate(int formationType)
//{
//    // 从formation中获取武士的点位
//    return (unitUid) => // to do: 需要所有的战斗单位（包括NPC）
//    {
//        //从atkFormations中获取对应的阵型点位
//        if (lstFormationQuery == null || lstFormationQuery.Count == 0)
//        {
//            throw new Exception("没有可用的阵型");
//        }
//        var formation = lstFormationQuery.FirstOrDefault(f =>  f.UnitInfo.Uid == unitUid);
//        return formation?.Point ?? -1; // 如果没有找到对应的阵型点位，返回-1
//    };
//}
