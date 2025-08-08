using TiktokGame2Server.Entities;
namespace Tiktok
{
    public class RewardDTO
    {
        public List<CurrencyDTO>? Currencies { get; set; }
        public List<ItemDTO>? BagItems { get; set; }
    }
}
