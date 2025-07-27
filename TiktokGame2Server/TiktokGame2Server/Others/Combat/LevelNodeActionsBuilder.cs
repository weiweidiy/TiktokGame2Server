using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class LevelNodeActionsBuilder : IJCombatActionBuilder
    {
        string formationUnitBusinessId;
        IJCombatTurnBasedEventRecorder recorder;
        TiktokConfigService tiktokConfigService;
        public LevelNodeActionsBuilder(string formationUnitBusinessId, IJCombatTurnBasedEventRecorder recorder, TiktokConfigService tiktokConfigService) {
            this.formationUnitBusinessId = formationUnitBusinessId;
            this.recorder = recorder;
            this.tiktokConfigService = tiktokConfigService;
        }
        public List<IJCombatAction> Create()
        {
            var result = new List<IJCombatAction>();

            var actionBusinessIds = GetActionsBusiness(formationUnitBusinessId);

            foreach(var actionBusinessId in actionBusinessIds)
            {
                var args = tiktokConfigService.GetActionFormulasArgs(actionBusinessId, 0);
                var finder1 = new JCombatDefaultFinder(null);
                var formula = new TiktokDamageFormula(new float[] { 1});
                var executor1 = new JCombatExecutorDamage(finder1, formula, null);
                var lstExecutor1 = new List<IJCombatExecutor>();
                lstExecutor1.Add(executor1);

                var actionInfo = new TiktokJCombatActionInfo();
                actionInfo.Uid = Guid.NewGuid().ToString();
                actionInfo.ActionBusinessId = actionBusinessId;
                actionInfo.Executors = lstExecutor1;

                var action1 = new JCombatActionBase(actionInfo, recorder);
                result.Add(action1);
            }

            
            return result;
        }

        List<string> GetActionsBusiness(string formationUnitBusinessId)
        {
            var result = new List<string>();

            var soldierBusinessId = tiktokConfigService.GetFormationUnitSoldierBusinessId(formationUnitBusinessId);
            var actions = tiktokConfigService.GetSoldierActions(soldierBusinessId);
            result.AddRange(actions);
            return result;
        }


    }
}

