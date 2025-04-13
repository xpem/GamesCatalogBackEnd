using Models.Reqs.User;
using Models.Resps;

namespace Services
{
    public interface IUserService
    {
        Task<BaseResp> CreateAsync(ReqUser reqUser);
    }
}