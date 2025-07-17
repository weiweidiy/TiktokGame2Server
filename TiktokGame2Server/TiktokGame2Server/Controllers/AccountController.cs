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
            
            var userEntity = await myDbContext.Users.FindAsync(uid);
            if(userEntity == null)
            {
                userEntity = new User() { Id = uid, Name = uid };
                try
                {
                    myDbContext.Users.Add(userEntity);
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


    }


}
