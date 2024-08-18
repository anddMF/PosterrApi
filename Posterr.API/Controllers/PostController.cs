using Microsoft.AspNetCore.Mvc;
using Posterr.API.Entities;
using Posterr.API.Interfaces;
using Posterr.API.Services;

namespace Posterr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Retrieves a list of posts based on the provided query parameters, if they are complete.
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetPosts([FromQuery] PostQueryParameters queryParameters)
        {
            try
            {
                if (!queryParameters.GetAll && (!queryParameters.UserId.HasValue || queryParameters.UserId.Value <= 0))
                    return BadRequest("UserId must be valid if param GetAll is false");

                var posts = _postService.GetPosts(queryParameters);

                return Ok(posts);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates new post based on the object provided.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post([FromBody] PostBody post)
        {
            try
            {
                _postService.InsertPost(post);
                return StatusCode(201);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
