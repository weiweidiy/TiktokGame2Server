using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class LevelNodeFormationBuilder : JCombatFormationBuilder<TiktokJCombatForamtionInfo, TiktokJCombatUnitInfo>
    {
        public LevelNodeFormationBuilder(string levelNodeBusinessId, IJCombatUnitBuilder<TiktokJCombatUnitInfo> unitBuilder) : base(unitBuilder)
        {

        }

        public override List<TiktokJCombatForamtionInfo> Build()
        {
            //fake data
            var fakeResult = new List<TiktokJCombatForamtionInfo>();

            var formation = new TiktokJCombatForamtionInfo();
            formation.Point = 1;
            formation.UnitInfo = unitBuilder.Build(2); //这里是配置表中的id
            fakeResult.Add(formation);

            var formation2 = new TiktokJCombatForamtionInfo();
            formation2.Point = 2;
            formation2.UnitInfo = unitBuilder.Build(3); //这里是配置表中的id
            fakeResult.Add(formation2);

            return fakeResult;
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
