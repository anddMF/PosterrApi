using Posterr.API.Entities;

namespace Posterr.API.Interfaces
{
    public interface IUserService
    {
        public User GetUser(int userId);
        public int GetTodayPostsUsage(int userId);
    }
}
