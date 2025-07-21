/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class LevelsNodesTable : BaseConfigTable<LevelsNodesCfgData>
    {
    }

    public class LevelsNodesCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //关卡uid
        public string LevelUid;

        //关卡序列
        public int NodeIndex;

        //关卡事件
        public string EventType;

        //后置节点
        public List<string> NextUid;

        //前置节点
        public string PreUid;

        //关卡节点Uid
        public string PrefabUid;

        //底座预制体UID
        public string BottomPrefabUid;

        //成就预制体Uid
        public string StarPrefabUid;

    }
}
