using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tiktok;
using TiktokGame2Server.Entities;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(MyDbContext myDbContext, ITokenService tokenService) : Controller
    {
        MyDbContext myDbContext = myDbContext;
        private readonly ITokenService tokenService = tokenService;

        //[HttpPost("FastLogin")]
        //public virtual async Task<ActionResult<LoginDTO>> FastLogin(string uid)
        //{
        //    var loginDTO = new LoginDTO();
        //    var userDTO = new UserDTO();
        //    loginDTO.User = userDTO;

        //    var userEntity = await myDbContext.Players.FindAsync(uid);
        //    if (userEntity == null)
        //    {
        //        userEntity = new Player() { Id = uid, Name = uid };
        //        try
        //        {
        //            myDbContext.Players.Add(userEntity);
        //            myDbContext.SaveChanges();
        //        }
        //        catch
        //        {
        //            return BadRequest();
        //        }
        //    }

        //    userDTO.Uid = userEntity?.Id;
        //    userDTO.Username = userEntity?.Name;

        //    return loginDTO;
        //}


        [HttpPost("FastLogin")]
        public IActionResult FastLogin([FromBody]AccountDTO accountDto)
        {
            // 判断是否存在，不存在则创建游客账号
            

            // 创建游客用户
            var guestAccount = new Account
            {
                //Username = guestUsername,
                //Password = null, // 游客不需要密码
                Uid = accountDto.Uid,
                Role = "Guest"
            };

            myDbContext.Add(guestAccount);
            myDbContext.SaveChanges();

            // 生成JWT token
            var token = tokenService.GenerateToken(guestAccount);

            return Ok(new
            {
                Token = token,
                Account = new { guestAccount.Id, guestAccount.Uid, guestAccount.Role }
            });
        }

    }


}
