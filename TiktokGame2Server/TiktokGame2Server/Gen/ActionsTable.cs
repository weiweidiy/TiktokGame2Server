/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class ActionsTable : BaseConfigTable<ActionsCfgData>
    {
    }

    public class ActionsCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //触发器
        public List<string> Triggers;

        //查找器
        public string Finder;

        //查找器参数
        public float FinderArgs;

        //公示计算器
        public List<string> Formulas;

        //执行器参数
        public int FormulasArgs;

        //执行器
        public List<string> Executors;

    }
}
