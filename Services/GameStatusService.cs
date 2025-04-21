using Models.DTOs;
using Models.Reqs;
using Models.Resps;
using Repos;

namespace Services
{
    public interface IGameStatusService
    {
        Task<BaseResp> GetByAfterUpdatedAtAsync(int userId, DateTime updatedAt, int page);
        Task<BaseResp> InactivateAsync(int id, int uid);
        Task<BaseResp> UpdateAsync(ReqGameStatus reqGameStatus, int uid);
        Task<BaseResp> UpsertAsync(ReqGame reqGame, int uid);
    }

    public class GameStatusService(IGameRepo gameRepo, IGameStatusRepo gameStatusRepo) : IGameStatusService
    {
        readonly int pageSize = 50;

        public async Task<BaseResp> UpsertAsync(ReqGame reqGame, int uid)
        {
            try
            {
                string? validateError = reqGame.Validate();
                if (!string.IsNullOrEmpty(validateError)) return new BaseResp(null, validateError);

                var game = await gameRepo.GetByIGDBIdAsync(reqGame.IGDBId);

                if (game is null)
                {
                    game = new Models.DTOs.GameDTO
                    {
                        IGDBId = reqGame.IGDBId,
                        CreatedAt = DateTime.UtcNow,
                        Name = reqGame.Name,
                        CoverUrl = reqGame.CoverUrl,
                        Platforms = reqGame.Platforms,
                        ReleaseDate = reqGame.ReleaseDate,
                        Summary = reqGame.Summary
                    };

                    await gameRepo.CreateAsync(game);
                }

                var gameStatus = await gameStatusRepo.GetByGameIdAndUserIdAsync(game.Id, uid);

                if (gameStatus is null)
                {
                    gameStatus = new Models.DTOs.GameStatusDTO
                    {
                        UserId = uid,
                        Status = reqGame.Status,
                        Rate = reqGame.Rate,
                        CreatedAt = DateTime.UtcNow,
                        GameId = game.Id
                    };

                    await gameStatusRepo.CreateAsync(gameStatus);
                }
                else
                {
                    gameStatus.Status = reqGame.Status;
                    gameStatus.Rate = reqGame.Rate;
                    await gameStatusRepo.UpdateAsync(gameStatus);
                }

                return new BaseResp(new ResGameStatusIds(game.Id, gameStatus.Id));
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<BaseResp> UpdateAsync(ReqGameStatus reqGameStatus, int uid)
        {
            try
            {
                string? validateError = reqGameStatus.Validate();
                if (!string.IsNullOrEmpty(validateError)) return new BaseResp(null, validateError);

                var gameStatus = await gameStatusRepo.GetByIdAndUserIdAsync(reqGameStatus.Id, uid);

                if (gameStatus is null) return new BaseResp(null, "Game status not found");

                gameStatus.Status = reqGameStatus.Status;
                gameStatus.Rate = reqGameStatus.Rate;

                await gameStatusRepo.UpdateAsync(gameStatus);

                return new BaseResp(new ResGameStatusIds(gameStatus.GameId, gameStatus.Id));
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<BaseResp> InactivateAsync(int id, int uid)
        {
            try
            {
                var gameStatus = await gameStatusRepo.GetByIdAndUserIdAsync(id, uid);
                if (gameStatus is null) return new BaseResp(null, "Game status not found");
                gameStatus.Inactive = true;
                await gameStatusRepo.UpdateAsync(gameStatus);
                return new BaseResp(new ResGameStatusIds(gameStatus.GameId, gameStatus.Id));
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<BaseResp> GetByAfterUpdatedAtAsync(int userId, DateTime updatedAt, int page)
        {
            try
            {
                if (page <= 0)
                    return new BaseResp(null, "Page must be greater than 0");

                var gameStatuses = await gameStatusRepo.GetByAfterUpdatedAtAsync(userId, updatedAt, page, pageSize);

                List<ResGameStatus> resGameStatuses = [];

                foreach (var gameStatus in gameStatuses)
                {
                    resGameStatuses.Add(new ResGameStatus
                    {
                        Id = gameStatus.Id,
                        CreatedAt = gameStatus.CreatedAt,
                        Status = gameStatus.Status,
                        Rate = gameStatus.Rate,
                        UpdatedAt = gameStatus.UpdatedAt,
                        Inactive = gameStatus.Inactive,
                        Game = new ResGame
                        {
                            Id = gameStatus.GameId,
                            Name = gameStatus.Game.Name,
                            CoverUrl = gameStatus.Game.CoverUrl,
                            Platforms = gameStatus.Game.Platforms,
                            ReleaseDate = gameStatus.Game.ReleaseDate,
                            Summary = gameStatus.Game.Summary,
                            CreatedAt = gameStatus.Game.CreatedAt,
                            IGDBId = gameStatus.Game.IGDBId
                        }
                    });
                }

                return new BaseResp(resGameStatuses);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
