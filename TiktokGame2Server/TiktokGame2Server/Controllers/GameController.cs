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
    public class GameController(MyDbContext myDbContext
        , ITokenService tokenService
        , IPlayerService playerService
        , ILevelNodesService levelNodeService
        , ISamuraiService samuraiService
        , IFormationService formationService
        , TiktokConfigService tiktokConfigService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        ITokenService tokenService = tokenService;
        IPlayerService playerService = playerService;
        ILevelNodesService levelNodeService = levelNodeService;
        ISamuraiService samuraiService = samuraiService;
        IFormationService formationService = formationService;
        TiktokConfigService tiktokConfigService = tiktokConfigService;

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


            //游戏登录数据汇总对象
            var gameDto = new GameDTO
            {
                PlayerDTO = new PlayerDTO
                {
                    Uid = playerUid,
                    Username = account.Player?.Name
                },
                LevelNodesDTO = await GetLevelNodeDTOs(playerId),
                SamuraisDTO = await GetSamuraiDTOs(playerId),
                AtkFormationDTO = await GetFormationDTOs(playerId, tiktokConfigService.GetAtkFormationType()),
                DefFormationDTO = await GetFormationDTOs(playerId, tiktokConfigService.GetDefFormationType()),
            };

            return Ok(gameDto);
        }

        /// <summary>
        /// 获取玩家的关卡节点信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        async Task<List<LevelNodeDTO>> GetLevelNodeDTOs(int playerId)
        {
            // 获取玩家的关卡节点
            var levelNodes = await levelNodeService.GetLevelNodesAsync(playerId);
            var levelNodeDtos = levelNodes?.Select(n => new LevelNodeDTO
            {
                Id = n.Id,
                BusinessId = n.BusinessId,
                Process = n.Process,
            }).ToList();

            return levelNodeDtos ?? new List<LevelNodeDTO>();
        }

        /// <summary>
        /// 获取玩家的武士信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        async Task<List<SamuraiDTO>> GetSamuraiDTOs(int playerId)
        {
            //获取玩家samurai
            var samurais = await samuraiService.GetAllSamuraiAsync(playerId);
            if (samurais == null || samurais.Count == 0)
            {
                samurais = new List<Samurai>();
                var defaultSamurai = await samuraiService.AddSamuraiAsync(tiktokConfigService.GetDefaultSamuraiBusinessId()
                    , tiktokConfigService.GetDefaultSoldierBusinessId(tiktokConfigService.GetDefaultSamuraiBusinessId()), playerId);
                samurais.Add(defaultSamurai);
            }
            var samuraisDTO = samurais?.Select(n => new SamuraiDTO
            {
                Id = n.Id,
                BusinessId = n.BusinessId,
                Level = n.Level,
                Experience = n.Experience,
            }).ToList();

            return samuraisDTO ?? new List<SamuraiDTO>();
        }

        /// <summary>
        /// 获取玩家的阵型信息
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="formationType"></param>
        /// <returns></returns>
        async Task<List<FormationDTO>> GetFormationDTOs(int playerId, int formationType)
        {
            //获取玩家formation
            var formations = await formationService.GetFormationAsync(formationType, playerId);
            if (formations == null || formations.Count == 0)
            {
                formations = new List<Formation>();

                //获取所有武士
                var samurais = await samuraiService.GetAllSamuraiAsync(playerId);
                var first = samurais.FirstOrDefault();
                if(first == null)
                {
                    first = await samuraiService.AddSamuraiAsync(tiktokConfigService.GetDefaultSamuraiBusinessId()
                            , tiktokConfigService.GetDefaultSoldierBusinessId(tiktokConfigService.GetDefaultSamuraiBusinessId()), playerId);
                }

                var defaultFormation = await formationService.AddFormationAsync(formationType, tiktokConfigService.GetDefaultFormationPoint(), first.Id, playerId);
                formations.Add(defaultFormation);
            }
            var formationsDTO = formations?.Select(n => new FormationDTO
            {
                Id = n.Id,
                FormationType = n.FormationType,
                FormationPoint = n.FormationPoint,
                SamuraiId = n.SamuraiId,
            }).ToList();
            return formationsDTO ?? new List<FormationDTO>();
        }


    }
}