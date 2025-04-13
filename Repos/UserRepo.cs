using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Repos;

public class UserRepo(IDbContextFactory<DbCtx> DbCtx) : IUserRepo
{
    public async Task CreateAsync(UserDTO user)
    {
        using var context = DbCtx.CreateDbContext();

        await context.User.AddAsync(user);

        await context.SaveChangesAsync();
    }

    public async Task<UserDTO?> GetByEmailAsync(string email)
    {
        using var context = DbCtx.CreateDbContext();
        return await context.User.FirstOrDefaultAsync(x => x.Email.Equals(email));
    }
}
