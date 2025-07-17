namespace TiktokGame2Server.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }
    }


}
