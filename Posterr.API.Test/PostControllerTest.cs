using Microsoft.AspNetCore.Mvc;
using Posterr.API.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Posterr.API.Entities;
using Posterr.API.Controllers;
using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI.Common;

namespace Posterr.API.Test
{
    public class PostControllerTest
    {
        private static readonly Random _random = new Random();

        [Fact]
        public void GetPosts_OK()
        {
            var setupParameter = new PostQueryParameters() { GetAll = true, StartDate = DateTime.Now, PageSize = It.IsAny<int>(), PageNumber = It.IsAny<int>() };
            var setup = SetupListPost();
            Mock<IPostService> mock = new Mock<IPostService>();
            mock.Setup(x => x.GetPosts(setupParameter)).Returns(setup);

            PostController controller = new PostController(mock.Object);

            //Act
            var act = controller.GetPosts(setupParameter);
            var result = act.Result as OkObjectResult;
            List<Post> response = (List<Post>)result.Value;

            //Assert
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(response.Count, setup.Count);
            for (int i = 0; i < response.Count; i++)
            {
                Assert.Equal(response[i].Username, setup[i].Username);
                Assert.Equal(response[i].Id, setup[i].Id);
                Assert.Equal(response[i].IdType, setup[i].IdType);
                Assert.Equal(response[i].Content, setup[i].Content);
            }
        }

        [Fact]
        public void GetPosts_NOK()
        {
            var setupParameter = new PostQueryParameters() { GetAll = true, StartDate = DateTime.Now, PageSize = It.IsAny<int>(), PageNumber = It.IsAny<int>() };
            var setup = SetupListPost();
            Mock<IPostService> mock = new Mock<IPostService>();
            mock.Setup(x => x.GetPosts(setupParameter)).Throws(new Exception("Error message to assert"));

            PostController controller = new PostController(mock.Object);

            //Act and Assert
            var exception = Assert.Throws<Exception>(() => controller.GetPosts(setupParameter));
            Assert.Equal("Error message to assert", exception.Message);
            mock.Verify(service => service.GetPosts(setupParameter), Times.Once);
        }

        [Fact]
        public void GetPostsBadRequest_OK()
        {
            Mock<IPostService> mock = new Mock<IPostService>();
            PostController controller = new PostController(mock.Object);

            //Act
            var act = controller.GetPosts(new PostQueryParameters());
            var res = act.Result as BadRequestObjectResult;

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, res.StatusCode);
            Assert.Equal("UserId must be valid if param GetAll is false", res.Value);
        }

        [Fact]
        public void InsertPost_OK()
        {
            var setup = SetupPostBody();
            Mock<IPostService> mock = new Mock<IPostService>();
            mock.Setup(x => x.InsertPost(setup));

            PostController controller = new PostController(mock.Object);

            // Act
            var result = controller.Post(setup);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            mock.Verify(service => service.InsertPost(setup), Times.Once);
        }

        [Fact]
        public void InsertPost_NOK()
        {
            var setup = SetupPostBody();
            Mock<IPostService> mock = new Mock<IPostService>();
            mock.Setup(x => x.InsertPost(setup));
            PostController controller = new PostController(mock.Object);

            mock.Setup(service => service.InsertPost(setup)).Throws(new Exception("Insertion error message"));

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => controller.Post(setup));
            Assert.Equal("Insertion error message", exception.Message);
            mock.Verify(service => service.InsertPost(setup), Times.Once);
        }

        private List<Post> SetupListPost()
        {
            var response = new List<Post>();
            int counter = 3;

            for (int i = 0; i < counter; i++)
            {
                Post obj = new Post()
                {
                    Id = i,
                    Content = Guid.NewGuid().ToString().Substring(0, 18),
                    IdUser = i,
                    IdType = _random.Next(1, 4),
                    PostDate = DateTime.Now,
                    Username = Guid.NewGuid().ToString().Substring(0, 14)
                };
                response.Add(obj);
            }

            return response;
        }

        private PostBody SetupPostBody()
        {
            return new PostBody
            {
                IdType = 1,
                Content = "Test"
            };
        }
    }
}
