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
        IHpPoolService hpPoolService;
        IAchievementService achievementService;
        ISamuraiService samuraiService;

        public FightController(ILevelNodesService levelNodeService
                            , ITokenService tokenService
                            , ILevelNodeCombatService levelNodeCombatService
                            , TiktokConfigService tiktokConfigService
                            , IHpPoolService hpPoolService
                            , IAchievementService achievementService
                            , ISamuraiService samuraiService)
        {
            this.levelNodeService = levelNodeService;
            this.tokenService = tokenService;
            this.levelNodeCombatService = levelNodeCombatService;
            this.tiktokConfigService = tiktokConfigService;
            this.hpPoolService = hpPoolService;
            this.achievementService = achievementService;
            this.samuraiService = samuraiService;
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
                if (levelNodeBusinessId != tiktokConfigService.GetDefaultFirstNodeBusinessId()) //如果不是默认的初始节点，则检查前置节点是否解锁
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


            if (reportData == null)
            {
                return BadRequest(new { message = "战斗数据获取失败" });
            }

            //获取玩家的hpPool剩余血量
            int hpPoolRemainHp = 0;
            var hpPool = await hpPoolService.GetHpPoolAsync(playerId);
            if(hpPool != null)
            {
                hpPoolRemainHp = hpPool.Hp;
            }
            
            //获取玩家的samurai剩余血量
            var formationData = reportData.FormationData;
            var lstSamurai = formationData[playerUid];
            var samuraiDTOs = new List<SamuraiDTO>();
            foreach (var unit in lstSamurai)
            {
                var samuraiId = int.Parse(unit.Uid);
                var curHp = unit.CurHp;
                var maxHp = unit.MaxHp;
                //如果curHp不满，则尝试从hppool中补充
                if (curHp < maxHp)
                {
                    var offset = maxHp - curHp;
                    if (offset <= hpPoolRemainHp)
                    {
                        curHp += offset;
                        hpPoolRemainHp -= offset;
                        //更新hppool
                        await hpPoolService.SubtractHpPoolAsync(playerId, offset);
                    }
                    else
                    {
                        curHp += hpPoolRemainHp; //补充到满血
                        hpPoolRemainHp = 0; //hppool清空
                        await hpPoolService.SubtractHpPoolAsync(playerId, hpPoolRemainHp);
                    }

                    //更新samurai的血量
                    var samurai = await samuraiService.UpdateSamuraiHpAsync(samuraiId, curHp);
                    var samuraiDTO = new SamuraiDTO
                    {
                        Id = samuraiId,
                        BusinessId = unit.SamuraiBusinessId,
                        CurHp = samurai.CurHp,
                        //MaxHp = maxHp
                    };
                    samuraiDTOs.Add(samuraiDTO);
                }
            }

            //更新血池信息
            var hpPoolDTO = new HpPoolDTO
            {
                Hp = hpPoolRemainHp,
                MaxHp = hpPool?.MaxHp ?? 0 // 如果hpPool为null，则默认为0
            };


            var result = reportData.winnerTeamUid == playerUid ? true : false;
            if (result)
            {
                levelNode = await levelNodeService.LevelNodeVictoryAsync(levelNodeBusinessId, playerId);
                //根据成就达成条件 更新levelNode process
                var process = levelNode.Process + 1;
                var achievementBusinessId = tiktokConfigService.GetAchievementBusinessId(levelNodeBusinessId, process);
                if(achievementBusinessId != null)
                {
                    int maxAchievementProcess = tiktokConfigService.GetMaxAchievementProcess(levelNodeBusinessId);
                    if (achievementService.IsAchievementCompleted(playerUid, reportData, achievementBusinessId) && levelNode.Process < maxAchievementProcess)
                    {
                        levelNode.Process++;
                        //更新levelNode process
                        levelNode = await levelNodeService.UpdateLevelNodeProcessAsync(levelNodeBusinessId, playerId, levelNode.Process);


                    }
                }  
            }

            var levelNodeDTO = new LevelNodeDTO
            {
                BusinessId = levelNodeBusinessId,
                Process = levelNode?.Process ?? 0,
            };

            fightDTO.LevelNodeDTO = levelNodeDTO;
            fightDTO.ReportData = reportData ?? new TiktokJCombatTurnBasedReportData();
            fightDTO.SamuraiDTOs = samuraiDTOs;
            fightDTO.HpPoolDTO = hpPoolDTO;
            return Ok(fightDTO);

        }
    }
}