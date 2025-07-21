namespace Tiktok
{
    public class FightDTO 
    {
        public required string LevelNodeUid { get; set; }

        public LevelNodeDTO? LevelNodeDTO { get; set; }
    }
}