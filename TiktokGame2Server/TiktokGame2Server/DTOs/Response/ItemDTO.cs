namespace Tiktok
{
    public class ItemDTO
    {
        public required string Uid { get; set; }
        public required string ItemBusinessId { get; set; } = string.Empty;
        public int Count { get; set; } = 1;
        public int BagSlotId { get; set; }
    }
}