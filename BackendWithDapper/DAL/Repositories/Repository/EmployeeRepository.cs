using System.Data;
using DAL.Core;
using DAL.Repositories.IRepository;
using Dapper;
using Models.Employee;

namespace DAL.Repositories.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDapperDbConnection _dbConnection;

        public EmployeeRepository(IDapperDbConnection dapperDbConnection) => _dbConnection = dapperDbConnection;

        public List<EmployeeData> GetAllEmployee(int pageSize, int pageNumber)
        {
            using (IDbConnection dbConnection = _dbConnection.CreateConnection()) {
                var result = dbConnection.Query<EmployeeData>(
                    "[dbo].[GetAllEmployees]",
                    param: new {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                    },
                    commandType: CommandType.StoredProcedure
                ).ToList();

                return result;
            }
        }
    }
}
