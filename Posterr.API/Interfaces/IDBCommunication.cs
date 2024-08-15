namespace Posterr.API.Interfaces
{
    public interface IDBCommunication
    {
        public dynamic ExecuteOperation(string name, Dictionary<string, object> param);
        public List<T> ExecuteGet<T>(string name, Dictionary<string, dynamic> param);
    }
}
