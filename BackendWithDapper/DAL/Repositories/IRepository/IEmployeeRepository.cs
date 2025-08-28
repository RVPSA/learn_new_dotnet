using Models.Employee;

namespace DAL.Repositories.IRepository
{
    public interface IEmployeeRepository
    {
        List<EmployeeData> GetAllEmployee(int pageSize, int pageNumber);
    }
}
