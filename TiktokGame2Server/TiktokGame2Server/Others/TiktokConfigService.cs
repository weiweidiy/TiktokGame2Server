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
            var formationUnitCfg = Get<FormationUnitsCfgData>(formationUnitBusinessId);
            return 0; //to do:读取配置
        }

        /// <summary>
        /// 阵型单位的额外等级
        /// </summary>
        /// <param name="formationUnitBusinessId"></param>
        /// <returns></returns>
        public int GetFormationUnitExtraLevel(string formationUnitBusinessId, FormationUnitsCfgData cfg = null)
        {
            var formationUnitCfg = Get<FormationUnitsCfgData>(formationUnitBusinessId);
            return 1;//to do:读取配置
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
            var speed = GetSamuraiSpeed(samurai);
            var extraSpeed = GetFormationUnitExtraSpeed(formationUnitBusinessId, cfg);
            return speed + extraSpeed;
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
            return FormulaMaxHp(level) + extraHp;
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
            result.Add("1");
            result.Add("2");
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

        #endregion

        #region 公式相关
        /// <summary>
        /// 计算最大血量
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int FormulaMaxHp(int level)
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
            return (int)(Math.Sqrt(experience / 100f)) + 1;
        }


        #endregion
    }
}