using Microsoft.AspNetCore.Mvc;

namespace GamesCatalogAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [Route("")]
        [HttpPost]
        public IActionResult Index(User user)
        {
            // Use the 'user' parameter to avoid the IDE0060 warning
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            return Ok($"Received user: {user.Name}, {user.Email}");
        }
    }

    public record User(string Name, string Email)
    {
        public required string Name { get; set; } = Name;
        public required string Email { get; set; } = Email;
    }
}
