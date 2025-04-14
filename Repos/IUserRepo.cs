using Models.DTOs;

namespace Repos
{
    public interface IUserRepo
    {
        Task CreateAsync(UserDTO user);
        Task<UserDTO?> GetByEmailAndPasswordAsync(string email, string encryptedPassword);
        Task<UserDTO?> GetByEmailAsync(string email);
        Task<UserDTO?> GetByIdAsync(int uid);
    }
}