using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public abstract class TiktokJCombatAcionsBaseBuilder : IJCombatActionBuilder
    {
        protected TiktokConfigService tiktokConfigService;
        protected IJCombatTurnBasedEventRecorder recorder;
        public TiktokJCombatAcionsBaseBuilder(IJCombatTurnBasedEventRecorder recorder, TiktokConfigService tiktokConfigService)
        {
            this.recorder = recorder;
            this.tiktokConfigService = tiktokConfigService;
        }

        protected abstract List<string> GetActionsBusiness();

        public List<IJCombatAction> Create()
        {
            var result = new List<IJCombatAction>();

            var actionBusinessIds = GetActionsBusiness();

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

                for (int i = 0; i < formulasNames.Length; i++)
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
            if (finderName == null || finderName == "")
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

