namespace Tiktok
{
    public class FightDTO 
    {
        public required string LevelNodeId { get; set; }

        public LevelNodeDTO? LevelNodeDTO { get; set; }
    }
}