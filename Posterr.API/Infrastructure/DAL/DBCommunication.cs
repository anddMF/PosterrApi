using Posterr.API.Infrastructure.Factories;
using Posterr.API.Interfaces;
using System.Data;

namespace Posterr.API.Infrastructure.DAL
{
    public class DBCommunication : IDBCommunication
    {
        private ConnectionFactory _connection;
        public DBCommunication(string connectionString)
        {
            _connection = new ConnectionFactory(connectionString);
        }

        /// <summary>
        /// Executes a procedure that will make operations on the database, i.e. insert, update or delete.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic ExecuteOperation(string name, Dictionary<string, object> param)
        {
            var result = _connection.ExecuteScalar(name, CommandType.StoredProcedure, param);
            return result;
        }

        /// <summary>
        /// Executes and returns select queries.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> ExecuteGet<T>(string name, Dictionary<string, dynamic> param)
        {
            DataTable dt = new DataTable();
            var sqlResult = _connection.GetReader(name, CommandType.StoredProcedure, param);
            dt.Load(sqlResult);

            var result = TranslateDataTable<T>(dt);
            return result.ToList();
        }

        /// <summary>
        /// Make the transformation of the DB result for a object from a class inside the code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <returns></returns>
        private IEnumerable<T> TranslateDataTable<T>(dynamic db)
        {
            var dt = (DataTable)db;

            var columns = dt.Columns.Cast<DataColumn>().Select(d => new { d.DataType, d.ColumnName }).ToList();
            var result = new List<T>();

            foreach (DataRow item in dt.Rows)
            {
                var entity = Activator.CreateInstance(typeof(T));

                foreach (var column in columns)
                {
                    var type = Type.GetTypeCode(column.DataType);
                    var value = item[column.ColumnName];

                    if (type == TypeCode.UInt64)
                    {
                        if (value == null || DBNull.Value.Equals(value))
                        {
                            value = false;
                        }
                        else
                        {
                            value = Convert.ToBoolean(value);
                        }

                    }

                    if (value == null || DBNull.Value.Equals(value))
                    {
                        value = ValidateNullValue(column.DataType.Name);
                    }

                    entity?.GetType()?.GetProperty(column.ColumnName).SetValue(entity, value);
                }
                result.Add((T)entity);
            }

            return result;
        }

        private dynamic ValidateNullValue(string type)
        {
            switch (type)
            {
                case "Int32":
                    return 0;

                case "String":
                    return "";

                case "DateTime":
                    return null;

                case "Boolean":
                    return false;

                default:
                    return null;
            }
        }
    }
}
