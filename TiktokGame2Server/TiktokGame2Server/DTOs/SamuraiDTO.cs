namespace Tiktok
{
    public class SamuraiDTO
    {
        public int Id { get; set; }
        public required string BusinessId { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        //public int PlayerId { get; set; }
    }
}