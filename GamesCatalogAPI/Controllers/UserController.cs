using Microsoft.AspNetCore.Mvc;
using Models.Reqs.User;
using Services;

namespace GamesCatalogAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class UserController(IUserService userService) : BaseController
    {
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> SignUp(ReqUser reqUser) => BuildResponse(await userService.CreateAsync(reqUser));
    }
}
