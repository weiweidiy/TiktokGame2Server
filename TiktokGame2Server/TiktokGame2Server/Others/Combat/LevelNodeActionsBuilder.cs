using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class LevelNodeActionsBuilder : IJCombatActionBuilder
    {
        string formationUnitBusinessId;
        IJCombatTurnBasedEventRecorder recorder;
        public LevelNodeActionsBuilder(string formationUnitBusinessId, IJCombatTurnBasedEventRecorder recorder) {
            this.formationUnitBusinessId = formationUnitBusinessId;
            this.recorder = recorder;
        }
        public List<IJCombatAction> Create()
        {
            var result = new List<IJCombatAction>();

            var actionBusinessIds = GetActionsBusiness(formationUnitBusinessId);

            foreach(var actionBusinessId in actionBusinessIds)
            {
                var finder1 = new JCombatDefaultFinder();
                var formula = new TiktokNormalFormula();
                var executor1 = new JCombatExecutorDamage(finder1, formula);
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

            result.Add("Action1");

            return result;
        }
    }
}

