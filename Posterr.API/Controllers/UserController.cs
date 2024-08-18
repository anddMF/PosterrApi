using Microsoft.AspNetCore.Mvc;
using Posterr.API.Entities;
using Posterr.API.Interfaces;
using Posterr.API.Services;

namespace Posterr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns information about the user based on the parameter provided.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieves number of posts from the user today, based on user id if its valid.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
