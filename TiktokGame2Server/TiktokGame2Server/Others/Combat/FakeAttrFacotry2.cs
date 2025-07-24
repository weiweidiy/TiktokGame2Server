using JFramework;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class FakeAttrFacotry2 : IJCombatUnitAttrFactory
    {
        public List<IUnique> Create()
        {
            var result = new List<IUnique>();

            var hp = new GameAttributeInt("Hp", 200, 200);
            var speed = new GameAttributeInt("Speed", 40, 60);
            result.Add(hp);
            result.Add(speed);

            return result;
        }
    }

    


}


