using JFramework.Game;

namespace TiktokGame2Server.Others
{
    /// <summary>
    /// 常用的伤害计算公式，攻击-防御
    /// </summary>
    public class TiktokNormalFormula : JCombatFormula
    {
        public override int CalcHitValue(IJAttributeableUnit target)
        {
            //伤害= 释放者攻击力 * 释放者Power - 目标防御力 * 目标Def
            var caster = query.GetUnit(GetOwner().GetCaster());
            var atk = caster.GetAttribute("Atk") as GameAttributeInt;
            var targetDef = target.GetAttribute("Def") as GameAttributeInt;
            return atk.CurValue - targetDef.CurValue;
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
