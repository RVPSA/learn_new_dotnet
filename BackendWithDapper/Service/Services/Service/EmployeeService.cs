using DAL.Repositories.IRepository;
using Models.Employee;
using Service.Services.IService;

namespace Service.Services.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository) {
            _employeeRepository = employeeRepository;
        }
        public List<EmployeeData> GetAllEmployees(int pageSize, int pageNumber) {
            return _employeeRepository.GetAllEmployee(pageSize, pageNumber);
        }
    }
}
