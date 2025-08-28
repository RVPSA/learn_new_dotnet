using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL.Core
{
    public class DapperDbConnection : IDapperDbConnection
    {
        public readonly string _connectionString;

        public DapperDbConnection() => _connectionString = "Server=localhost\\SQLEXPRESS;Database=Dev;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"; 

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
