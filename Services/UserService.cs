using Models.DTOs;
using Models.Reqs.User;
using Models.Resps;
using Repos;
using Services.Functions;

namespace Services
{
    public class UserService(IUserRepo userRepo, IEncryptionService encryptionService, IJwtTokenService jwtTokenService) : IUserService
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

            if (user.Password != null)
                user.Password = encryptionService.Encrypt(user.Password);
            else throw new NullReferenceException(nameof(user.Password));

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

        public async Task<BaseResp> GenerateTokenAsync(ReqUserSession reqUserSession)
        {
            string? validateError = reqUserSession.Validate();

            if (!string.IsNullOrEmpty(validateError)) return new BaseResp(null, validateError);

            UserDTO? userResp = await userRepo.GetByEmailAndPasswordAsync(reqUserSession.Email, encryptionService.Encrypt(reqUserSession.Password));

            if (userResp is null) return new BaseResp(null, "User/Password incorrect");

            string userJwt = jwtTokenService.GenerateToken(userResp.Id, userResp.Email, DateTime.UtcNow.AddDays(5));

            ResToken resToken = new(userJwt);

            return new BaseResp(resToken);
        }

        public async Task<BaseResp> GetByIdAsync(int uid)
        {
            UserDTO? userResp = await userRepo.GetByIdAsync(uid);

            if (userResp == null)
                return new BaseResp(null, "User not found");

            return new BaseResp(new ResUser() { Id = userResp.Id, Name = userResp.Name, Email = userResp.Email, CreatedAt = userResp.CreatedAt });
        }
    }
}
