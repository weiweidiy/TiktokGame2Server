/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class TiktokGenConfigManager : JConfigManager
    {
        public TiktokGenConfigManager(IConfigLoader loader, IDeserializer deserializer) : base(loader)
        {
          RegisterTable<LevelsTable, LevelsCfgData>(nameof(LevelsTable), deserializer);
          RegisterTable<LevelsNodesTable, LevelsNodesCfgData>(nameof(LevelsNodesTable), deserializer);
          RegisterTable<PrefabsTable, PrefabsCfgData>(nameof(PrefabsTable), deserializer);
          RegisterTable<SamuraiTable, SamuraiCfgData>(nameof(SamuraiTable), deserializer);
        }
    }

}
