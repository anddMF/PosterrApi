﻿using Posterr.API.Entities;
using Posterr.API.Infrastructure.DAL.DAO;
using Posterr.API.Infrastructure.DAL;
using Posterr.API.Interfaces;

namespace Posterr.API.Services
{
    public class PostService : IPostService
    {
        private readonly IDBCommunicationFactory _dbCommFactory;
        private IDBCommunication _dbComm;

        public PostService(IDBCommunicationFactory dbCommunicationFactory)
        {
            _dbCommFactory = dbCommunicationFactory;
            _dbComm = _dbCommFactory.Create(AppSettings.ConnectionStrings.MainDB);
        }

        /// <summary>
        /// Retrieves a list of posts based on the provided query parameters.
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public List<Post> GetPosts(PostQueryParameters queryParameters)
        {
            Dictionary<string, dynamic> param = ParamGetPosts(queryParameters.GetAll, queryParameters.PageNumber, queryParameters.PageSize, queryParameters.UserId, queryParameters.StartDate, queryParameters.EndDate);
            List<PostDAO> postsDAO = _dbComm.ExecuteGet<PostDAO>("STP_GET_POSTS", param);

            List<Post> response = TransformFromDAO(postsDAO);

            return response;
        }

        /// <summary>
        /// Inserts a new post into the database.
        /// </summary>
        /// <param name="body"></param>
        /// <exception cref="Exception"></exception>
        public void InsertPost(PostBody body)
        {
            if (body.IdType > 1 && !body.IdOriginalPost.HasValue)
                throw new Exception("Posts that are not from type ORIGINAL require an IdOriginalPost");

            Dictionary<string, dynamic> param = ParamInsertPost(body);
            _dbComm.ExecuteOperation("STP_INSERT_POST", param);
        }

        private Dictionary<string, dynamic> ParamGetPosts(bool getAll, int pageNumber, int pageSize, int? userId, DateTime startDate, DateTime? endDate)
        {
            return new Dictionary<string, dynamic>
            {
                { "page_number", pageNumber },
                { "page_size", pageSize },
                { "user_id", getAll ? null : userId },
                { "start_date", startDate },
                { "end_date", endDate }
            };
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

        /// <summary>
        /// Converts a PostDAO list into a Post list.
        /// </summary>
        /// <param name="daoList"></param>
        /// <returns></returns>
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
    }
}
