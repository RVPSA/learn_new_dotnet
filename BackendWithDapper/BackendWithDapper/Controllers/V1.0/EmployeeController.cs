using Microsoft.AspNetCore.Mvc;
using Service.Services.IService;

namespace BackendWithDapper.Controllers.V1._0
{
    [Route("[Controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService) {
            _employeeService = employeeService;
        }
        [HttpGet("GetAllEmployee")]
        public IActionResult GetAllEmployee(int pageSize,int pageNumber) {
            try
            {
                return Ok(_employeeService.GetAllEmployees(pageSize,pageNumber));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
