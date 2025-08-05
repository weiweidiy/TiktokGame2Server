using JFramework;
using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class LevelNodeService : ILevelNodesService
    {
        private readonly MyDbContext _dbContext;
        private readonly TiktokConfigService tiktokConfigService;
        public LevelNodeService(MyDbContext dbContext, TiktokConfigService tiktokConfigService)
        {
            _dbContext = dbContext;
            this.tiktokConfigService = tiktokConfigService;
        }

        public Task<LevelNode?> GetLevelNodeAsync(string levelNodeBusinessId, int playerId)
        {
            // 查找对应的 LevelNode
            var levelNode = _dbContext.LevelNodes
                .FirstOrDefaultAsync(n => n.BusinessId == levelNodeBusinessId && n.PlayerId == playerId);
            return levelNode;

        }

        /// <summary>
        /// 获取玩家的关卡节点信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<List<LevelNode>?> GetLevelNodesAsync(int playerId)
        {
            // 查找对应的 LevelNodes
            var levelNodes = await _dbContext.LevelNodes
                .Where(n => n.PlayerId == playerId)
                .ToListAsync();

            return levelNodes;
        }

        /// <summary>
        /// 玩家通过指定节点胜利（即完成该节点）
        /// </summary>
        /// <param name="levelNodeBusinessId"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<LevelNode> LevelNodeVictoryAsync(string levelNodeBusinessId, int playerId)
        {
            if(!CheckUid(levelNodeBusinessId))
                throw new ArgumentException($"节点 {levelNodeBusinessId} 不存在或无效。");

            // 查找对应的 LevelNode
            var levelNode = _dbContext.LevelNodes.FirstOrDefault(n => n.BusinessId == levelNodeBusinessId && n.PlayerId == playerId);
            if (levelNode == null)
            {
                // to do: 验证前置节点是否完成

                //添加一个新的 LevelNode
                levelNode = new LevelNode
                {
                    BusinessId = levelNodeBusinessId,
                    PlayerId = playerId,
                    Process = 0
                };
                _dbContext.LevelNodes.Add(levelNode);
            }
            //// 更新 LevelNode 的 Process 状态 process小于3，则+1
            //if (levelNode.Process < QueryLevelNodeMaxProcess(levelNodeBusinessId))
            //{
            //    levelNode.Process++;
            //}

            // 保存更改到数据库
            await _dbContext.SaveChangesAsync();

            return levelNode;
        }

        bool CheckUid(string levelNodeBusinessId)
        {
            return tiktokConfigService.IsValidLevelNode(levelNodeBusinessId);
        }

        int QueryLevelNodeMaxProcess(string nodeId)
        {
            //测试数据，临时硬编码
            return 3;
        }
    }
}
