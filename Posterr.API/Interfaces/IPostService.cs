using Posterr.API.Entities;

namespace Posterr.API.Interfaces
{
    public interface IPostService
    {
        public List<Post> GetPosts(PostQueryParameters queryParameters);
        public void InsertPost(PostBody obj);

    }
}
