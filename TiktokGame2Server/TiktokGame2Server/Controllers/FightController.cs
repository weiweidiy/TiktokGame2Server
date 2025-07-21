using Microsoft.AspNetCore.Mvc;
using Tiktok;
using TiktokGame2Server.Others; // 假设TokenService在此命名空间

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : Controller
    {
        private readonly ILevelNodesService levelNodeService;

        ITokenService tokenService;
        public FightController(ILevelNodesService levelNodeService, ITokenService tokenService)
        {
            this.levelNodeService = levelNodeService;
            this.tokenService = tokenService;
        }

        [HttpPost("Fight")]
        public async Task<IActionResult> Fight([FromBody] FightDTO fightDTO)
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            //需要打的关卡节点ID
            var levelNodeId = fightDTO.LevelNodeId;
            var levelNode = await levelNodeService.LevelNodeVictoryAsync(levelNodeId, playerId);
            //如果levelNode为null，说明关卡节点不存在或未找到
            if (levelNode == null)
            {
                return NotFound("关卡节点不存在或未找到");
            }

            var levelNodeDTO = new LevelNodeDTO
            {
                Uid = levelNode.NodeUid,
                Process = levelNode.Process

            };
               

            fightDTO.LevelNodeDTO = levelNodeDTO;
            return Ok(fightDTO);

        }
    }
}