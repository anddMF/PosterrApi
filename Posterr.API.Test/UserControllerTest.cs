using Microsoft.AspNetCore.Mvc;
using Moq;
using Posterr.API.Controllers;
using Posterr.API.Entities;
using Posterr.API.Infrastructure.DAL.DAO;
using Posterr.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Posterr.API.Test
{
    public class UserControllerTest
    {
        private static readonly Random _random = new Random();

        [Fact]
        public void GetUser_OK()
        {
            int randomId = _random.Next(1, 9);
            Mock<IUserService> mock = new Mock<IUserService>();
            mock.Setup(x => x.GetUser(It.IsAny<int>())).Returns(SetupUser(randomId));

            UserController controller = new UserController(mock.Object);

            //Act
            var act = controller.GetUser(randomId);
            var result = act.Result as OkObjectResult;

            //Assert
            User response = Assert.IsType<User>(result.Value);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response.Id, randomId);
            mock.Verify(service => service.GetUser(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetTodayPostUsage_OK()
        {
            int randomId = _random.Next(1, 9);
            int randomUsage = _random.Next(1, 9);
            User setup = SetupUser(randomId, randomUsage);
            Mock<IUserService> mock = new Mock<IUserService>();
            mock.Setup(x => x.GetTodayPostsUsage(It.IsAny<int>())).Returns(randomUsage);

            UserController controller = new UserController(mock.Object);

            //Act
            var act = controller.GetTodayPostUsage(randomId);
            var result = act.Result as OkObjectResult;

            //Assert
            int response = Assert.IsType<int>(result.Value);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response, randomUsage);
            mock.Verify(service => service.GetTodayPostsUsage(It.IsAny<int>()), Times.Once);
        }

        private User SetupUser(int intendedId, int intendedTodayPosts = 0)
        {
            return new User() { Id = intendedId, RegistrationDate = DateTime.Now, TodayPosts = intendedTodayPosts <= 0 ? _random.Next(0, 10) : intendedTodayPosts, Username = Guid.NewGuid().ToString().Substring(0, 14) };
        }
    }
}
