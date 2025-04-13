using Models.DTOs;

namespace Repos
{
    public interface IUserRepo
    {
        Task CreateAsync(UserDTO user);
        Task<UserDTO?> GetByEmailAsync(string email);
    }
}