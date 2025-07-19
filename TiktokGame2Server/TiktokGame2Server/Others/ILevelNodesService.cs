using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public interface ILevelNodesService
    {
        Task<LevelNode?> GetLevelNodeAsync(int levelNodeId, int playerId);
        Task<List<LevelNode>?> GetLevelNodesAsync(int playerId);

        /// <summary>
        /// 指定节点胜利（玩家通过该节点）
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<LevelNode> LevelNodeVictoryAsync(int nodeId, int playerId);

    }
}
