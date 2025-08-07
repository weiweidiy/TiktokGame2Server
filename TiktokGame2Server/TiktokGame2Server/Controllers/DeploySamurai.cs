using Microsoft.AspNetCore.Mvc;
using Tiktok;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeploySamurai : Controller
    {
        ITokenService tokenService;
        IFormationService formationService;
        TiktokConfigService tiktokConfigService;
        public DeploySamurai(ITokenService tokenService, TiktokConfigService tiktokConfigService, IFormationService formationService)
        {
            this.tokenService = tokenService;
            this.tiktokConfigService = tiktokConfigService;
            this.formationService = formationService;
        }

        [HttpPost("Deploy")]
        public async Task<IActionResult> Deploy([FromBody] DeployDTO deployDTO)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            if (deployDTO.FormationAtkDTO != null && deployDTO.FormationAtkDTO.Count > 0)
            {
                var formationAtk = await formationService.UpdateFormationAsync(FormationType.FormationAtk, deployDTO.FormationAtkDTO, playerId);
            }

            if (deployDTO.FormationDefDTO != null && deployDTO.FormationDefDTO.Count > 0)
            {
                var formationAtk = await formationService.UpdateFormationAsync(FormationType.FormationDef, deployDTO.FormationDefDTO, playerId);
            }

            return Ok(deployDTO);
        }

    }
}

//更新规则：如果formationNewDataList中的数据在formationDataList中不存在，则添加；如果存在，则更新，如果数据库中有的数据在formationNewDataList中不存在，则删除 (用playerId和formationType和formationPoint作为条件进行更新)
