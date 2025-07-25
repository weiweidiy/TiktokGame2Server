using JFramework.Game;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    public class LevelNodeUnitBuilder : JCombatBaseUnitBuilder
    {
        //string levelNodeBusinessId;
        string formationUnitBusinessId;
        TiktokConfigService tiktokConfigService;
        public LevelNodeUnitBuilder(IJCombatAttrBuilder attrBuilder, IJCombatActionBuilder actionBuilder
            ,string formationUnitBusinessId, /*string levelNodeBusinessId,*/ TiktokConfigService tiktokConfigService) : base(attrBuilder, actionBuilder)
        {
            //this.levelNodeBusinessId = levelNodeBusinessId;
            this.formationUnitBusinessId = formationUnitBusinessId;
            this.tiktokConfigService = tiktokConfigService;
        }

        public override IJCombatUnitInfo Build()
        {
            var unitInfo = new TiktokJCombatUnitInfo
            {
                Uid = Guid.NewGuid().ToString(),
                AttrList = attrBuilder.Create(),
                Actions = actionBuilder.Create(),
                SamuraiBusinessId = GetSamuraiBusinessId(formationUnitBusinessId),
                SoldierBusinessId = GetSamuraiBusinessId(formationUnitBusinessId),
            };

            return unitInfo;
        }

        string GetSamuraiBusinessId(string formationUnitBusinessId)
        {
            return tiktokConfigService.GetFormationUnitSamuraiBusinessId(formationUnitBusinessId);
        }

        string GetSoldierBusinessId(string formationUnitBusinessId)
        {
            return tiktokConfigService.GetFormationUnitSoldierBusinessId(formationUnitBusinessId);
        }
    }
}

