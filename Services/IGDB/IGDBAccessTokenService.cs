using ApiRepos;
using Models.DTOs;
using Models.Resps.IGDB;
using Newtonsoft.Json;
using Repos;

namespace Services.IGDB
{
    public interface IIGDBAccessTokenService
    {
        Task<IGDBAccessTokenDTO> GetAsync();
        Task<string> UpdateAccessToken(string accessToken);
    }

    public class IGDBAccessTokenService(IIGDBAccessTokenRepo iGDBAccessTokenRepo, IIGDBAccessTokenAPIRepo iIGDBAccessTokenAPIRepo) : IIGDBAccessTokenService
    {
        public async Task<Models.DTOs.IGDBAccessTokenDTO> GetAsync()
        {
            Models.DTOs.IGDBAccessTokenDTO? accessToken = await iGDBAccessTokenRepo.GetAsync();

            //first run
            if (accessToken is null)
            {
                var apiResp = await iIGDBAccessTokenAPIRepo.GetAsync();

                if (apiResp is not null && apiResp.Success)
                {
                    accessToken = BuildIGDBAccessTokenDTO(apiResp.Content);

                    await iGDBAccessTokenRepo.CreateAsync(accessToken);
                }
                else throw new Exception("Error Getting User IGDB API Access Token: " + apiResp?.Content);
            }

            return accessToken;
        }

        public async Task<string> UpdateAccessToken(string accessToken)
        {
            Models.DTOs.IGDBAccessTokenDTO oldAccessTokenDTO = await GetAsync();

            if (oldAccessTokenDTO.AccessToken == accessToken)
            {
                oldAccessTokenDTO.AccessToken = accessToken;
                var newAccessTokenDTO = await UpdateAsync(oldAccessTokenDTO);

                return newAccessTokenDTO.AccessToken;
            }
            else return oldAccessTokenDTO.AccessToken;
        }

        private async Task<IGDBAccessTokenDTO> UpdateAsync(IGDBAccessTokenDTO accessToken)
        {
            var apiResp = await iIGDBAccessTokenAPIRepo.GetAsync();

            if (apiResp is not null && apiResp.Success)
            {
                accessToken = BuildIGDBAccessTokenDTO(apiResp.Content);

                await iGDBAccessTokenRepo.UpdateAsync(accessToken);
            }
            else throw new Exception("Error Getting User IGDB API Access Token: " + apiResp?.Content);

            return accessToken;
        }

        private static IGDBAccessTokenDTO BuildIGDBAccessTokenDTO(string apiRespContent)
        {
            var iGDBToken = JsonConvert.DeserializeObject<IGDBToken>(apiRespContent);

            return new IGDBAccessTokenDTO()
            {
                AccessToken = iGDBToken.access_token,
                ExpiresIn = iGDBToken.expires_in,
                TokenType = iGDBToken.token_type,
                UpdatedAt = DateTime.UtcNow,
            };
        }
    }
}
