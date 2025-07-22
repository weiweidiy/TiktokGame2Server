using TiktokGame2Server.Entities;

namespace Tiktok
{
    public class GameDTO 
    {     
        public PlayerDTO PlayerDTO { get; set; }
        public List<LevelNodeDTO>? LevelNodesDTO { get; set; }

        public List<SamuraiDTO>? SamuraisDTO { get; set; } = new List<SamuraiDTO>();

        public List<FormationDTO>? AtkFormationDTO { get; set; } = new List<FormationDTO>();

        public List<FormationDTO>? DefFormationDTO { get; set; } = new List<FormationDTO>();
    }
}