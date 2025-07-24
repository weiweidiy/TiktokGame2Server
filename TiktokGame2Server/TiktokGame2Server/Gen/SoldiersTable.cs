/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class SoldiersTable : BaseConfigTable<SoldiersCfgData>
    {
    }

    public class SoldiersCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //兵种Uid
        public string Soldier;

        //攻击力
        public int Atk;

        //防御力
        public int Def;

        //生命力
        public int Hp;

        //速度
        public int Speed;

    }
}
