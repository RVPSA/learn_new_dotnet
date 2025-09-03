using Microsoft.AspNetCore.Mvc;
using Models.Employee;
using Redis;
using Service.Services.IService;

namespace BackendWithDapper.Controllers.V1._0
{
    [Route("[Controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService, IRedisCacheService redisCacheService)
        : Controller
    {
        [HttpGet("GetAllEmployee")]
        public IActionResult GetAllEmployee(int pageSize,int pageNumber) {
            try
            {
                var employee = redisCacheService.GetData<IEnumerable<EmployeeData>>("employee"); //Get data from the cache
                
                if(employee is not null)
                    return Ok(employee);

                employee = employeeService.GetAllEmployees(pageSize, pageNumber);
                
                redisCacheService.SetData("employee", employee); //If not adding data to cache
                return Ok(employee);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
