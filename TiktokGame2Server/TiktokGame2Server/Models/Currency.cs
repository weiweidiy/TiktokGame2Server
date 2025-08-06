namespace TiktokGame2Server.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        /// <summary>
        /// 铜币
        /// </summary>
        public int Coin { get; set; }

        /// <summary>
        /// 日本战国时期的一种货币，小判
        /// </summary>
        public int Pan { get; set; }

        public int PlayerId { get; set; }
        public Player? Player { get; set; }

    }
}
