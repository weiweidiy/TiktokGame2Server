namespace Tiktok
{
    public class SamuraiDTO
    {
        public required string Uid { get; set; }
        public required string BusinessId { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;      
        
        public int CurHp { get; set; }

        //public int MaxHp { get; set; }
    }
}