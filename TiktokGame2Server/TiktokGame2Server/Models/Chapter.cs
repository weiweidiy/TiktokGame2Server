namespace TiktokGame2Server.Entities
{
    public class Chapter
    {
        public int Id { get; set; }

        public int ChapterId { get; set; }

        public ICollection<ChapterNode>? ChapterNodes { get; set; }

        public ICollection<ChapterNodeStar>? ChapterNodeStars { get; set;}

        public int PlayerId { get; set; }

    }


}
