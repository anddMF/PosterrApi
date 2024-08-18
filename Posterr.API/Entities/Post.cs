using DataAnnotationsExtensions;
using Posterr.API.Infrastructure.DAL.DAO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Posterr.API.Entities
{
    public class Post
    {
        public int? Id { get; set; }

        [Range(1, 3, ErrorMessage = "IdType must be valid")]
        public int IdType { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PostType? TypeName => Enum.IsDefined(typeof(PostType), IdType) ? (PostType)IdType : null;

        [Min(1, ErrorMessage = "IdUser must be a valid number")]
        public int? IdUser { get; set; }
        public string? Username { get; set; }
        public int? IdOriginalPost { get; set; }
        public string? Content { get; set; }
        public DateTime PostDate { get; set; }
        public Post? OriginalPost { get; set; }
        public Post? MiddlePost { get; set; }

        public Post()
        {
        }

        public Post(int id, string content, string username, DateTime postDate, int idType)
        {
            Id = id;
            Content = content;
            Username = username;
            PostDate = postDate;
            IdType = idType;
        }

        public Post(PostDAO dao)
        {
            Id = dao.id;
            IdType = dao.id_type;
            IdUser = dao.id_user;
            Username = dao.username;
            IdOriginalPost = dao.id_original_post;
            Content = dao.content;
            PostDate = dao.post_date;

            if (dao.id_original_post > 0)
            {
                // post with 3 layers
                if (dao.second_id_original > 0)
                {
                    AddMiddlePost(dao.id_original_post, dao.second_content, dao.second_username, dao.second_post_date, dao.second_type);
                    AddOriginalPost(dao.second_id_original, dao.first_content, dao.first_username, dao.first_post_date, dao.first_type);
                }
                else
                {
                    AddOriginalPost(dao.id_original_post, dao.second_content, dao.second_username, dao.second_post_date, dao.second_type);
                }
            }
        }

        /// <summary>
        /// Add original post on the OriginalPost feature.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="username"></param>
        /// <param name="postDate"></param>
        /// <param name="idType"></param>
        public void AddOriginalPost(int id, string content, string username, DateTime postDate, int idType)
        {
            OriginalPost = new Post(id, content, username, postDate, idType);
        }

        /// <summary>
        /// Add middle layer post on the MiddlePost feature.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="username"></param>
        /// <param name="postDate"></param>
        /// <param name="idType"></param>
        public void AddMiddlePost(int id, string content, string username, DateTime postDate, int idType)
        {
            MiddlePost = new Post(id, content, username, postDate, idType);
        }
    }

    public enum PostType
    {
        ORIGINAL = 1,
        REPOST = 2,
        QUOTE = 3
    }
}
