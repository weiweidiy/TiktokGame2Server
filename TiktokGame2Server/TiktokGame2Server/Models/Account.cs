namespace TiktokGame2Server.Entities
{
    public class Account
    {
        public int Id { get; set; } 

        public string Uid { get; set; }
        public string? PlayerId { get; set; }

        public Player? Player { get; set; }

        public string Role { get; set; } = "Guest";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


}
