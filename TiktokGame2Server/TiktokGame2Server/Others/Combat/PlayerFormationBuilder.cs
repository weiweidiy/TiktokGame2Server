using JFramework;
using JFramework.Game;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class PlayerFormationBuilder : JCombatFormationBuilder
    {
        List<Formation> playerFormation;
        public PlayerFormationBuilder(List<Formation> playerFormation, IJCombatUnitBuilder unitBuilder):base(unitBuilder)
        {
            this.playerFormation = playerFormation ?? throw new ArgumentNullException(nameof(playerFormation));
        }
        public override List<JCombatFormationInfo> Build()
        {
            return playerFormation.Select(f => new JCombatFormationInfo
            {
                Point = f.FormationPoint,
                UnitInfo = unitBuilder.Build(f.SamuraiId)               
            }).ToList();
        }
    }


    public class TiktokJCombatUnitInfo : JCombatUnitInfo
    {
        public int SamuraiId { get; set; }
        public int SoldierId { get; set; }
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
