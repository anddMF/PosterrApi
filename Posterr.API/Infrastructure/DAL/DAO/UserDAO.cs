namespace Posterr.API.Infrastructure.DAL.DAO
{
    public class UserDAO
    {
        public int id { get; set; }
        public string username { get; set; }
        public int today_posts { get; set; }
        public int total_posts { get; set; }
        public DateTime registration_date { get; set; }
    }
}
