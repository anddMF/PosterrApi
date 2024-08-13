using Microsoft.AspNetCore.Mvc;
using Posterr.API.Entities;
using Posterr.API.Services;

namespace Posterr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        public UserController()
        {
            _userService = new UserService();
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUser(int userId)
        {
            try
            {
                if (userId <= 0)
                    return BadRequest("UserId must be valid");

                var user = _userService.GetUser(userId);

                return Ok(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("today")]
        [HttpGet]
        public ActionResult<int> GetTodayPostUsage(int userId)
        {
            try
            {
                if (userId <= 0)
                    return BadRequest("UserId must be valid");

                int usage = _userService.GetTodayPostsUsage(userId);

                return Ok(usage);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
