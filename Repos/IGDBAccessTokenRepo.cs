using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Repos
{
    public interface IIGDBAccessTokenRepo
    {
        Task CreateAsync(IGDBAccessTokenDTO token);
        Task<IGDBAccessTokenDTO?> GetAsync();
        Task UpdateAsync(IGDBAccessTokenDTO token);
    }

    public class IGDBAccessTokenRepo(IDbContextFactory<DbCtx> DbCtx) : IIGDBAccessTokenRepo
    {
        public async Task<IGDBAccessTokenDTO?> GetAsync()
        {
            using var context = DbCtx.CreateDbContext();
            return await context.IGDBGame.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(IGDBAccessTokenDTO token)
        {
            using var context = DbCtx.CreateDbContext();
            await context.IGDBGame.AddAsync(token);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IGDBAccessTokenDTO token)
        {
            using var context = DbCtx.CreateDbContext();
            context.IGDBGame.Update(token);
            await context.SaveChangesAsync();
        }
    }
}
