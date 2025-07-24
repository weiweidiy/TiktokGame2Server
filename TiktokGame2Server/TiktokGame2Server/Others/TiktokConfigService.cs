using JFramework;
using JFramework.Game;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    public class TiktokConfigService : TiktokGenConfigManager
    {
        public TiktokConfigService(IConfigLoader loader, IDeserializer deserializer) : base(loader, deserializer)
        {
            // 可以在这里添加额外的初始化逻辑
        }

        public string GetDefaultSamuraiBusinessId() => "1";

        /// <summary>
        /// 根据武士BusinessId获取默认的SoldierBusinessId
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetDefaultSoldierBusinessId(string samuraiBusinessId)
        {
            var samuraiCfg = Get<SamuraiCfgData>(samuraiBusinessId);
            if (samuraiCfg == null)
            {
                throw new Exception($"SamuraiCfgData not found for businessId: {samuraiBusinessId}");
            }
            return samuraiCfg.SoldierUid;
        }

        public int GetDefaultFormationPoint() => 4;
        public int GetAtkFormationType() => 1;

        public int GetDefFormationType() => 2;

        public string GetDefaultFirstNodeBusinessId() => "1";

        /// <summary>
        /// 判断是否是新关卡的第一个节点
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public bool IsNewLevelFirstNode(string levelNodeBusinessId)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            var preUid = nodeCfgData.PreUid;
            if (preUid == "0")
                return true;
            var preNode = Get<LevelsNodesCfgData>(preUid);
            return preNode.LevelUid != nodeCfgData.LevelUid;

        }

        /// <summary>
        /// 验证关卡节点BusinessId是否有效(存在于配置中)
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public bool IsValidLevelNode(string levelNodeBusinessId)
        {
            return Get<LevelsNodesCfgData>(levelNodeBusinessId) != null;
        }

        /// <summary>
        /// 获取下一个关卡节点的BusinessId列表
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public List<string> GetNextLevelNodes(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            return nodeCfg.NextUid;
        }

        /// <summary>
        /// 获取前置关卡节点的BusinessId
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        public string GetPreviousLevelNode(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            return nodeCfg.PreUid;
        }

        /// <summary>
        /// 获取下一个关卡的BusinessId
        /// </summary>
        /// <param name="levelBusinessId"></param>
        /// <returns></returns>
        public string GetNextLevel(string levelBusinessId)
        {
            var levelCfg = Get<LevelsCfgData>(levelBusinessId);
            return levelCfg.Next;
        }

        /// <summary>
        /// 获取前置关卡的BusinessId
        /// </summary>
        /// <param name="levelBusinessId"></param>
        /// <returns></returns>
        public string GetPreLevel(string levelBusinessId)
        {
            var levelCfg = Get<LevelsCfgData>(levelBusinessId);
            return levelCfg.Pre;
        }

        /// <summary>
        /// 获取关卡节点的阵型配置
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetLevelNodeFormation(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            if (nodeCfg == null)
            {
                throw new Exception($"LevelNodeCfgData not found for businessId: {levelNodeBusinessId}");
            }
            var formationUid = nodeCfg.FormationUid;
            var foramtionCfg = Get<FormationsCfgData>(formationUid);
            return foramtionCfg.UnitsUid.ToArray();
        }
    }
}