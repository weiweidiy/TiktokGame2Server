namespace TiktokGame2Server.Entities
{
    public class Bag
    {
        public int Id { get; set; }

        /// <summary>
        /// 道具外键
        /// </summary>
        public int ItemId { get; set; }
        public Item? Item { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }
    }
}
