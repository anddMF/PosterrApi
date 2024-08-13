using Microsoft.AspNetCore.Mvc;
using Posterr.API.Entities;
using Posterr.API.Services;

namespace Posterr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private PostService _postService;
        public PostController()
        {
            _postService = new PostService();
        }

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

        [Route("types")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetPostTypes()
        {
            try
            {
                return Ok(new string[] { "value1", "value2" });
            }
            catch (Exception)
            {
                throw;
            }
        }

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
