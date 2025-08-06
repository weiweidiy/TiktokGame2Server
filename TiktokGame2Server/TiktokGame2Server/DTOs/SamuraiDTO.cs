namespace Tiktok
{
    public class SamuraiDTO
    {
        public int Id { get; set; }
        public required string BusinessId { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;      
        
        public int CurHp { get; set; }

        //public int MaxHp { get; set; }
    }

    public class BagSlotDTO
    {
        public int Id { get; set; }
        public ItemDTO? ItemDTO { get; set; }
    }

    public class ItemDTO
    {
        public int Id { get; set; }
        public required string ItemBusinessId { get; set; } = string.Empty;
        public int Count { get; set; } = 1;
    }
}