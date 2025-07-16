using Microsoft.AspNetCore.Mvc;
using Tiktok;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        [HttpPost("FastLogin")]
        public virtual ActionResult<LoginDTO> FastLogin(string uid)
        {
            var loginDTO = new LoginDTO();
            var user = new UserDTO();
            loginDTO.User = user;

            return loginDTO;
        }


    }


}
