﻿using Models.Reqs.User;
using Models.Resps;

namespace Services
{
    public interface IUserService
    {
        Task<BaseResp> CreateAsync(ReqUser reqUser);
        Task<BaseResp> GenerateTokenAsync(ReqUserSession reqUserSession);
        Task<BaseResp> GetByIdAsync(int uid);
        Task<BaseResp> SendRecoverPasswordEmailAsync(ReqUserEmail reqUserEmail);
        Task<BaseResp> UpdatePasswordAsync(ReqRecoverPassword reqRecoverPassword, int uid);
    }
}