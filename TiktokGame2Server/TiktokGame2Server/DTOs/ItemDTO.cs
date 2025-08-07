namespace Tiktok
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public required string ItemBusinessId { get; set; } = string.Empty;
        public int Count { get; set; } = 1;
    }
}