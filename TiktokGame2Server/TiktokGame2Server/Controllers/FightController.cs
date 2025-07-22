using Microsoft.AspNetCore.Mvc;
using Tiktok;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others; // 假设TokenService在此命名空间

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : Controller
    {
        private readonly ILevelNodesService levelNodeService;
        private readonly ITokenService tokenService;
        private readonly ILevelNodeCombatService levelNodeCombatService;
        TiktokConfigService tiktokConfigService;
        public FightController(ILevelNodesService levelNodeService
                            , ITokenService tokenService
                            , ILevelNodeCombatService levelNodeCombatService
                            , TiktokConfigService tiktokConfigService)
        {
            this.levelNodeService = levelNodeService;
            this.tokenService = tokenService;
            this.levelNodeCombatService = levelNodeCombatService;
            this.tiktokConfigService = tiktokConfigService;
        }

        // 修复 CS8600: 将 null 文本或可能的 null 值转换为不可为 null 类型。
        // 主要是 levelNode 可能为 null，需加上 null 检查。

        [HttpPost("Fight")]
        public async Task<IActionResult> Fight([FromBody] FightDTO fightDTO)
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            //需要打的关卡节点ID
            var levelNodeBusinessId = fightDTO.LevelNodeBusinessId;
            var levelNode = await levelNodeService.GetLevelNodeAsync(levelNodeBusinessId, playerId);

            //如果levelNode为null，说明关卡节点还没有解锁
            if (levelNode == null)
            {
                if(levelNodeBusinessId != tiktokConfigService.GetDefaultFirstNodeBusinessId()) //如果不是默认的初始节点，则检查前置节点是否解锁
                {
                    var previousNodeBusinessId = tiktokConfigService.GetPreviousLevelNode(levelNodeBusinessId);
                    //数据库查询是否存在该节点
                    var previousNode = await levelNodeService.GetLevelNodeAsync(previousNodeBusinessId, playerId);
                    if (previousNode == null)
                    {
                        return BadRequest(new { message = "前置节点未解锁或未完成" });
                    }
                }
            }

            var reportData = await levelNodeCombatService.GetReport(playerId, levelNodeBusinessId);

            var result = true;

            if(result)
            {
                levelNode = await levelNodeService.LevelNodeVictoryAsync(levelNodeBusinessId, playerId);
            }
  
            var levelNodeDTO = new LevelNodeDTO
            {
                BusinessId = levelNodeBusinessId,
                Process = levelNode.Process,
            };

            fightDTO.LevelNodeDTO = levelNodeDTO;
            fightDTO.ReportData = reportData;

            return Ok(fightDTO);

        }
    }
}