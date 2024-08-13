namespace Posterr.API.Infrastructure.DAL.DAO
{
    public class PostDAO
    {
        public int id { get; set; }
        public int id_type { get; set; }
        public string name { get; set; }
        public int id_user { get; set; }
        public string username { get; set; }
        public int id_original_post { get; set; }
        public string content { get; set; }
        public DateTime post_date { get; set; }
        public string second_content { get; set; }
        public int second_type { get; set; }
        public string second_username { get; set; }
        public DateTime second_post_date { get; set; }
        public int second_id_original { get; set; }
        public string first_content { get; set; }
        public int first_type { get; set; }
        public string first_username { get; set; }
        public DateTime first_post_date { get; set; }

    }
}
