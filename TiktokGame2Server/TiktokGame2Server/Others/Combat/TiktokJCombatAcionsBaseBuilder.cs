﻿using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public abstract class TiktokJCombatAcionsBaseBuilder : IJCombatActionBuilder
    {
        protected TiktokConfigService tiktokConfigService;
        protected IJCombatContext context;
        public TiktokJCombatAcionsBaseBuilder(TiktokConfigService tiktokConfigService, IJCombatContext context)
        {

            this.tiktokConfigService = tiktokConfigService;
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected abstract List<string> GetActionsBusiness();

        public List<IJCombatAction> Create()
        {
            var result = new List<IJCombatAction>();

            var actionBusinessIds = GetActionsBusiness();

            foreach (var actionBusinessId in actionBusinessIds)
            {
                //构造触发器
                var lstTriggers = new List<IJCombatTrigger>();
                
                var actionTriggerUid = GetActionTriggersUid(actionBusinessId);
                if(actionTriggerUid != null )
                {
                    foreach (var triggerUid in actionTriggerUid)
                    {
                        IJCombatTargetsFinder triggerFinder = null;
                        var triggerFinderUid = GetTriggerFinderUid(triggerUid);
                        if(triggerFinderUid != null)
                        {
                            var triggerFinderName = GetFinderName(triggerFinderUid);
                            triggerFinder = CreateFinder(triggerFinderName, null); //finder暂时没有参数
                        }
                        
                        var triggerName = GetTriggerName(triggerUid);
                        var trigger = CreateTrigger(triggerName, null, triggerFinder); //触发器暂时没有参数
                        lstTriggers.Add(trigger);
                    }
                }


                //构造finder
                var actionFinderUid = GetActionFinderUid(actionBusinessId);
                IJCombatTargetsFinder? actionFinder = null;
                if (actionFinderUid != null )
                {
                    actionFinder = CreateFinder(GetFinderName(actionFinderUid), null); //finder暂时没有参数
                }


                //构造执行器
                var lstExecutors = new List<IJCombatExecutor>();
                var actionExecutorsUid = GetActionExecutorsUid(actionBusinessId);
                foreach(var executorUid in actionExecutorsUid)
                {
                    IJCombatFilter? filter = null;
                    var executorFilterName = GetExecutorFilterName(executorUid);
                    if(executorFilterName != null)
                    {
                        var executorFilterArgs = GetExecutorFilterArgs(executorUid);
                        filter = CreateFilter(executorFilterName, executorFilterArgs);
                    }

                    IJCombatFormula formula = null;
                    var executorFormulaName = GetExecutorFormulaName(executorUid);
                    var executorFormulaArgs = GetExecutorFormulaArgs(executorUid);
                    formula = CreateFormula(executorFormulaName, executorFormulaArgs);

                    IJCombatExecutor executor = null;
                    var executeName = GetExecutorName(executorUid);
                    var executorArgs = GetExecutorArgs(executorUid);
                    executor = CreateExecutor(executeName, filter, null, formula, executorArgs);

                    lstExecutors.Add(executor);
                }

                var actionInfo = new TiktokJCombatActionInfo();
                actionInfo.Uid = Guid.NewGuid().ToString();
                actionInfo.ActionBusinessId = actionBusinessId;
                actionInfo.Triggers = lstTriggers; // 这里可以添加触发器，如果有的话
                actionInfo.Finder = actionFinder;
                actionInfo.Executors = lstExecutors;

                var action1 = new JCombatActionBase(actionInfo, context);
                result.Add(action1);
            }

            return result;
        }



        string[]? GetActionTriggersUid(string actionBusinessId)
        {
            return tiktokConfigService.GetActionTriggersUid(actionBusinessId);
        }

        string? GetActionFinderUid(string actionBusinessId)
        {
            return tiktokConfigService.GetActionFinderUid(actionBusinessId);
        }

        string[] GetActionExecutorsUid(string actionBusinessId)
        {
            return tiktokConfigService.GetActionExecutorsUid(actionBusinessId);
        }



        string GetTriggerName(string triggerBusinessId)
        {
            return tiktokConfigService.GetTriggerName(triggerBusinessId);
        }

        string? GetTriggerFinderUid(string triggerBusinessId)
        {
            return tiktokConfigService.GetTriggerFinderUid(triggerBusinessId);
        }

        string GetFinderName(string finderBusinessId)
        {
            return tiktokConfigService.GetFinderName(finderBusinessId);
        }

        string GetExecutorName(string executorBusinessId) {
            return tiktokConfigService.GetExecutorName(executorBusinessId);
        }

        float[]? GetExecutorArgs(string executorBusinessId)
        {
            return tiktokConfigService.GetExecutorArgs(executorBusinessId);
        }

        string? GetExecutorFilterName(string executorBusinessId) {

            return tiktokConfigService.GetExecutorFilterName(executorBusinessId);
        }

        float[]? GetExecutorFilterArgs(string executorBusinessId)
        {
            return tiktokConfigService.GetExecutorFilterArgs(executorBusinessId);
        }

        string GetExecutorFormulaName(string executorBusinessId) {
        
            return tiktokConfigService.GetExecutorFormulaName(executorBusinessId);
        }

        float[]? GetExecutorFormulaArgs(string executorBusinessId)
        {
            return tiktokConfigService.GetExecutorFormulaArgs(executorBusinessId);
        }

   

        IJCombatTrigger CreateTrigger(string triggerName, float[] args, IJCombatTargetsFinder finder)
        {
            if (triggerName == null || triggerName == "")
            {
                return null;
            }
            // 假设 args 已经定义
            object[] ctorArgs = new object[] { args, finder };
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

        IJCombatFilter CreateFilter(string? filterName, float[]? args)
        {
            if (filterName == null || filterName == "")
            {
                return null;
            }
            // 假设 args 已经定义
            object[] ctorArgs = new object[] { args };
            return (IJCombatFilter)TypeHelper.CreateInstanceByClassName(filterName, ctorArgs);

        }

        IJCombatFormula CreateFormula(string? formulaName, float[]? args)
        {
            if (formulaName == null || formulaName == "")
            {
                throw new ArgumentException("Formula name cannot be null or empty", nameof(formulaName));
            }
            // 假设 args 已经定义
            object[] ctorArgs = new object[] { args };
            return (IJCombatFormula)TypeHelper.CreateInstanceByClassName(formulaName, ctorArgs);
        }

        IJCombatExecutor CreateExecutor(string executorName, IJCombatFilter filter, IJCombatTargetsFinder finder, IJCombatFormula formula, float[] args)
        {
            if (executorName == null || executorName == "")
            {
                throw new ArgumentException("Executor name cannot be null or empty", nameof(executorName));
            }
            object[] ctorArgs = new object[] { filter, finder, formula, args };
            return (IJCombatExecutor)TypeHelper.CreateInstanceByClassName(executorName, ctorArgs);
        }
    }
}

