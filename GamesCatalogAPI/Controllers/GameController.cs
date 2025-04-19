using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Reqs.IGDB;
using Services.IGDB;

namespace GamesCatalogAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class GameController(IIGDBGamesApiService iGDBGamesApiService) : BaseController
    {
        [Route("IGDB")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetIGDB(ReqIGDBGamesSearch reqIGDBGamesSearch) => BuildResponse(await iGDBGamesApiService.GetAsync(reqIGDBGamesSearch));
    }
}
