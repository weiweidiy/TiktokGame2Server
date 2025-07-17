namespace TiktokGame2Server.Entities
{
    public class ChapterNodeStar
    {
        public int Id { get; set; }

        public int Process { get; set; }

        public int ChapterNodeId { get; set; }
        public ChapterNode? ChapterNode { get; set; }
    }


}
