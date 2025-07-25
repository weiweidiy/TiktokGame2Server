using JFramework;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class LevelNodeAttrBuilder : IJCombatAttrBuilder
    {
        public LevelNodeAttrBuilder(string formationUnitBusinessId) { 
        
        }
        public List<IUnique> Create()
        {
            var result = new List<IUnique>();

            var hp = new GameAttributeInt("Hp", 500, 500);
            var atk = new GameAttributeInt("Atk", 70, 70);
            var def = new GameAttributeInt("Def", 10, 10);
            var speed = new GameAttributeInt("Speed", 50, 50);
            result.Add(hp);
            result.Add(speed);
            result.Add(atk);
            result.Add(def);

            return result;
        }
    }
}

