using JFramework.Game;
using TiktokGame2Server.Entities;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    public class PlayerUnitBuilder : JCombatBaseUnitBuilder
    {
        Samurai samurai;
        public PlayerUnitBuilder(IJCombatAttrBuilder attrBuilder, IJCombatActionBuilder actionBuilder, Samurai samurai) : base(attrBuilder, actionBuilder)
        {
            this.samurai = samurai;
        }

        public override IJCombatUnitInfo Build()
        {
            var unitInfo = new TiktokJCombatUnitInfo
            {
                Uid = samurai.Id.ToString(),
                AttrList = attrBuilder.Create(),
                Actions = actionBuilder.Create(),
                SamuraiBusinessId = samurai.BusinessId,
                SoldierBusinessId = samurai.SoldierUid,
            };

            return unitInfo;
        }
    }
}



//public class FormationInfo
//{
//    public int FormationPoints { get; set; }

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
