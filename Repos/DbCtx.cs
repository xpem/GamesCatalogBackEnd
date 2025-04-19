using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Repos
{
    public class DbCtx(DbContextOptions<DbCtx> options) : DbContext(options)
    {
        public DbSet<UserDTO> User => Set<UserDTO>();

        public DbSet<IGDBAccessTokenDTO> IGDBGame => Set<IGDBAccessTokenDTO>();
    }
}

//migrations
//no console do gerenciador de pacotes selecione o Repo referente:
//EntityFrameworkCore\Add-Migration "create User tbl" -Context DbCtx

//EntityFrameworkCore\update-database -Context DbCtx

//EntityFrameworkCore\Remove-Migration -Context DbCtx