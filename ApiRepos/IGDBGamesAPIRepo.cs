using Models.Resps.IGDB;
using System.Net;
using System.Text;

namespace APIRepos;

public static class IGDBGamesAPIRepo
{
    public async static Task<ApiResp> GetAsync(string clientID, string tokenTemp, string search, int startIndex)
    {
        HttpClient httpClient = new();

        httpClient.DefaultRequestHeaders.Add("Client-ID", clientID);
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenTemp}");

        var bodyContent = new StringContent($"fields cover,cover.url,cover.image_id,first_release_date,name,platforms.abbreviation,summary;search \"{search}\"; limit 10; offset {startIndex};", Encoding.UTF8, "application/json");

        HttpResponseMessage httpResponse = await httpClient.PostAsync("https://api.igdb.com/v4/games", bodyContent);

        return new ApiResp()
        {
            Success = httpResponse.IsSuccessStatusCode,
            Error = BuildErrorType(httpResponse.StatusCode),
            Content = await httpResponse.Content.ReadAsStringAsync()
        };
    }

    public static ErrorTypes? BuildErrorType(HttpStatusCode httpStatusCode) => httpStatusCode switch
    {
        HttpStatusCode.Unauthorized => ErrorTypes.Unauthorized,
        HttpStatusCode.TooManyRequests => ErrorTypes.TooManyRequests,
        _ => null
    };
}
