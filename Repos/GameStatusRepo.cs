using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Repos
{
    public interface IGameStatusRepo
    {
        Task CreateAsync(GameStatusDTO gameStatus);

        Task<GameStatusDTO?> GetByGameIdAndUserIdAsync(int gameId, int userId);

        Task UpdateAsync(GameStatusDTO gameStatus);

        Task<GameStatusDTO?> GetByIdAndUserIdAsync(int id, int userId);

        Task<List<GameStatusDTO>> GetByAfterUpdatedAtAsync(int userId, DateTime updatedAt, int page, int pageSize);
    }

    public class GameStatusRepo(IDbContextFactory<DbCtx> DbCtx) : IGameStatusRepo
    {
        public async Task CreateAsync(GameStatusDTO gameStatus)
        {
            using var context = DbCtx.CreateDbContext();
            await context.GameStatus.AddAsync(gameStatus);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GameStatusDTO gameStatus)
        {
            using var context = DbCtx.CreateDbContext();
            context.GameStatus.Update(gameStatus);
            await context.SaveChangesAsync();
        }

        public async Task<GameStatusDTO?> GetByGameIdAndUserIdAsync(int gameId, int userId)
        {
            using var context = DbCtx.CreateDbContext();
            return await context.GameStatus
                .FirstOrDefaultAsync(x => x.GameId.Equals(gameId) && x.UserId.Equals(userId) && !x.Inactive);
        }

        public async Task<GameStatusDTO?> GetByIdAndUserIdAsync(int id, int userId)
        {
            using var context = DbCtx.CreateDbContext();
            return await context.GameStatus
                .FirstOrDefaultAsync(x => x.Id.Equals(id) && x.UserId.Equals(userId) && !x.Inactive);
        }

        public async Task<List<GameStatusDTO>> GetByAfterUpdatedAtAsync(int userId, DateTime updatedAt, int page, int pageSize)
        {
            using var context = DbCtx.CreateDbContext();
            return await context.GameStatus.Where(x => x.UserId == userId && x.UpdatedAt > updatedAt)
                .Include(x => x.Game)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
