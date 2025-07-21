namespace TiktokGame2Server.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class LevelNode
    {
        public int Id { get; set; }

        public required string NodeUid { get; set; }

        public int Process { get; set; }

        public int PlayerId { get; set; }

    }


}
