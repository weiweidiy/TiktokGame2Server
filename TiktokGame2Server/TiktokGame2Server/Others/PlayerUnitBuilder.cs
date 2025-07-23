using JFramework;
using JFramework.Game;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    public class PlayerUnitBuilder : JCombatBaseUnitBuilder
    {
        public PlayerUnitBuilder(IJCombatAttrBuilder attrBuilder, IJCombatActionBuilder actionBuilder) : base(attrBuilder, actionBuilder)
        {
        }
    }

    public class PlayerAttrBuilder : IJCombatAttrBuilder
    {
        public List<IUnique> Create(int key)
        {
            return new FakeAttrFacotry().Create();
        }
    }

    public class PlayerActionsBuilder : IJCombatActionBuilder
    {
        public List<IJCombatAction> Create(int key)
        {
            var result = new List<IJCombatAction>();

            var finder1 = new JCombatDefaultFinder();
            var executor1 = new JCombatExecutorDamage(finder1);
            var lstExecutor1 = new List<IJCombatExecutor>();
            lstExecutor1.Add(executor1);

            var action = new TiktokJCombatAction(Guid.NewGuid().ToString(), lstExecutor1);
            result.Add(action);

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
