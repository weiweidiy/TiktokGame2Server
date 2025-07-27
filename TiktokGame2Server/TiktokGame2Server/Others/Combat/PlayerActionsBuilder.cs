using JFramework.Game;
using System.Reflection;

namespace TiktokGame2Server.Others
{
    public class PlayerActionsBuilder : IJCombatActionBuilder
    {
        int playerId;
        int samuraiId;
        TiktokConfigService tiktokConfigService;
        IJCombatTurnBasedEventRecorder recorder;
        public PlayerActionsBuilder(int playerId, int samuraiId, IJCombatTurnBasedEventRecorder recorder, TiktokConfigService tiktokConfigService)
        {
            this.playerId = playerId;
            this.samuraiId = samuraiId;
            this.recorder = recorder;
            this.tiktokConfigService = tiktokConfigService;
        }

        public List<IJCombatAction> Create()
        {
            var result = new List<IJCombatAction>();

            var actionBusinessIds = GetActionsBusiness(playerId, samuraiId);

            foreach (var actionBusinessId in actionBusinessIds)
            {
                var lstTriggers = new List<IJCombatTrigger>();
                var triggersNames = GetTriggersName(actionBusinessId);
                foreach (var triggerName in triggersNames)
                {
                    var trigger = CreateTrigger(triggerName, null);
                    lstTriggers.Add(trigger);
                }


                var lstExecutors = new List<IJCombatExecutor>();

                var finder = CreateFinder(GetFinderName(actionBusinessId), null);

                var formulasNames = GetFormulasName(actionBusinessId);
                var executorNames = GetExecutorsNames(actionBusinessId);

                for(int i = 0; i < formulasNames.Length; i++)
                {
                    var formulaName = formulasNames[i];
                    var formulaArgs = GetFormulasArgs(actionBusinessId, i);
                    if (formulaName == null || formulaName == "")
                    {
                        throw new ArgumentException("Formula name cannot be null or empty", nameof(formulaName));
                    }
                    var formula = CreateFormula(formulaName, formulaArgs);

                    var executorName = executorNames.Length > i ? executorNames[i] : null;
                    var executor = CreateExecutor(executorName, finder, formula, null);

                    lstExecutors.Add(executor);
                }     

                var actionInfo = new TiktokJCombatActionInfo();
                actionInfo.Uid = Guid.NewGuid().ToString();
                actionInfo.ActionBusinessId = actionBusinessId;
                actionInfo.Executors = lstExecutors;
                actionInfo.Triggers = lstTriggers; // 这里可以添加触发器，如果有的话

                var action1 = new JCombatActionBase(actionInfo, recorder);
                result.Add(action1);
            }

            return result;
        }

        private List<string> GetActionsBusiness(int playerId, int samuraiId)
        {
            var result = new List<string>();

            result.Add("1");
            result.Add("2");
            return result;
        }

        string[] GetTriggersName(string actionBusinessId)
        {
            return tiktokConfigService.GetActionTriggersName(actionBusinessId);
        }

        string GetFinderName(string actionBusinessId)
        {
            return tiktokConfigService.GetActionFinderName(actionBusinessId);
        }

        string[] GetFormulasName(string actionBusinessId)
        {
            return tiktokConfigService.GetActionFormulasName(actionBusinessId);
        }

        float[] GetFormulasArgs(string actionBusinessId, int index)
        {
            return tiktokConfigService.GetActionFormulasArgs(actionBusinessId, index);
        }

        string[] GetExecutorsNames(string actionBusinessId)
        {
            return tiktokConfigService.GetActionExecutorsName(actionBusinessId);
        }


        IJCombatTrigger CreateTrigger(string triggerName, float[] args)
        {
            if (triggerName == null || triggerName == "")
            {
                return null;
            }
            // 假设 args 已经定义
            object[] ctorArgs = new object[] { args };
            return (IJCombatTrigger)TypeHelper.CreateInstanceByClassName(triggerName, ctorArgs);
        }

        IJCombatTargetsFinder CreateFinder(string finderName, float[] args)
        {
            if(finderName == null || finderName == "")
            {
                return null;
            }
            // 假设 args 已经定义
            object[] ctorArgs = new object[] { args };
            return (IJCombatTargetsFinder)TypeHelper.CreateInstanceByClassName(finderName, ctorArgs);
        }

        IJCombatFormula CreateFormula(string formulaName, float[] args)
        {
            if (formulaName == null || formulaName == "")
            {
                throw new ArgumentException("Formula name cannot be null or empty", nameof(formulaName));
            }
            // 假设 args 已经定义
            object[] ctorArgs = new object[] { args };
            return (IJCombatFormula)TypeHelper.CreateInstanceByClassName(formulaName, ctorArgs);
        }

        IJCombatExecutor CreateExecutor(string executorName, IJCombatTargetsFinder finder, IJCombatFormula formula, float[] args)
        {
            if (executorName == null || executorName == "")
            {
                throw new ArgumentException("Executor name cannot be null or empty", nameof(executorName));
            }
            object[] ctorArgs = new object[] { finder, formula, args };
            return (IJCombatExecutor)TypeHelper.CreateInstanceByClassName(executorName, ctorArgs);
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
