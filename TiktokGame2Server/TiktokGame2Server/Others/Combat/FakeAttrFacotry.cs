using JFramework;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class FakeAttrFacotry : IJCombatUnitAttrFactory
    {
        public List<IUnique> Create()
        {
            var result = new List<IUnique>();

            var hp = new GameAttributeInt("Hp", 100, 100);
            var speed = new GameAttributeInt("Speed", 50, 50);
            result.Add(hp);
            result.Add(speed);

            return result;
        }
    }

    


}


