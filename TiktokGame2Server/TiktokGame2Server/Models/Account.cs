namespace TiktokGame2Server.Entities
{
    public class Account
    {
        public int Id { get; set; }

        //public string UserUid { get; set; }

        public string? PlayerId { get; set; }

        public Player? Player { get; set; }
    }


}
