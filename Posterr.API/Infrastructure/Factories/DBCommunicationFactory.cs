using Posterr.API.Infrastructure.DAL;
using Posterr.API.Interfaces;

namespace Posterr.API.Infrastructure.Factories
{
    public class DBCommunicationFactory : IDBCommunicationFactory
    {
        public IDBCommunication Create(string connectionString)
        {
            return new DBCommunication(connectionString);
        }
    }
}
