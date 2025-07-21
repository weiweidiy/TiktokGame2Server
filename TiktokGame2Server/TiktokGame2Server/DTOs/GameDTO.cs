using TiktokGame2Server.Entities;

namespace Tiktok
{
    public class GameDTO 
    {     
        public PlayerDTO PlayerDTO { get; set; }
        public List<LevelNodeDTO>? LevelNodesDTO { get; set; }

        public List<Samurai>? SamuraisDTO { get; set; } = new List<Samurai>();
    }
}