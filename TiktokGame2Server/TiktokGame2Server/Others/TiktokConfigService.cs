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

        #region 默认配置相关
        public string GetDefaultSamuraiBusinessId() => "1";
        public int GetDefaultFormationPoint() => 4;
        public int GetAtkFormationType() => 1;

        public int GetDefFormationType() => 2;

        public string GetDefaultFirstNodeBusinessId() => "1";

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
        #endregion

        #region 关卡相关
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
        #endregion

        #region 关卡节点相关
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
        #endregion

        #region 副本阵型相关
        /// <summary>
        /// 获取阵型单位的武士id
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public string GetFormationUnitSamuraiBusinessId(string formationUnitBusinessId)
        {
            var formationUnitCfg = Get<FormationUnitsCfgData>(formationUnitBusinessId);
            return formationUnitCfg.SamuraiUid;
        }

        /// <summary>
        /// 获取阵型单位的兵种id
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public string GetFormationUnitSoldierBusinessId(string formationUnitBusinessId)
        {
            var formationUnitCfg = Get<FormationUnitsCfgData>(formationUnitBusinessId);
            return formationUnitCfg.SoldierUid;
        }
        #endregion

        #region samurai相关
        /// <summary>
        /// 获取武士的战斗力
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiPower(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Power ?? 0;
        }

        /// <summary>
        /// 获取武士守备力
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiDef(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Def ?? 0;
        }

        /// <summary>
        /// 获取武士的智力
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiInt(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Intel ?? 0;
        }

        /// <summary>
        /// 获取武士的速度
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiSpeed(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId)?.Speed ?? 0;
        }
        #endregion

        #region soldier相关
        /// <summary>
        /// 获取兵种的攻击力
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        public int GetSoldierAttack(string soldierBusinessId)
        {
            return Get<SoldiersCfgData>(soldierBusinessId)?.Atk ?? 0;
        }

        /// <summary>
        /// 获取兵种的防御力
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        public int GetSoldierDefence(string soldierBusinessId)
        {
            return Get<SoldiersCfgData>(soldierBusinessId)?.Def ?? 0;
        }

        /// <summary>
        /// 获取兵种速度
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        public int GetSoldierSpeed(string soldierBusinessId)
        {
            return Get<SoldiersCfgData>(soldierBusinessId)?.Speed ?? 0;
        }

        /// <summary>
        /// 获取兵种的技能列表
        /// </summary>
        /// <param name="soldierBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetSoldierActions(string soldierBusinessId)
        {
            var soldierCfg = Get<SoldiersCfgData>(soldierBusinessId);
            if (soldierCfg == null)
            {
                throw new Exception($"SoldiersCfgData not found for businessId: {soldierBusinessId}");
            }
            return soldierCfg.Actions.ToArray();
        }
        #endregion

        public string[] GetActionTriggersName(string actionBusinessId)
        {
            var actionCfg = Get<ActionsCfgData>(actionBusinessId);
            if (actionCfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            return actionCfg.Triggers.ToArray();
        }

        //public float[] GetActionTriggersArgs(string actionBusinessId, int index)
        //{
        //    var actionCfg = Get<ActionsCfgData>(actionBusinessId);
        //    if (actionCfg == null)
        //    {
        //        throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
        //    }
        //    if (index < 0 || index >= actionCfg.TriggersArgs.Count)
        //    {
        //        throw new IndexOutOfRangeException($"TriggerArgs index {index} is out of range for action {actionBusinessId}");
        //    }
        //    var args = actionCfg.TriggersArgs[index];
        //    return args.ToArray();
        //}

        /// <summary>
        /// 获取指定action的查找者名称
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetActionFinderName(string actionBusinessId)
        {
            var actionCfg = Get<ActionsCfgData>(actionBusinessId);
            if (actionCfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            return actionCfg.Finder;
        }

        /// <summary>
        /// 获取指定action的查找者参数
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[] GetActionFinderArgs(string actionBusinessId)
        {
            var actionCfg = Get<ActionsCfgData>(actionBusinessId);
            if (actionCfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            return new float[] { actionCfg.FinderArgs };
        }

        /// <summary>
        /// 获取指定action的公式名称列表
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetActionFormulasName(string actionBusinessId)
        {
            var actionCfg = Get<ActionsCfgData>(actionBusinessId);
            if (actionCfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            return actionCfg.Formulas.ToArray();
        }

        /// <summary>
        /// 获取指定action的公式参数列表
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public float[] GetActionFormulasArgs(string actionBusinessId, int index)
        {
            var actionCfg = Get<ActionsCfgData>(actionBusinessId);
            if (actionCfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            if (index < 0 || index >= actionCfg.FormulasArgs.Count)
            {
                throw new IndexOutOfRangeException($"FormulaArgs index {index} is out of range for action {actionBusinessId}");
            }

            var args = actionCfg.FormulasArgs[index];
            return args.ToArray();

        }


        /// <summary>
        /// 获取指定action的执行者名称列表
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetActionExecutorsName(string actionBusinessId)
        {
            var actionCfg = Get<ActionsCfgData>(actionBusinessId);
            if (actionCfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            return actionCfg.Executors.ToArray();
        }
    }
}