using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Repos
{
    public class DbCtx(DbContextOptions<DbCtx> options) : DbContext(options)
    {
        public DbSet<UserDTO> User => Set<UserDTO>();

        public DbSet<IGDBAccessTokenDTO> IGDBGame => Set<IGDBAccessTokenDTO>();

        public DbSet<GameDTO> Game => Set<GameDTO>();

        public DbSet<GameStatusDTO> GameStatus => Set<GameStatusDTO>();
    }
}

//migrations
//EntityFrameworkCore\Add-Migration "<description>" -Context DbCtx

//EntityFrameworkCore\update-database -Context DbCtx

//revert
//EntityFrameworkCore\Remove-Migration -Context DbCtx