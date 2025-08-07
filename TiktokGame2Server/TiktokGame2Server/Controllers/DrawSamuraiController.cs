using Microsoft.AspNetCore.Mvc;
using Tiktok;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
   [ApiController]
    [Route("api/[controller]")]
    public class DrawSamuraiController : Controller
    {
        ITokenService tokenService;
        IDrawSamuraiService drawSamuraiService;
        TiktokConfigService tiktokConfigService;
        public DrawSamuraiController(ITokenService tokenService,IDrawSamuraiService drawSamuraiService, TiktokConfigService tiktokConfigService)
        {
            this.tokenService = tokenService;
            this.drawSamuraiService = drawSamuraiService;
            this.tiktokConfigService = tiktokConfigService;
        }

        [HttpPost("Draw")]
        public async Task<IActionResult> Draw([FromBody] RequestDrawSamurai request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            //单抽
            var samurai = await drawSamuraiService.DrawSamurai(playerId);
            var samuraiDTOs = new List<SamuraiDTO>();
            var samuraiDTO = new SamuraiDTO()
            {
                Id = samurai.Id,
                BusinessId = samurai.BusinessId,
                Level = 1,
                Experience = 0,
                CurHp = tiktokConfigService.FormulaMaxHpByLevel(1)
            };
            samuraiDTOs.Add(samuraiDTO);

            var response = new DrawDTO
            {
                SamuraiDTOs = samuraiDTOs
            };
            return Ok(response);
        }
    }
}