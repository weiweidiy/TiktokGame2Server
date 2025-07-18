using Microsoft.AspNetCore.Mvc;
using Tiktok;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others; // 假设TokenService在此命名空间

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController(MyDbContext myDbContext, ITokenService tokenService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        ITokenService tokenService = tokenService;

        [HttpPost("EnterGame")]
        public ActionResult<GameDTO> EnterGame([FromQuery] GameDTO gameDTO)
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

            // 查询玩家
            var player = myDbContext.Players
                .Where(p => p.Id == accountUid)
                .Select(p => new Player
                {
                    Id = p.Id,
                    Name = p.Name,
                    Chapters = p.Chapters
                })
                .FirstOrDefault();

            if (player == null)
            {
                return NotFound("玩家不存在");
            }

            // 构造UserDTO
            var userDto = new PlayerDTO
            {
                Uid = player.Id,
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
                //PlayerDTO = userDto,
                //ChapterDTO = chapterDto
            };

            return Ok(gameDto);
        }
    }
}