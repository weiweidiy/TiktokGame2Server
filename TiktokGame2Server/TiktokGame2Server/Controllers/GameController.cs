using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tiktok;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others; // 假设TokenService在此命名空间

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController(MyDbContext myDbContext, ITokenService tokenService, IPlayerService playerService
        , ILevelNodesService levelNodeService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        ITokenService tokenService = tokenService;
        IPlayerService playerService = playerService;
        ILevelNodesService chapterService = levelNodeService;

        [HttpPost("EnterGame")]
        public async Task<ActionResult<GameDTO>> EnterGame()
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? 0;

            if (string.IsNullOrEmpty(accountUid))
            {
                return BadRequest("账号Uid不能为空");
            }

            // 检查账号是否存在
            var account = await myDbContext.Accounts.Include(a => a.Player).Where(a => a.Uid == accountUid).FirstOrDefaultAsync();
            if (account == null)
            {
                return NotFound("账号不存在");
            }


            // 获取玩家的关卡节点
            var levelNodes = await chapterService.GetLevelNodesAsync(playerId);
            var levelNodeDtos = levelNodes?.Select(n => new LevelNodeDTO
            {
                NodeId = n.NodeUid,
                Process = n.Process,
            }).ToList();


            var gameDto = new GameDTO
            {
                PlayerDTO = new PlayerDTO
                {
                    Uid = playerUid,
                    Username = account.Player?.Name 
                },
                LevelNodesDTO = levelNodeDtos ?? new List<LevelNodeDTO>()
            };

            return Ok(gameDto);
        }
    }
}