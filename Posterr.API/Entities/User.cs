using Posterr.API.Infrastructure.DAL.DAO;

namespace Posterr.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int TodayPosts { get; set; }
        public int TotalPosts { get; set; }
        public DateTime RegistrationDate { get; set; }

        public User()
        {
        }

        public User(UserDAO dao)
        {
            Id = dao.id;
            Username = dao.username;
            TodayPosts = dao.today_posts;
            TotalPosts = dao.total_posts;
            RegistrationDate = dao.registration_date;
        }
    }
}
