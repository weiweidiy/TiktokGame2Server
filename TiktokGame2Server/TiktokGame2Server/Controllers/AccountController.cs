using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tiktok;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController(MyDbContext myDbContext) : Controller
    {
        MyDbContext myDbContext = myDbContext;

        [HttpPost("FastLogin")]
        public virtual async Task<ActionResult<LoginDTO>> FastLogin(string uid)
        {
            var loginDTO = new LoginDTO();
            var userDTO = new UserDTO();
            loginDTO.User = userDTO;

            var userEntity = await myDbContext.Players.FindAsync(uid);
            if (userEntity == null)
            {
                userEntity = new Player() { Id = uid, Name = uid };
                try
                {
                    myDbContext.Players.Add(userEntity);
                    myDbContext.SaveChanges();
                }
                catch
                {
                    return BadRequest();
                }
            }

            userDTO.Uid = userEntity?.Id;
            userDTO.Username = userEntity?.Name;

            return loginDTO;
        }


        //public async Task<IActionResult> RegisterGuest(string deviceId)
        //{
        //    // 1. 查找用户
        //    var account = await myDbContext.Players.FindAsync(deviceId);
        //    // 或者按设备ID查找（如果我们没有把设备ID作为用户名，则按DeviceId查找）
        //    // 2. 如果用户不存在，则创建
        //    //if (account == null)
        //    //{
        //    //    account = new Account
        //    //    {
        //    //        UserName = deviceId,
        //    //        DeviceId = deviceId
        //    //    };
        //    //    // 生成随机密码
        //    //    var password = GenerateRandomPassword();
        //    //    var result = await _userManager.CreateAsync(account, password);
        //    //    if (!result.Succeeded)
        //    //    {
        //    //        return BadRequest(result.Errors);
        //    //    }
        //    //}
        //    //// 3. 生成JWT Token
        //    //var token = GenerateJwtToken(account);
        //    return Ok(new { Token = token });
        //}

    }


}
