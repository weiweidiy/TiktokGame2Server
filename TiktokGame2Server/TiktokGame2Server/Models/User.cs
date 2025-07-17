namespace TiktokGame2Server.Entities
{

    public class User
    {
        public required string Id { get; set; }
        public string? Name { get; set; }         
        public ICollection<Chapter> Chapters { get; set; }
    }


}
