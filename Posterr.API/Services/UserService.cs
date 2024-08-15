using Posterr.API.Entities;
using Posterr.API.Infrastructure.DAL.DAO;
using Posterr.API.Infrastructure.DAL;
using Posterr.API.Interfaces;

namespace Posterr.API.Services
{
    public class UserService : IUserService
    {
        private readonly IDBCommunicationFactory _dbCommFactory;
        private IDBCommunication _dbComm;

        public UserService(IDBCommunicationFactory dbCommunicationFactory)
        {
            _dbCommFactory = dbCommunicationFactory;
            _dbComm = _dbCommFactory.Create(AppSettings.ConnectionStrings.MainDB);
        }

        public User GetUser(int userId)
        {
            var param = new Dictionary<string, object>() { { "user_id", userId } };
            List<UserDAO> usersDAO = _dbComm.ExecuteGet<UserDAO>("STP_GET_USER", param);

            return new User(usersDAO.First());
        }

        public int GetTodayPostsUsage(int userId)
        {
            User user = GetUser(userId);

            return user.TodayPosts;
        }
    }
}
