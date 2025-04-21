using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Reqs;
using Models.Reqs.IGDB;
using Services;
using Services.IGDB;

namespace GamesCatalogAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class GameController(IIGDBGamesApiService iGDBGamesApiService, IGameStatusService gameStatusService) : BaseController
    {
        [Route("IGDB")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetIGDB(ReqIGDBGamesSearch reqIGDBGamesSearch) => BuildResponse(await iGDBGamesApiService.GetAsync(reqIGDBGamesSearch));

        [Route("status")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpsertGameStatus(ReqGame reqGame) 
            => BuildResponse(await gameStatusService.UpsertAsync(reqGame, Uid));

        [Route("status")]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateGameStatus(ReqGameStatus reqGameStatus) 
            => BuildResponse(await gameStatusService.UpdateAsync(reqGameStatus, Uid));

        [Route("status/{id}")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> InactivateGameStatus(int id) 
            => BuildResponse(await gameStatusService.InactivateAsync(id, Uid));

        //date format 2023-06-10T21:53:28.331Z
        [Route("status/byupdatedat/{updatedAt}/{page}")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByAfterUpdatedAt(DateTime updatedAt, int page) 
            => BuildResponse(await gameStatusService.GetByAfterUpdatedAtAsync(Uid, updatedAt, page));
    }
}
