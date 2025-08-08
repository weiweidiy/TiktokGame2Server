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
        ICurrencyService currencyService;
        public DrawSamuraiController(ITokenService tokenService,IDrawSamuraiService drawSamuraiService, TiktokConfigService tiktokConfigService
            ,ICurrencyService currencyService)
        {
            this.tokenService = tokenService;
            this.drawSamuraiService = drawSamuraiService;
            this.tiktokConfigService = tiktokConfigService;
            this.currencyService = currencyService ?? throw new ArgumentNullException(nameof(currencyService));
        }

        [HttpPost("Draw")]
        public async Task<IActionResult> Draw([FromBody] RequestDrawSamurai request)
        {
            var token = Request.Headers["Authorization"].FirstOrDefault();
            var accountUid = tokenService.GetAccountUidFromToken(token);
            var playerUid = tokenService.GetPlayerUidFromToken(token);
            var playerId = tokenService.GetPlayerIdFromToken(token) ?? throw new Exception("解析token异常");

            //抽取的个数
            var poolType = request.DrawPoolType;
            var count = request.Count;

            //从配置表中获取抽取消耗的货币
            var drawCost = tiktokConfigService.GetDrawCost(poolType, count);
            var resourceType = drawCost.Item1;
            var businessId = drawCost.Item2;
            var costAmount = drawCost.Item3;

            //判断玩家货币是否足够
            var currency = await currencyService.GetCurrency(playerId);
            if (resourceType == ResourceType.Currency)
            {
                var currencyType = (CurrencyType)int.Parse(businessId);
                if (currencyType == CurrencyType.Coin)
                {
                    if (currency.Coin < costAmount)
                    {
                        return BadRequest(new { message = "金币不足" });
                    }
                }
                else if (currencyType == CurrencyType.Pan)
                {
                    if (currency.Pan < costAmount)
                    {
                        return BadRequest(new { message = "小判不足" });
                    }
                }

                //货币足够，扣除货币
                currency = await currencyService.SpendCurrency(playerId, currencyType, costAmount);
            }

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


            var remainCurrencyDTO = new CurrencyDTO
            {
                Coin = currency.Coin,
                Pan = currency.Pan
            };

            var response = new DrawDTO
            {
                SamuraiDTOs = samuraiDTOs,
                CurrencyDTO = remainCurrencyDTO,
            };
            return Ok(response);
        }
    }
}