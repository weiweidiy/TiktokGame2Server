namespace TiktokGame2Server.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public required string ItemBusinessId { get; set; }

        public int Count { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }
    }
}
