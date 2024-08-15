using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Posterr.API.Entities;
using Posterr.API.Infrastructure.DAL.DAO;
using Posterr.API.Interfaces;
using Posterr.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.API.Test
{
    public class UserServiceTest
    {
        private static readonly Random _random = new Random();
        private readonly IServiceProvider _serviceProvider;

        public UserServiceTest()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();

            var services = new ServiceCollection();

            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appSettings);

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetUser_OK()
        {
            int randomId = _random.Next(1, 9);
            var mockResturnDAO = SetupListUserDAO(randomId);
            Mock<IDBCommunicationFactory> mockFactory = new Mock<IDBCommunicationFactory>();
            Mock<IDBCommunication> mock = new Mock<IDBCommunication>();
            mock.Setup(x => x.ExecuteGet<UserDAO>(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>())).Returns(mockResturnDAO);
            mockFactory.Setup(y => y.Create(It.IsAny<string>())).Returns(mock.Object);

            UserService service = new UserService(mockFactory.Object);

            //Act
            var act = service.GetUser(randomId);

            //Assert
            Assert.Equal(randomId, act.Id);
            mock.Verify(dbComm => dbComm.ExecuteGet<UserDAO>(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }

        [Fact]
        public void GetTodayPostsUsage_OK()
        {
            int randomId = _random.Next(1, 9);
            int randomUsage = _random.Next(1, 9);
            var mockResturnDAO = SetupListUserDAO(randomId, randomUsage);
            Mock<IDBCommunicationFactory> mockFactory = new Mock<IDBCommunicationFactory>();
            Mock<IDBCommunication> mock = new Mock<IDBCommunication>();
            mock.Setup(x => x.ExecuteGet<UserDAO>(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>())).Returns(mockResturnDAO);
            mockFactory.Setup(y => y.Create(It.IsAny<string>())).Returns(mock.Object);

            UserService service = new UserService(mockFactory.Object);

            //Act
            var act = service.GetTodayPostsUsage(randomId);

            //Assert
            Assert.Equal(randomUsage, act);
            mock.Verify(dbComm => dbComm.ExecuteGet<UserDAO>(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()), Times.Once);
        }

        private List<UserDAO> SetupListUserDAO(int intendedId, int intendedTodayPosts = 0)
        {
            var response = new List<UserDAO>();

            response.Add(new UserDAO() { id = intendedId, registration_date = DateTime.Now, today_posts = intendedTodayPosts <= 0 ? _random.Next(0, 10) : intendedTodayPosts });

            return response;
        }
    }
}
