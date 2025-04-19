using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using Models.Reqs.User;
using Models.Resps;
using Services;
using Services.Functions;
using Services.IGDB;

namespace GamesCatalogAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class UserController(IUserService userService, IHostEnvironment hostingEnvironment, IJwtTokenService jwtTokenService) : BaseController
    {
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> SignUp(ReqUser reqUser) => BuildResponse(await userService.CreateAsync(reqUser));

        [Route("Session")]
        [HttpPost]
        public async Task<IActionResult> SignIn(ReqUserSession reqUserSession) => BuildResponse(await userService.GenerateTokenAsync(reqUserSession));

        [Route("")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser() { return BuildResponse(await userService.GetByIdAsync(Uid)); }

        [Route("RecoverPassword")]
        [HttpPost]
        public async Task<IActionResult> SendRecoverPasswordEmail(ReqUserEmail reqUserEmail) => BuildResponse(await userService.SendRecoverPasswordEmailAsync(reqUserEmail));

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("RecoverPassword/{token}")]
        [HttpGet]
        public IActionResult RecoverPasswordBody(string token)
        {
            string html = System.IO.File.ReadAllText(Path.Combine(hostingEnvironment.ContentRootPath, "StaticFiles", "RecoverPassword", "Index.html"));

            html = html.Replace("{{token}}", token);

            return base.Content(html, "text/html");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("RecoverPassword/{token}")]
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(string token, [FromForm] ReqRecoverPassword reqRecoverPassword)
        {
            string html = System.IO.File.ReadAllText(Path.Combine(hostingEnvironment.ContentRootPath, "StaticFiles", "RecoverPassword", "PasswordUpdated.html"));

            try
            {
                int? uid = jwtTokenService.GetUidFromToken(token);

                html = html.Replace("{{token}}", token);

                if (uid == null)
                    html = html.Replace("{{ReturnMessage}}", "User Not Found");
                else
                {
                    BaseResp bLLResponse = await userService.UpdatePasswordAsync(reqRecoverPassword, Convert.ToInt32(uid));

                    if (bLLResponse.Success)
                        html = html.Replace("{{ReturnMessage}}", bLLResponse.Content?.ToString());
                    else html = html.Replace("{{ReturnMessage}}", bLLResponse.Error?.Message?.ToString());
                }
            }
            catch (SecurityTokenExpiredException)
            {
                html = html.Replace("{{ReturnMessage}}", "Email link expired.");
            }

            return base.Content(html, "text/html");
        }
    }
}
