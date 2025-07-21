namespace TiktokGame2Server.Entities
{
    public class Samurai
    {
        public int Id { get; set; }

        public required string Uid { get; set; }

        public int Level { get; set; } = 1;

        public int Experience { get; set; } = 0;

        /// <summary>
        /// 外键，关联玩家
        /// </summary>·
        public int PlayerId { get; set; }
    }


}
