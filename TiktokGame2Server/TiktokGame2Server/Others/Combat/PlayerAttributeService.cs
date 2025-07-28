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
            return 1; //to do: 计算等级
        }

        public int GetSex()
        {
            return tiktokConfigService.GetSamuraiSex(samurai.BusinessId);
        }
        public int GetHp()
        {
            return samurai.CurHp; //to do: 从数据库中获取玩家的HP
        }
        public int GetMaxHp()
        {
            var level = GetLevel();
            return 1000; //to do: 计算最大HP
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
