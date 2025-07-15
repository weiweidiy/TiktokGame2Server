using Microsoft.AspNetCore.Mvc;
using Tiktok;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        [HttpPost("Login")]
        public virtual ActionResult<LoginDTO> Login(string uid)
        {
            var loginDTO = new LoginDTO();
            var user = new UserDTO();
            loginDTO.User = user;

            return loginDTO;
        }


    }

    //public class Home2Controller : LoginController
    //{
    //    [HttpGet("Index2")]
    //    public override ActionResult<int> Index()
    //    {
    //        return 4;;
    //        //return base.Index();
    //    }


    //}
}
