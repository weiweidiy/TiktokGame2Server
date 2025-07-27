using JFramework.Game;
using System.Reflection;
using System.Text;

namespace TiktokGame2Server.Others
{

    public class PlayerActionsBuilder : TiktokJCombatAcionsBaseBuilder
    {
        int playerId;
        int samuraiId;

        public PlayerActionsBuilder(int playerId, int samuraiId, IJCombatTurnBasedEventRecorder recorder, TiktokConfigService tiktokConfigService):base(recorder, tiktokConfigService)
        {
            this.playerId = playerId;
            this.samuraiId = samuraiId;
        }

       
        protected override List<string> GetActionsBusiness()
        {
            return GetActionsBusiness(playerId, samuraiId);
        }

        private List<string> GetActionsBusiness(int playerId, int samuraiId)
        {
            var result = new List<string>();

            result.Add("1");
            result.Add("2");
            return result;
        }

       
    }
}

