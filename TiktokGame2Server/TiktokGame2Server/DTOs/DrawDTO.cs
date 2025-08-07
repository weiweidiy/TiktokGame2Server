namespace Tiktok
{
    public class DrawDTO
    {
        public int Count;
        public CurrencyDTO? Currency { get; set; }
        public List<SamuraiDTO>? SamuraiDTOs { get; set; }
    }
}