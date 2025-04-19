using Models.Resps.IGDB;
using System.Net;

namespace ApiRepos
{
    public interface IIGDBAccessTokenAPIRepo
    {
        Task<ApiResp> GetAsync();
    }

    public class IGDBAccessTokenAPIRepo(string ID, string Secret) : IIGDBAccessTokenAPIRepo
    {
        public async Task<ApiResp> GetAsync()
        {
            HttpClient httpClient = new();

            HttpResponseMessage httpResponse = await httpClient.PostAsync($"https://id.twitch.tv/oauth2/token?client_id={ID}&client_secret={Secret}&grant_type=client_credentials", null);

            return new ApiResp()
            {
                Success = httpResponse.IsSuccessStatusCode,
                Error = httpResponse.StatusCode == HttpStatusCode.Unauthorized ? ErrorTypes.Unauthorized : null,
                Content = await httpResponse.Content.ReadAsStringAsync()
            };
        }
    }
}
