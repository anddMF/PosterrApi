namespace Posterr.API.Interfaces
{
    public interface IDBCommunicationFactory
    {
        IDBCommunication Create(string connectionString);
    }
}
