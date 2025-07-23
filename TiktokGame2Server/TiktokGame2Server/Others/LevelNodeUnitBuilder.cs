using JFramework;
using JFramework.Game;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    public class LevelNodeUnitBuilder : JCombatBaseUnitBuilder
    {
        public LevelNodeUnitBuilder(IJCombatAttrBuilder attrBuilder, IJCombatActionBuilder actionBuilder) : base(attrBuilder, actionBuilder)
        {
        }
    }

    public class LevelNodeAttrBuilder : IJCombatAttrBuilder
    {
        public List<IUnique> Create(int key)
        {
            return new FakeAttrFacotry2().Create();
        }
    }

    public class LevelNodeActionsBuilder : IJCombatActionBuilder
    {
        public List<IJCombatAction> Create(int key)
        {
            var result = new List<IJCombatAction>();

            var finder1 = new JCombatDefaultFinder();
            var executor1 = new JCombatExecutorDamage(finder1);
            var lstExecutor1 = new List<IJCombatExecutor>();
            lstExecutor1.Add(executor1);
            var action1 = new TiktokJCombatAction(Guid.NewGuid().ToString(), lstExecutor1);
            result.Add(action1);
            return result;
        }
    }
}

