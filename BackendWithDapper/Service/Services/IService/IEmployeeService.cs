using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Employee;

namespace Service.Services.IService
{
    public interface IEmployeeService
    {
        List<EmployeeData> GetAllEmployees(int pageSize, int pageNumber);
    }
}
