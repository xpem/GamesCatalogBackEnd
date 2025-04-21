using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Repos
{
    public interface IGameRepo
    {
        Task CreateAsync(GameDTO game);
        Task<GameDTO?> GetByIGDBIdAsync(int igdbId);
    }

    public class GameRepo(IDbContextFactory<DbCtx> DbCtx) : IGameRepo
    {
        public async Task CreateAsync(GameDTO game)
        {
            using var context = DbCtx.CreateDbContext();
            await context.Game.AddAsync(game);
            await context.SaveChangesAsync();
        }

        public async Task<GameDTO?> GetByIGDBIdAsync(int igdbId)
        {
            using var context = DbCtx.CreateDbContext();
            return await context.Game.FirstOrDefaultAsync(x => x.IGDBId.Equals(igdbId));
        }
    }
}
