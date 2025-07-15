using Microsoft.AspNetCore.Mvc;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("Index")]
        public virtual ActionResult<int> Index()
        {
            return 123;
        }


    }

    //public class Home2Controller : HomeController
    //{
    //    [HttpGet("Index2")]
    //    public override ActionResult<int> Index()
    //    {
    //        return 4;;
    //        //return base.Index();
    //    }


    //}
}
