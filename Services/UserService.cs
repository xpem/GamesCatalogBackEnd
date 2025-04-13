using Models.DTOs;
using Models.Reqs.User;
using Models.Resps;
using Repos;

namespace Services
{
    public class UserService(IUserRepo userRepo) : IUserService
    {
        public async Task<BaseResp> CreateAsync(ReqUser reqUser)
        {
            string? validateError = reqUser.Validate();
            if (!string.IsNullOrEmpty(validateError)) return new BaseResp(null, validateError);

            UserDTO user = new()
            {
                Name = reqUser.Name,
                Email = reqUser.Email,
                Password = reqUser.Password,
                CreatedAt = DateTime.Now
            };

            string? existingUserMessage = await ValidateExistingUserAsync(user);
            if (existingUserMessage != null) { return new BaseResp(null, existingUserMessage); }

            await userRepo.CreateAsync(user);

            ResUser? resUser = new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };

            return new BaseResp(resUser);
        }

        protected async Task<string?> ValidateExistingUserAsync(UserDTO user)
        {
            UserDTO? userResp = await userRepo.GetByEmailAsync(user.Email);

            if (userResp is not null && userResp.Email.Equals(user.Email))
                return "User Email already exists.";

            return null;
        }
    }
}
