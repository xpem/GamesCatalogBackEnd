using Microsoft.AspNetCore.Mvc;
using Models.Resps;

namespace GamesCatalogAPI.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult BuildResponse(BaseResp baseResp) =>
            (!string.IsNullOrEmpty(baseResp.Error?.Message)) ?
            BadRequest(baseResp.Error.Message) :
            Ok(baseResp.Content);
    }
}
