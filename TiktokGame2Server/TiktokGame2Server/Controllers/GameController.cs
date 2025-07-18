using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tiktok;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others; // 假设TokenService在此命名空间

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController(MyDbContext myDbContext, ITokenService tokenService, IPlayerService playerService
        , IChapterService chapterService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        ITokenService tokenService = tokenService;
        IPlayerService playerService = playerService;
        IChapterService chapterService = chapterService;

        [HttpPost("EnterGame")]
        public async Task<ActionResult<GameDTO>> EnterGame([FromQuery] GameDTO gameDTO)
        {
            var accountDto = gameDTO.AccountDTO;
            var accountUid = accountDto?.Uid;
            var token = accountDto?.Token;

            // 获取请求头中的token
            //var token = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(token) || !tokenService.ValidateToken(token))
            {
                return Unauthorized("Token无效或未提供");
            }

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

            // 检查账号是否已绑定玩家
            var player = account.Player;
            if (player == null)
            {
                player = await playerService.CreatePlayerAsync(Guid.NewGuid().ToString(), accountUid, account.Id);
                await chapterService.CreateChaptersAsync(player.Id); // 创建章节

            }

            // 构造UserDTO
            var playerDto = new PlayerDTO
            {
                Uid = player.Uid,
                Username = player.Name
            };

            // 构造ChapterDTO（假设ChapterDTO有合适的构造方式）
            var chapterDto = new ChapterDTO
            {
                // 这里根据你的ChapterDTO结构填充
                // 例如：Chapters = player.Chapters.Select(...)
            };

            var gameDto = new GameDTO
            {
                AccountDTO = accountDto,
                PlayerDTO = playerDto,
                //ChapterDTO = chapterDto
            };

            return Ok(gameDto);
        }
    }
}