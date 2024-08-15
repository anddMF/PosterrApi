using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Posterr.API.Entities;
using Posterr.API.Infrastructure.DAL.DAO;
using Posterr.API.Interfaces;
using Posterr.API.Services;

namespace Posterr.API.Test
{
    public class PostServiceTest
    {
        private static readonly Random _random = new Random();
        private readonly IServiceProvider _serviceProvider;

        public PostServiceTest()
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
        public void GetPosts_OK()
        {
            var mockResturnDAO = SetupListPostDAO();
            Mock<IDBCommunicationFactory> mockFactory = new Mock<IDBCommunicationFactory>();
            Mock<IDBCommunication> mock = new Mock<IDBCommunication>();
            mock.Setup(x => x.ExecuteGet<PostDAO>(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>())).Returns(mockResturnDAO);
            mockFactory.Setup(y => y.Create(It.IsAny<string>())).Returns(mock.Object);

            PostService service = new PostService(mockFactory.Object);
            var response = TransformFromDAO(mockResturnDAO);

            //Act
            var act = service.GetPosts(new PostQueryParameters());

            //Assert
            Assert.Equal(act.Count, mockResturnDAO.Count);
            for(int i = 0; i < act.Count; i++)
            {
                Assert.Equal(act[i].Username, response[i].Username);
                Assert.Equal(act[i].Id, response[i].Id);
                Assert.Equal(act[i].IdType, response[i].IdType);
                Assert.Equal(act[i].Content, response[i].Content);
            }
        }

        [Fact]
        public void InsertPost_NOK()
        {
            Mock<IDBCommunicationFactory> mockFactory = new Mock<IDBCommunicationFactory>();
            Mock<IDBCommunication> mock = new Mock<IDBCommunication>();
            mock.Setup(x => x.ExecuteOperation(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>())).Returns(true);
            mockFactory.Setup(y => y.Create(It.IsAny<string>())).Returns(mock.Object);

            PostService service = new PostService(mockFactory.Object);

            //Act
            var exception = Assert.Throws<Exception>(() => service.InsertPost(SetupPostBodyNOK()));

            // Assert
            Assert.Equal("Posts that are not from type ORIGINAL require an IdOriginalPost", exception.Message);
        }

        [Fact]
        public void InsertPostCalledMethods_OK()
        {
            Mock<IDBCommunicationFactory> mockFactory = new Mock<IDBCommunicationFactory>();
            Mock<IDBCommunication> mock = new Mock<IDBCommunication>();
            mockFactory.Setup(y => y.Create(It.IsAny<string>())).Returns(mock.Object);
            PostService service = new PostService(mockFactory.Object);

            var postBody = new PostBody
            {
                IdType = 1,
                IdOriginalPost = null
            };

            var param = ParamInsertPost(postBody);

            mock.Setup(db => db.ExecuteOperation("STP_INSERT_POST", It.IsAny<Dictionary<string, dynamic>>()));

            // Act
            service.InsertPost(postBody);

            // Assert
            mock.Verify(db => db.ExecuteOperation("STP_INSERT_POST", param), Times.Once);
        }

        private List<PostDAO> SetupListPostDAO()
        {
            List<PostDAO> response = new List<PostDAO>();

            int counter = 3;

            for (int i = 0; i < counter; i++)
            {
                int idOriginalPost = _random.Next(0, 4);
                int idSecondPost = _random.Next(0, 4);
                var obj = new PostDAO
                {
                    id = i + 1,
                    id_type = _random.Next(1, 4),
                    id_user = _random.Next(1, 4),
                    username = Guid.NewGuid().ToString().Substring(0, 8),
                    content = Guid.NewGuid().ToString().Substring(0, 8),
                    name = "ORIGINAL",
                    id_original_post = idOriginalPost,
                    post_date = DateTime.Now,
                    second_content = "",
                    second_type = _random.Next(1, 4),
                    second_username = Guid.NewGuid().ToString().Substring(0, 8),
                    second_post_date = DateTime.Now,
                    second_id_original = idOriginalPost > 0 ? idSecondPost : 0,
                    first_content = Guid.NewGuid().ToString().Substring(0, 8),
                    first_type = _random.Next(1, 4),
                    first_post_date = DateTime.Now,
                    first_username = Guid.NewGuid().ToString().Substring(0, 8)                    
                };

                response.Add(obj);
            }

            return response;
        }

        private PostBody SetupPostBodyNOK()
        {
            return new PostBody
            {
                IdType = 3,
                Content = Guid.NewGuid().ToString().Substring(0, 8),
                IdUser = 1
            };
        }

        private List<Post> TransformFromDAO(List<PostDAO> daoList)
        {
            List<Post> response = new List<Post>();

            for (int i = 0; i < daoList.Count; i++)
            {
                PostDAO current = daoList[i];
                Post converted = new Post(current);
                response.Add(converted);
            }

            return response;
        }

        private Dictionary<string, dynamic> ParamInsertPost(PostBody obj)
        {
            return new Dictionary<string, dynamic>
            {
                { "pid_type", obj.IdType },
                { "pid_user", obj.IdUser },
                { "pid_original_post", obj.IdOriginalPost },
                { "pcontent", obj.Content },
                { "ppost_date", obj.PostDate }
            };
        }
    }
}
