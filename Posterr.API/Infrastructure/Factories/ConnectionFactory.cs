using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace Posterr.API.Infrastructure.Factories
{
    public class ConnectionFactory
    {
        private MySqlConnection Connection;
        private string connString;

        public ConnectionFactory(string connectionString)
        {
            connString = connectionString;
        }

        private MySqlConnection GetConnection()
        {

            if (Connection == null)
                Connection = new MySqlConnection(connString);

            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            return Connection;
        }

        public MySqlCommand GetCommand()
        {
            return GetConnection().CreateCommand();
        }

        public void Dispose()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
                Connection.Close();
            Connection.Dispose();
        }

        public DbDataReader GetReader(string cmdText,
            CommandType cmdType = CommandType.Text,
            Dictionary<string, object> param = null
            )
        {
            using (var cmd = this.GetCommand())
            {
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (param != null)
                {
                    foreach (var pr in param)
                    {
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = pr.Key;
                        parameter.Value = pr.Value;
                        if (pr.Value != null && pr.Value.GetType().Name == "Boolean")
                        {
                            parameter.MySqlDbType = MySqlDbType.Bit;
                        }
                        cmd.Parameters.Add(parameter);
                    }
                }

                return cmd.ExecuteReader(); //SELECT
            }
        }

        public bool ExecuteNonQuery(string cmdText,
            CommandType cmdType = CommandType.Text,
            Dictionary<string, object> param = null)
        {
            using (var cmd = this.GetCommand())
            {
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (param != null)
                {
                    foreach (var pr in param)
                    {
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = pr.Key;
                        parameter.Value = pr.Value;
                        if (pr.Value != null && pr.Value.GetType().Name == "Boolean")
                        {
                            parameter.MySqlDbType = MySqlDbType.Bit;
                        }
                        cmd.Parameters.Add(parameter);
                    }
                }

                cmd.ExecuteNonQuery(); //INSERT, DELETE, UPDATE
                return true;
            }
        }

        public object ExecuteScalar(string cmdText,
            CommandType cmdType = CommandType.Text,
            Dictionary<string, object> param = null)
        {
            using (var cmd = this.GetCommand())
            {
                cmd.CommandText = cmdText;
                cmd.CommandType = cmdType;

                if (param != null)
                {
                    foreach (var pr in param)
                    {
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = pr.Key;
                        parameter.Value = pr.Value;

                        if (pr.Value != null && pr.Value.GetType().Name == "Boolean")
                        {
                            parameter.MySqlDbType = MySqlDbType.Bit;
                        }
                        cmd.Parameters.Add(parameter);
                    }
                }

                return cmd.ExecuteScalar();
            }
        }
    }
}
