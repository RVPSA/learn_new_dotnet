using System.Data;

namespace DAL.Core
{
    public interface IDapperDbConnection
    {
        public IDbConnection CreateConnection();
    }
}
