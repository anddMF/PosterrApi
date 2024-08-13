namespace Posterr.API.Entities
{
    public class AppSettings
    {
        public static AppConnections ConnectionStrings { get; set; }
    }

    public class AppConnections
    {
        public string MainDB { get; set; }
        public string SecondaryDB { get; set; }
    }
}
