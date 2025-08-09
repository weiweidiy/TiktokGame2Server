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

        public int GetDefaultHpPoolHp() => 10000;
        public int GetDefaultHpPoolMaxHp() => 10000;

        public int GetDefaultCurrencyCoin() => 0;

        public int GetDefaultCurrencyPan() => 0;

        public int GetDefaultBagSlotCount() => 20;

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

        public string GetLevelNodeAchievementRewardBusinessId(string levelNodeBusinessId, int process)
        {
            return "1"; // to do: 根据关卡节点BusinessId和process获取奖励BusinessId
        }

        public string GetLevelNodeVictoryRewardBusinessId(string levelNodeBusinessId)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            return "1";// to do: 根据关卡节点BusinessId获取胜利奖励BusinessId
            //return nodeCfgData.WinRewardUid;
        }

        public int GetMaxAchievementProcess(string levelNodeBusinessId)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            var achievements = nodeCfgData.AchievementUid;
            return achievements.Count;
        }

        public string? GetAchievementBusinessId(string levelNodeBusinessId, int process)
        {
            var nodeCfgData = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            var achievements = nodeCfgData.AchievementUid;
            if (achievements == null || achievements.Count == 0)
            {
                return string.Empty; // 没有成就
            }
            if (process < 1 || process > achievements.Count)
            {
                return null;
                //throw new ArgumentOutOfRangeException(nameof(process), "Process must be within the range of achievements.");
            }
            return achievements[process - 1];
        }

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

        /// <summary>
        /// 获取关卡节点的战斗场景BusinessId
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetLevelNodeCombatSceneBusinessId(string levelNodeBusinessId)
        {
            var nodeCfg = Get<LevelsNodesCfgData>(levelNodeBusinessId);
            if (nodeCfg == null)
            {
                throw new Exception($"LevelNodeCfgData not found for businessId: {levelNodeBusinessId}");
            }
            return nodeCfg.CombatSceneUid;

        }

        #endregion

        #region 副本阵型单位相关
        /// <summary>
        /// 获取阵型单位的武士id
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public string GetFormationUnitSamuraiBusinessId(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            return (cfg?.SamuraiUid) ?? Get<FormationUnitsCfgData>(formationUnitBusinessId).SamuraiUid;
        }

        /// <summary>
        /// 获取阵型单位的兵种id
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public string GetFormationUnitSoldierBusinessId(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            return (cfg?.SoldierUid) ?? Get<FormationUnitsCfgData>(formationUnitBusinessId).SoldierUid;
        }

        /// <summary>
        /// 阵型单位的额外攻击力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraAtk(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            return (cfg?.Atk) ?? Get<FormationUnitsCfgData>(formationUnitBusinessId).Atk;
        }

        /// <summary>
        /// 阵型单位的额外防御力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraDefence(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            return (cfg?.Def) ?? Get<FormationUnitsCfgData>(formationUnitBusinessId).Def;
        }

        /// <summary>
        /// 阵型单位的额外速度
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraSpeed(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            return (cfg?.Speed) ?? Get<FormationUnitsCfgData>(formationUnitBusinessId).Speed;
        }

        /// <summary>
        /// 阵型单位的额外生命值
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationeUnitExtraHp(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            return (cfg?.Hp) ?? Get<FormationUnitsCfgData>(formationUnitBusinessId).Hp;
        }

        /// <summary>
        /// 阵型单位的额外等级
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraLevel(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            return (cfg?.Level) ?? Get<FormationUnitsCfgData>(formationUnitBusinessId).Level;
        }

        /// <summary>
        /// 获取阵型单位的性别
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitSex(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var formationUnitCfg = Get<FormationUnitsCfgData>(formationUnitBusinessId);
            return 0;//to do:
        }

        /// <summary>
        /// 获取阵型单位的攻击力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetFormationUnitAttack(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var soldier = GetFormationUnitSoldierBusinessId(formationUnitBusinessId, cfg);
            var atk = GetSoldierAttack(soldier);
            var extraAtk = GetFormationUnitExtraAtk(formationUnitBusinessId, cfg);
            return atk + extraAtk;
        }

        /// <summary>
        /// 获取阵型单位的防御力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitDefence(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var soldier = GetFormationUnitSoldierBusinessId(formationUnitBusinessId, cfg);
            var def = GetSoldierDefence(soldier);
            var extraDef = GetFormationUnitExtraDefence(formationUnitBusinessId, cfg);
            return def + extraDef;
        }

        /// <summary>
        /// 获取阵型单位的速度
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitSpeed(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            var samuraiSpeed = GetSamuraiSpeed(samurai);
            var soldier = GetFormationUnitSoldierBusinessId(formationUnitBusinessId, cfg);
            var soldierSpeed = GetSoldierSpeed(soldier);
            var extraSpeed = GetFormationUnitExtraSpeed(formationUnitBusinessId, cfg);
            return samuraiSpeed + soldierSpeed + extraSpeed;
        }

        /// <summary>
        /// 获取阵型单位的最大生命值
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitMaxHp(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var level = GetFormationUnitExtraLevel(formationUnitBusinessId, cfg);
            var extraHp = GetFormationeUnitExtraHp(formationUnitBusinessId, cfg);
            return FormulaMaxHpByLevel(level) + extraHp;
        }

        /// <summary>
        /// 获取阵型单位的战斗力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitPower(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            return GetSamuraiPower(samurai);
        }

        /// <summary>
        /// 获取阵型单位的守备力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitDef(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            return GetSamuraiDef(samurai);
        }

        /// <summary>
        /// 获取阵型单位的智力
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitIntel(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var samurai = GetFormationUnitSamuraiBusinessId(formationUnitBusinessId, cfg);
            return GetSamuraiIntel(samurai);
        }

        #endregion

        #region samurai相关
        public bool IsValidSamurai(string samuraiBusinessId)
        {
            return Get<SamuraiCfgData>(samuraiBusinessId) != null;
        }

        /// <summary>
        /// 获取武将作为经验武将时，增加的经验值
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetSamuraiExpAddValue(string businessId)
        {
            return 100; // to do: 读取配置
        }

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
        public int GetSamuraiIntel(string samuraiBusinessId)
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

        /// <summary>
        /// 获取武士的性别
        /// </summary>
        /// <param name="samuraiBusinessId"></param>
        /// <returns></returns>
        public int GetSamuraiSex(string samuraiBusinessId)
        {
            return 0; // to do: 读取配置
        }

        /// <summary>
        /// 根据武士等级，获取武士解锁的action列表
        /// </summary>
        /// <param name="level"></param>
        /// <param name="smuraiBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<string> GetSamuraiActions(int level, string smuraiBusinessId)
        {
            var result = new List<string>();
            //to do: 根据武士等级和武士BusinessId获取武士解锁的action列表
            //result.Add("1");
            //result.Add("2");
            return result;
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

        #region action相关

        /// <summary>
        /// 获取触发器名字
        /// </summary>
        /// <param name="triggerBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetTriggerName(string triggerBusinessId)
        {
            var triggerCfg = Get<ActionTriggersCfgData>(triggerBusinessId);
            if (triggerCfg == null)
            {
                throw new Exception($"ActionsTriggersCfgData not found for businessId: {triggerBusinessId}");
            }
            return triggerCfg.Name;
        }

        /// <summary>
        /// 获取触发器中的查找器uid
        /// </summary>
        /// <param name="triggerBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string? GetTriggerFinderUid(string triggerBusinessId)
        {
            var triggerCfg = Get<ActionTriggersCfgData>(triggerBusinessId);
            if (triggerCfg == null)
            {
                throw new Exception($"ActionsTriggersCfgData not found for businessId: {triggerBusinessId}");
            }
            return triggerCfg.FinderUid == "" ? null : triggerCfg.FinderUid;
        }

        /// <summary>
        /// 获取查找器名字
        /// </summary>
        /// <param name="finderBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetFinderName(string finderBusinessId)
        {
            var finderCfg = Get<ActionFindersCfgData>(finderBusinessId);
            if (finderCfg == null)
            {
                throw new Exception($"ActionsFindersCfgData not found for businessId: {finderBusinessId}");
            }
            return finderCfg.Name;
        }

        /// <summary>
        /// 获取执行器名字
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetExecutorName(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.Name;
        }

        /// <summary>
        /// 获取执行器参数
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[] GetExecutorArgs(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.Args.ToArray();
        }

        /// <summary>
        /// 获取执行器过滤器的名字
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string? GetExecutorFilterName(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.FilterName == "" ? null : executorCfg.FilterName;
        }

        /// <summary>
        /// 获取执行器过滤器的参数
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[]? GetExecutorFilterArgs(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.FilterArgs.ToArray();
        }

        /// <summary>
        /// 获取执行器公式名字
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetExecutorFormulaName(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }

            //公式名字不能为空
            if (executorCfg.FormulaName == "")
            {
                throw new Exception($"ActionsExecutorsCfgData FormulaName is empty for businessId: {executorBusinessId}");
            }

            return executorCfg.FormulaName;
        }

        /// <summary>
        /// 获取执行器公式参数
        /// </summary>
        /// <param name="executorBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float[]? GetExecutorFormulaArgs(string executorBusinessId)
        {
            var executorCfg = Get<ActionExecutorsCfgData>(executorBusinessId);
            if (executorCfg == null)
            {
                throw new Exception($"ActionsExecutorsCfgData not found for businessId: {executorBusinessId}");
            }
            return executorCfg.FormulaArgs.ToArray();
        }

        /// <summary>
        /// 获取action触发器Uid列表
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[]? GetActionTriggersUid(string actionBusinessId, ActionsCfgData cfg = null)
        {
            if (cfg == null)
            {
                cfg = Get<ActionsCfgData>(actionBusinessId);
            }
            if (cfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }
            var triggersUid = cfg.TriggersUid;
            if (triggersUid == null || triggersUid.Count == 0)
            {
                return null;
            }

            return triggersUid.ToArray();
        }



        /// <summary>
        /// 获取指定action的查找者名称
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string? GetActionFinderUid(string actionBusinessId, ActionsCfgData cfg = null)
        {
            if (cfg == null)
            {
                cfg = Get<ActionsCfgData>(actionBusinessId);
            }

            if (cfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }

            return cfg.FinderUid == "" ? null : cfg.FinderUid;
        }

        /// <summary>
        /// 获取指定action的执行者uid列表
        /// </summary>
        /// <param name="actionBusinessId"></param>
        /// <param name="index"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string[] GetActionExecutorsUid(string actionBusinessId, ActionsCfgData cfg = null)
        {
            if (cfg == null)
            {
                cfg = Get<ActionsCfgData>(actionBusinessId);
            }
            if (cfg == null)
            {
                throw new Exception($"ActionsCfgData not found for businessId: {actionBusinessId}");
            }

            var executorsUid = cfg.ExecutorsUid;
            if (executorsUid == null || executorsUid.Count == 0)
            {
                throw new Exception($"No executors found for action: {actionBusinessId}");
            }

            return executorsUid.ToArray();
        }

        #endregion

        #region 公式相关
        /// <summary>
        /// 计算最大血量
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int FormulaMaxHpByLevel(int level)
        {
            return (int)(1000 * (1 + level / 10f)); //to do: 计算最大HP
        }

        /// <summary>
        /// 根据经验值计算等级
        /// </summary>
        /// <param name="experience"></param>
        /// <returns></returns>
        public int FormulaLevel(int experience)
        {
            //to do: 计算等级
            return (int)(experience / 100f) + 1;
        }

        #endregion

        #region 成就相关
        public string GetAchievementClassName(string achievementBusinessId)
        {
            var achData = Get<AchievementsCfgData>(achievementBusinessId);
            return achData.Name;
        }

        public float[] GetAchievementArgs(string achievementBusinessId)
        {
            var achData = Get<AchievementsCfgData>(achievementBusinessId);
            return achData.Args.ToArray();
        }



        #endregion

        #region 道具相关
        public int GetItemMaxCount(string itemBusinessId)
        {
            return 99;
        }


        #endregion

        #region 奖励相关
        public CurrencyType[]? GetRewardCurrenciesTypes(string rewardBusinessId)
        {
            return new CurrencyType[] { CurrencyType.Coin }; // to do: 读取配置 ， 暂时就给铜钱ss
        }

        public int[]? GetRewardCurrenciesCounts(string rewardBusinessId)
        {
            return new int[] { 100 }; // to do: 读取配置 ， 暂时就给100铜钱
        }

        public string[]? GetRewardItemsBusinessIds(string rewardBusinessId)
        {
            return new string[] { "1" }; // to do: 读取配置 ， 暂时就给1号道具
        }

        public int[]? GetRewardItemsCounts(string rewardBusinessId)
        {
            return new int[] { 1 }; // to do: 读取配置 ， 暂时就给1个1号道具
        }
        #endregion

        #region 抽卡池相关
        public string[] GetSamuraiDrawPool()
        {
            return new string[] { "1", "2", "3" }; // to do: 读取配置
        }

        public (ResourceType, string, int) GetDrawCost(int poolType, int count)
        {
            return (ResourceType.Currency, "1", 100 * count); // to do: ���取配置，暂时就给100铜钱
        }



        #endregion
    }
}