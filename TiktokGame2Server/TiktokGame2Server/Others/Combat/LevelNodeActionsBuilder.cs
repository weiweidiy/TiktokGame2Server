using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class LevelNodeActionsBuilder : IJCombatActionBuilder
    {
        string formationUnitBusinessId;
        public LevelNodeActionsBuilder(string formationUnitBusinessId) {
            this.formationUnitBusinessId = formationUnitBusinessId;
        }
        public List<IJCombatAction> Create()
        {
            var result = new List<IJCombatAction>();

            var finder1 = new JCombatDefaultFinder();
            var formula = new TiktokNormalFormula();
            var executor1 = new JCombatExecutorDamage(finder1, formula);
            var lstExecutor1 = new List<IJCombatExecutor>();
            lstExecutor1.Add(executor1);
            var action1 = new JCombatActionBase(Guid.NewGuid().ToString(), null, lstExecutor1);
            result.Add(action1);
            return result;
        }
    }
}

