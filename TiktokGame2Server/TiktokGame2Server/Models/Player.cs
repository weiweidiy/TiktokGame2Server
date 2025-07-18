namespace TiktokGame2Server.Entities
{

    public class Player
    {
        public int Id { get; set; }
        public string Uid { get; set; } 
        public string? Name { get; set; }         

        public ICollection<Chapter> Chapters { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }


}
