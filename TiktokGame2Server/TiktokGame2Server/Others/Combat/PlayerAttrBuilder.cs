using JFramework;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class PlayerAttrBuilder : IJCombatAttrBuilder
    {
        int playerId;
        int samuraiId;
        public PlayerAttrBuilder(int playerId, int samuraiId) { 
            this.playerId = playerId;
            this.samuraiId = samuraiId;
        }
        public List<IUnique> Create()
        {
            var result = new List<IUnique>();

            var hp = new GameAttributeInt("Hp", 1001, 1001);
            var atk = new GameAttributeInt("Atk", 300, 300);
            var def = new GameAttributeInt("Def", 20, 20);
            var speed = new GameAttributeInt("Speed", 50, 50);
            result.Add(hp);
            result.Add(speed);
            result.Add(atk);
            result.Add(def);

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
