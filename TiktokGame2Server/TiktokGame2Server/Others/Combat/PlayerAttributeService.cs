using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class PlayerAttributeService : IAttributeService
    {
        Samurai samurai;
        TiktokConfigService tiktokConfigService;
        public PlayerAttributeService(Samurai samurai, TiktokConfigService tiktokConfigService)
        {
            this.samurai = samurai ?? throw new ArgumentNullException(nameof(samurai));
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }
        public int GetPower()
        {
            return tiktokConfigService.GetSamuraiPower(samurai.BusinessId);
        }
        public int GetDef()
        {
            return tiktokConfigService.GetSamuraiDef(samurai.BusinessId);
        }
        public int GetIntel()
        {
            return tiktokConfigService.GetSamuraiIntel(samurai.BusinessId);
        }

        public int GetSpeed()
        {
            return tiktokConfigService.GetSamuraiSpeed(samurai.BusinessId) + tiktokConfigService.GetSoldierSpeed(samurai.SoldierUid);
        }
        public int GetLevel()
        {
            var experience = samurai.Experience;
            return tiktokConfigService.FormulaLevel(experience);
        }

        public int GetSex()
        {
            return tiktokConfigService.GetSamuraiSex(samurai.BusinessId);
        }
        public int GetHp()
        {
            return samurai.CurHp; 
        }
        public int GetMaxHp()
        {
            var level = GetLevel();
            return tiktokConfigService.FormulaMaxHp(level); 
        }

        public int GetAttack()
        {
            return tiktokConfigService.GetSoldierAttack(samurai.SoldierUid); //+ 其他比如装备
        }
        public int GetDefence()
        {
            return tiktokConfigService.GetSoldierDefence(samurai.SoldierUid); //+ 其他比如装备
        }
    }
}
