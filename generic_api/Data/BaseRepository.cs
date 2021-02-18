using Microsoft.Data.SqlClient;
using System.Data;

namespace generic_api.Data
{
    public class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected IDbConnection GetConn()
        {
            var conn = new SqlConnection { ConnectionString = _connectionString };
            conn.Open();

            return conn;
        }
    }
}
