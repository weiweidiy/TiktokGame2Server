using System.ComponentModel.DataAnnotations;

namespace TiktokGame2Server.Entities
{
    public class Formation
    {
        public int Id { get; set; }

        public int FormationType { get; set; } // 1: 攻阵 2：防阵

        [Range(0, 8, ErrorMessage = "Point必须在0到8之间")]
        public int FormationPoint { get; set; } 

        public int SamuraiId { get; set; }

        public Samurai? Samurai { get; set; } // 外键关联到Samurai实体

        public int PlayerId { get; set; } // 外键，关联玩家
    }
}
