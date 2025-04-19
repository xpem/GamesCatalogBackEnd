using APIRepos;
using Models.Reqs.IGDB;
using Models.Resps;
using Models.Resps.IGDB;

namespace Services.IGDB
{
    public interface IIGDBGamesApiService
    {
        Task<BaseResp> GetAsync(ReqIGDBGamesSearch reqIGDBGamesSearch);
    }

    public class IGDBGamesApiService(string clientID, IIGDBAccessTokenService iGDBAccessTokenService) : IIGDBGamesApiService
    {
        public async Task<BaseResp> GetAsync(ReqIGDBGamesSearch reqIGDBGamesSearch)
        {
            string accessToken = (await iGDBAccessTokenService.GetAsync()).AccessToken;

            int retry = 0;

            while (retry <= 4)
            {
                ApiResp? apiResp = await IGDBGamesAPIRepo.GetAsync(clientID, accessToken, reqIGDBGamesSearch.Search, reqIGDBGamesSearch.StartIndex);

                if (apiResp is not null && apiResp.Success)
                {
                    return new BaseResp(apiResp.Content);
                }
                else if (apiResp?.Error == ErrorTypes.Unauthorized)
                {
                    accessToken = await iGDBAccessTokenService.UpdateAccessToken(accessToken) ?? throw new Exception("Error Updating User IGDB API Access Token: null accessToken");
                    retry++;
                }
                else if (apiResp?.Error == ErrorTypes.TooManyRequests)
                {
                    Task.Delay(1000).Wait();
                    retry++;
                }
                else throw new Exception("Error Getting IGDB Games: " + apiResp?.Content);
            }

            return new BaseResp(null, "Error Getting IGDB Games");
        }
    }
}
