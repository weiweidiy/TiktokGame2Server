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
        IRewardService rewardService;

        public FightController(ILevelNodesService levelNodeService
                            , ITokenService tokenService
                            , ILevelNodeCombatService levelNodeCombatService
                            , TiktokConfigService tiktokConfigService
                            , IHpPoolService hpPoolService
                            , IAchievementService achievementService
                            , ISamuraiService samuraiService
                            , IRewardService rewardService)
        {
            this.levelNodeService = levelNodeService;
            this.tokenService = tokenService;
            this.levelNodeCombatService = levelNodeCombatService;
            this.tiktokConfigService = tiktokConfigService;
            this.hpPoolService = hpPoolService;
            this.achievementService = achievementService;
            this.samuraiService = samuraiService;
            this.rewardService = rewardService;
        }

        // 修复 CS8600: 将 null 文本或可能的 null 值转换为不可为 null 类型。
        // 主要是 levelNode 可能为 null，需加上 null 检查。

        [HttpPost("Fight")]
        public async Task<IActionResult> Fight([FromBody] RequestFight requestFight)
        {
            //从token解析中获取账号Uid
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            //需要打的关卡节点ID
            var levelNodeBusinessId = requestFight.LevelNodeBusinessId;
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
                //var samuraiId = await samuraiService.QuerySamuraiId(unit.Uid, playerId);
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
                    var samurai = await samuraiService.UpdateSamuraiHpAsync(int.Parse(unit.Uid), curHp);
                    var samuraiDTO = new SamuraiDTO
                    {
                        Id = samurai.Id,
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

            RewardDTO? winRewardDTO = null;
            RewardDTO? achievementRewardDTO = null;
            var result = reportData.winnerTeamUid == playerUid ? true : false;
            if (result)
            {
                //战斗胜利
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

                        //根据Process 添加奖励,只在达成成就时添加奖励一次
                        var rewardBusinessId = tiktokConfigService.GetLevelNodeAchievementRewardBusinessId(levelNodeBusinessId, levelNode.Process);
                        if (rewardBusinessId != null)
                        {
                            //添加奖励
                            var achievementReward = await rewardService.AddReward(playerId, rewardBusinessId);

                            // 组装AchievementRewardDTO
                            achievementRewardDTO = new RewardDTO
                            {
                                Currencies = BuildCurrencyDTOs(rewardBusinessId, achievementReward),
                                BagItems = BuildItemDTOs(rewardBusinessId, achievementReward)
                            };
                        }
                    }
                }

                //只要胜利就给与胜利奖励
                var victoryRewardBusinessId = tiktokConfigService.GetLevelNodeVictoryRewardBusinessId(levelNodeBusinessId);
                if (victoryRewardBusinessId != null)
                {
                    //添加奖励
                    var winReward = await rewardService.AddReward(playerId, victoryRewardBusinessId);
                    // 组装WinRewardDTO
                    winRewardDTO = new RewardDTO
                    {
                        Currencies = BuildCurrencyDTOs(victoryRewardBusinessId, winReward),
                        BagItems = BuildItemDTOs(victoryRewardBusinessId, winReward)
                    };
                }
            }

            var levelNodeDTO = new LevelNodeDTO
            {
                BusinessId = levelNodeBusinessId,
                Process = levelNode?.Process ?? 0,
            };

            var responseFight = new FightDTO()
            {
                LevelNodeBusinessId = levelNodeBusinessId,
                LevelNodeDTO = levelNodeDTO,
                ReportData = reportData,
                SamuraiDTOs = samuraiDTOs,
                HpPoolDTO = hpPoolDTO,
                WinRewardDTO = winRewardDTO,
                AchievementRewardDTO = achievementRewardDTO,
            };

            return Ok(responseFight);

        }

        private List<CurrencyDTO> BuildCurrencyDTOs(string rewardBusinessId, List<(ResourceType, string, int)> rewardList)
        {
            var result = new List<CurrencyDTO>();
            foreach (var (type, businessId, count) in rewardList)
            {
                if (type == ResourceType.Currency)
                {
                    result.Add(new CurrencyDTO
                    {
                        CurrencyType = (CurrencyType)int.Parse(businessId),
                        Count = count
                    });
                }
            }
            return result;
        }

        private List<ItemDTO> BuildItemDTOs(string rewardBusinessId, List<(ResourceType, string, int)> rewardList)
        {
            var result = new List<ItemDTO>();
            foreach (var (type, businessId, count) in rewardList)
            {
                // 你可以根据实际的 ResourceType 枚举名调整判断条件
                if (type == ResourceType.BagItem )
                {
                    result.Add(new ItemDTO
                    {
                        Id = 0, // 如有实际Id可补充
                        ItemBusinessId = businessId,
                        Count = count,
                        BagSlotId = 0 // 如有实际BagSlotId可补充
                    });
                }
            }
            return result;
        }

    }
}