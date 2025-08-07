using Microsoft.AspNetCore.Mvc;
using Tiktok;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SamuraiController : Controller
    {
        ITokenService tokenService;

        TiktokConfigService tiktokConfigService;
        public SamuraiController(ITokenService tokenService, TiktokConfigService tiktokConfigService)
        {
            this.tokenService = tokenService;

            this.tiktokConfigService = tiktokConfigService;
        }

        //[HttpPost("AddExperience")]
        //public async Task<IActionResult> AddExperience([FromBody] RequestAddSamuraiExp request)
        //{

        //}
    }
}