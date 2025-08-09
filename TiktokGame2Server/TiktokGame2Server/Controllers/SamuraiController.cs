using Microsoft.AspNetCore.Mvc;
using Tiktok;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SamuraiController : Controller
    {
        ITokenService tokenService;
        ISamuraiService samuraiService;
        TiktokConfigService tiktokConfigService;
        public SamuraiController(ITokenService tokenService, ISamuraiService samuraiService, TiktokConfigService tiktokConfigService)
        {
            this.tokenService = tokenService;
            this.samuraiService = samuraiService;
            this.tiktokConfigService = tiktokConfigService;
        }

        [HttpPost("AddExperience")]
        public async Task<IActionResult> AddExperience([FromBody] RequestAddSamuraiExp request)
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            var targetSamuraiId = request.TargetSamuraiId;
            var expSamuraiIds = request.ExpSamuraisIds;
            //检查这些ID是否是当前账号的武将ID
            if (targetSamuraiId <= 0 || expSamuraiIds == null || expSamuraiIds.Count == 0)
            {
                return BadRequest("Invalid request data.");
            }
            //检查武将是否存在
            var targetSamurai = await samuraiService.GetSamuraiAsync(targetSamuraiId);
            if (targetSamurai == null || targetSamurai.PlayerId != playerId)
            {
                return NotFound("Target samurai not found or does not belong to the player.");
            }
            //检查经验武将是否存在
            var expSamuraisValid = await samuraiService.CheckSamurais(expSamuraiIds, playerId);
            if (!expSamuraisValid)
            {
                return NotFound("One or more experience samurai IDs are invalid or do not belong to the player.");
            }

            //获取所有经验值武将的经验值总和
            var totalExp = 0;
            var expSamurais = await samuraiService.GetSamuraisAsync(expSamuraiIds);  
            foreach (var expSamurai in expSamurais)
            {
                totalExp += expSamurai.Experience;
                totalExp += tiktokConfigService.GetSamuraiExpAddValue(expSamurai.BusinessId);
            }

            //添加经验值到目标武将
            var updatedSamurai = await samuraiService.AddSamuraiExperience(targetSamuraiId, totalExp);

            if (updatedSamurai == null)
            {
                return StatusCode(500, "Failed to add experience to the samurai.");
            }

            // 删除经验武将
            await samuraiService.DeleteSamuraisAsync(expSamuraiIds);

            // 返回更新后的武将信息 samuraiDTO
            var samuraiDTO = new SamuraiDTO
            {
                Id = updatedSamurai.Id,

                Level = tiktokConfigService.FormulaLevel(updatedSamurai.Experience),
                Experience = updatedSamurai.Experience,
                CurHp = updatedSamurai.CurHp,
                BusinessId = updatedSamurai.BusinessId,
                SoldierBusinessId = updatedSamurai.SoldierBusinessId,
            };

            // 返回结果
            return Ok(samuraiDTO);

        }
    }
}