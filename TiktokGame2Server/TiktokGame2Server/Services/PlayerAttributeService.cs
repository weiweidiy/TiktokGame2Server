namespace TiktokGame2Server.Others
{
    public class PlayerAttributeService : IAttributeService
    {
        private readonly int playerId;
        private readonly int samuraiId;
        public PlayerAttributeService(int playerId, int samuraiId)
        {
            this.playerId = playerId;
            this.samuraiId = samuraiId;
        }
        public int GetPower() => 100; // Example value
        public int GetDef() => 50; // Example value
        public int GetInt() => 30; // Example value
        public int GetSpeed() => 20; // Example value
        public int GetLevel() => 1; // Example value
        public int GetSex() => 1; // Example value
        public int GetHp() => 1000; // Example value
        public int GetMaxHp() => 1000; // Example value
        public int GetAttack() => 150; // Example value
        public int GetDefence() => 75; // Example value
    }
}
