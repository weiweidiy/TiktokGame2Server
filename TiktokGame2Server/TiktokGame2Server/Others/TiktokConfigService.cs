using JFramework;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class TiktokConfigService : TiktokGenConfigManager
    {
        public TiktokConfigService(IConfigLoader loader, IDeserializer deserializer) : base(loader, deserializer)
        {
            // 可以在这里添加额外的初始化逻辑
        }
    }
}