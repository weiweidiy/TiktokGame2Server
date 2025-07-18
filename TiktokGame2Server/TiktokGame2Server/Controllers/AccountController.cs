using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tiktok;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(MyDbContext myDbContext, ITokenService tokenService, IAccountService accountService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        private readonly ITokenService tokenService = tokenService;
        IAccountService accountService = accountService;

        [HttpPost("FastLogin")]
        public async Task<IActionResult> FastLogin([FromBody] AccountDTO accountDto)
        {
            if (accountDto == null || string.IsNullOrEmpty(accountDto.Uid))
            {
                return BadRequest("Invalid account data.");
            }

            // 将 Account account = await GetAccount(accountDto.Uid); 改为可空类型
            Account? account = await accountService.GetAccountAsync(accountDto.Uid);
            // 检查是否存在该账号
            if (account == null)
                account = await accountService.CreateAccountAsync(accountDto.Uid, "Guest");

            if (account == null)
                return BadRequest("create account failed");

            // 生成JWT token
            var token = tokenService.GenerateToken(account);

            return Ok(new AccountDTO
            {
                Token = token,
                Uid = account.Uid
                //Account = new { account.Id, account.Uid, account.Role }
            });
        }
    }
}
